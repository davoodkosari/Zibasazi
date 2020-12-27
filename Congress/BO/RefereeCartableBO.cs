using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Radyn.Congress.DA;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Evaluation.Models;
using Radyn.FormGenerator;
using Radyn.FormGenerator.ControlFactory.Base;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;


namespace Radyn.Congress.BO
{
    internal class RefereeCartableBO : BusinessBase<RefereeCartable>
    {
        public override bool Insert(IConnectionHandler connectionHandler, RefereeCartable obj)
        {
            obj.Id = Guid.NewGuid();
            obj.InsertDate = DateTime.Now;
            return base.Insert(connectionHandler, obj);
        }

        public bool ModifyCartabl(IConnectionHandler connectionHandler, Guid articleId, Guid refreeId, byte state, bool visited , bool isActive)
        {
            var cartabl = this.FirstOrDefault(connectionHandler, x => x.RefereeId == refreeId && x.ArticleId == articleId);
            if (cartabl == null)
            {
                var ct = new RefereeCartable
                {
                    ArticleId = articleId,
                    Visited = false,
                    Status = state,
                    RefereeId = refreeId,
                    IsActive = true
                };
                return Insert(connectionHandler, ct);
            }
            cartabl.Visited = visited;
            cartabl.Status = state;
            cartabl.IsActive = isActive;
            if (!this.Update(connectionHandler, cartabl))
                throw new Exception(Resources.Congress.ErrorInEditRefereeCartabl);
            return true;
        }


        public bool AnswerArticle(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, Guid congressId,
            RefereeCartable refereeCartable, Guid answeredrefreeId, string comments,
        HttpPostedFileBase attachment)
        {
            var articleBo = new ArticleBO();
            var articleFlowBo = new ArticleFlowBO();
            var config = new ConfigurationBO().ValidConfig(connectionHandler, congressId);
            var refree = new RefereeBO().Get(connectionHandler, answeredrefreeId);


            if (!this.ModifyCartabl(connectionHandler, refereeCartable.ArticleId, answeredrefreeId,
                  refereeCartable.Status, refereeCartable.Visited,false))
                return false;

            var article = articleBo.Get(connectionHandler, refereeCartable.ArticleId);
            article.FinalState = (byte)Enums.FinalState.WaitForAnswer;
            if (config.SentArticleSpecialReferee)
                article.Status = refree.IsSpecial
                    ? (byte)Enums.ArticleState.WaitForScientificTeacher
                    : (byte)Enums.ArticleState.WaitforSpecialRefereeOpinion;
            else
                article.Status = (byte)Enums.ArticleState.WaitForScientificTeacher;

            if (!articleBo.Update(connectionHandler, article))
                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleCongress, Extention.GetAtricleTitle(article.CongressId)));

            var lastSenderId = new ArticleFlowBO().SelectFirstOrDefault(connectionHandler, x => x.SenderId,
                 x => x.ReceiverId == answeredrefreeId && x.ArticleId == article.Id,
                 new OrderByModel<ArticleFlow>()
                 {
                     Expression = x => x.SaveDate + "" + x.SaveTime,
                     OrderType = OrderType.DESC
                 });
            

            var lastrefree = new RefereeBO().Get(connectionHandler, lastSenderId);
            if (lastrefree != null && lastrefree.IsSpecial)
            {
                var lastrefereeCartable = new RefereeCartable();
                lastrefereeCartable = this.FirstOrDefaultWithOrderByDescending(connectionHandler, x => x.InsertDate,
                    x => x.ArticleId == article.Id && x.RefereeId == lastrefree.Id);

                if (lastrefereeCartable != null)
                {
                    if (!this.ModifyCartabl(connectionHandler, lastrefereeCartable.ArticleId, lastSenderId,
                (byte)Enums.FinalState.RefereeAnswered, lastrefereeCartable.Visited, false))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!this.ModifyCartabl(connectionHandler, article.Id, lastSenderId, (byte)Enums.FinalState.WaitForAnswer, false,true))
                        return false;
                }

                if (!articleFlowBo.AddFlow(connectionHandler, filemanagerconnection, answeredrefreeId, lastSenderId != Guid.Empty ? lastSenderId : (Guid?)null, refereeCartable.ArticleId,
                    refereeCartable.Status, comments, attachment))
                    return false;
            }
            else
            {

                var articleFlow =
                    articleFlowBo.FirstOrDefaultWithOrderBy(connectionHandler, x => x.SaveDate + " " + x.SaveTime,
                        x => x.ArticleId == refereeCartable.ArticleId && x.ReceiverId == answeredrefreeId);
                var senderId = articleFlow != null ? articleFlow.SenderId : (Guid?)null;
                if (!articleFlowBo.AddFlow(connectionHandler, filemanagerconnection, answeredrefreeId, senderId,
                    refereeCartable.ArticleId, refereeCartable.Status, comments, attachment))
                    return false;
            }

            return true;
        }
        public ModelView.ModifyResult<RefereeCartable> SpecialRefereeAssignArticle(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, Guid congressId, RefereeCartable refereeCartable, Guid answeredrefreeId, string comments,
        HttpPostedFileBase attachment, List<Guid> refreeIdlist)
        {
            if (!this.ModifyCartabl(connectionHandler, refereeCartable.ArticleId, answeredrefreeId, refereeCartable.Status, refereeCartable.Visited,false)) return new ModelView.ModifyResult<RefereeCartable>();
            return this.AssigneArticleToRefreeCartabl(connectionHandler, filemanagerconnection, refereeCartable.ArticleId, answeredrefreeId, refreeIdlist, true);
        }
        public ModelView.ModifyResult<RefereeCartable> AssigneArticleToRefreeCartabl(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconection, Guid articleId, Guid flowsender, List<Guid> refereesId, bool specialrefereesender = false)
        {
            var keyValuePairs = new ModelView.ModifyResult<RefereeCartable>();
            var articleBo = new ArticleBO();
            var article = articleBo.Get(connectionHandler, articleId);
            var config = new ConfigurationBO().ValidConfig(connectionHandler, article.CongressId);
            article.FinalState = (byte)Enums.FinalState.WaitForAnswer;
            bool articleSpecialReferee = config.SentArticleSpecialReferee;
            if (refereesId.Count > 0)
            {
                if (config.SentArticleSpecialReferee)
                {
                    if (specialrefereesender)
                    {
                        articleSpecialReferee = false;
                        article.Status = (byte)Enums.ArticleState.WaitForRefereeOpinion;
                    }
                    else
                        article.Status = (byte)Enums.ArticleState.SentToSpecialReferee;
                }
                else
                    article.Status = (byte)Enums.ArticleState.WaitForRefereeOpinion;
            }
            if (!articleBo.Update(connectionHandler, article)) return keyValuePairs;
            if (
                !this.UpdateRefreeCartabl(connectionHandler, filemanagerconection,
                    article, flowsender, refereesId, articleSpecialReferee))
                return keyValuePairs;
            foreach (var guid in refereesId)
            {
                keyValuePairs.AddInform(
                new RefereeCartable() { RefereeId = guid, ArticleId = articleId }, Resources.Congress.RefereeInformArticleEmail,
 Resources.Congress.RefereeInformArticleSMS);
            }

            keyValuePairs.Result = true;
            keyValuePairs.SendInform = true;
            return keyValuePairs;

        }
        public bool Refereeopinion(IConnectionHandler connectionHandler, IConnectionHandler formGeneratorConnection, RefereeCartable refereeCartable, FormStructure postFormData)
        {

            if (refereeCartable == null) return false;
            postFormData.RefId = refereeCartable.RefereeId + "," + refereeCartable.ArticleId;
            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection).ModifyFormData(postFormData))
                throw new Exception(Resources.Congress.ErrorInEditRefereeCartabl);

            var formStructureTransactionalFacade = FormGeneratorComponent.Instance.FormStructureTransactionalFacade(formGeneratorConnection);
            var formStructure = formStructureTransactionalFacade.Get(postFormData.Id);
            if (formStructure.Controls.Any())
            {
                var controlsWeight = formStructureTransactionalFacade.GetFormControlsWeightInform(formStructure);
                refereeCartable.Score = SetContractorGradByTopsis(formGeneratorConnection, postFormData.RefId, formStructure, controlsWeight);
            }
            if (!this.Update(connectionHandler, refereeCartable))
                throw new Exception(Resources.Congress.ErrorInEditRefereeCartabl);
            return true;
        }
        public double? SetContractorGradByTopsis(IConnectionHandler connectionHandler, string refId, FormStructure formStructure, Dictionary<string, double> controlsWeight)
        {
            double score = 0;
            var formEvaluationBo = FormGeneratorComponent.Instance.FormEvaluationTransactionalFacade(connectionHandler);
            var formData = FormGeneratorComponent.Instance.FormDataTransactionalFacade(connectionHandler).GetWithoutGridFormData(formStructure, refId, new Referee().GetType().Name + "opinion", formStructure.CurrentUICultureName);
            foreach (var control in formStructure.Controls)
            {
                var key = ((Control)control).Id;
                if (formData != null && formData.GetFormControl.ContainsKey(key))
                {
                    var value = formData.GetFormControl[key];
                    if (value != null)
                    {
                        var d1 = controlsWeight.ContainsKey(key) ? controlsWeight[key] : 0;
                        var controlWeight = formEvaluationBo.GetControlWeight(control, value);
                        score += controlWeight * d1;

                    }
                }
            }
            return Math.Round(score, 2);


        }
        public bool DeleteFromRefreeCartable(IConnectionHandler connectionHandler, List<Guid> list, Guid refreeId)
        {
            var da = new RefereeCartableDA(connectionHandler);
            foreach (var item in list)
            {
                if (!da.DeleteFromRefreeCartable(item, refreeId))
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateRefreeCartabl(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnectionHandler, Article article, Guid flowsender, List<Guid> refereesId, bool sendtospecialreferee)
        {
            var oldAssignedList = this.Where(connectionHandler, x => x.ArticleId == article.Id);
            var senderCartabel = this.FirstOrDefault(connectionHandler,
                x => x.ArticleId == article.Id && x.RefereeId == flowsender && x.IsActive);
            var special = oldAssignedList.FirstOrDefault(x => x.RefereeId == flowsender);
            if (special != null)
            {
                oldAssignedList.Remove(special);
            }

            if (oldAssignedList.Any())
            {
                if (refereesId.Any())
                {
                    foreach (var item in refereesId)
                    {
                        if (oldAssignedList.Any(x => x.RefereeId == item))
                        {
                            var obj = oldAssignedList.First(x => x.RefereeId == item);
                            oldAssignedList.Remove(obj);
                        }
                    }
                }

                foreach (var item in oldAssignedList)
                {
                    item.IsActive = false;
                    if (!this.Update(connectionHandler, item))
                        throw new Exception(string.Format(Resources.Congress.ErrorInDeleteArticleFromRefereecartabl, Extention.GetAtricleTitle(article.CongressId)));
                }
            }

            if (senderCartabel != null)
            {
                senderCartabel.Status = (byte)Enums.FinalState.SendToReferee;
                if (!this.Update(connectionHandler, senderCartabel))
                    throw new Exception(string.Format(Resources.Congress.ErrorInDeleteArticleFromRefereecartabl, Extention.GetAtricleTitle(article.CongressId)));
            }

            foreach (var reciverId in refereesId)
            {
                if (!this.AssigneToCartable(connectionHandler, filemanagerconnectionHandler, flowsender, reciverId, article))
                    return false;
            }
            return true;
        }

        public bool AssigneToCartable(IConnectionHandler connectionHandler,
            IConnectionHandler filemanagerconnectionHandler, Guid sender, Guid reciverId,
            Article article)
        {
            if (!this.ModifyCartabl(connectionHandler, article.Id, reciverId,
                (byte)Enums.FinalState.WaitForAnswer, false,true))
                throw new Exception(string.Format(Resources.Congress.ErrorInDeleteArticleFromRefereecartabl, Extention.GetAtricleTitle(article.CongressId)));
            if (
                !new ArticleFlowBO().AddFlow(connectionHandler, filemanagerconnectionHandler, sender, reciverId,
                    article.Id, (byte)Enums.FinalState.WaitForAnswer, ((Enums.ArticleState)article.Status).GetDescriptionInLocalization()))
                throw new Exception(Resources.Congress.ErrorInSaveArticleFlow);
            return true;
        }


    }
}
