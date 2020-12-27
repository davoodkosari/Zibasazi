using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static Radyn.Congress.Tools.Enums;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.Facade
{
    public sealed class ArticleFacade : CongressBaseFacade<Article>, IArticleFacade
    {
        public ArticleFacade()
        {
        }

        internal ArticleFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }



        private void UserFillComplex(Article item, Configuration config)
        {



            item.AllowSent = config.AllowGetArticleOrginal();
            item.AllowPrintCertificate = config.AllowUserPrintCertification && (config.ArticleCertificateState == null || item.FinalState == config.ArticleCertificateState);
            item.AllowDelete = config.CanUserDeleteArticle;
            if (!config.HasArticlePayment)
            {
                return;
            }

            if ((item.PayStatus == null || item.PayStatus == (byte)Enums.ArticlepayState.PayWait ||
                 item.PayStatus == (byte)Enums.ArticlepayState.PayDenial) &&
                config.ArticlePaymentStep == (byte)Enums.ArticlePaymentSteps.AfterFinalState &&
                item.FinalState == (byte)Enums.FinalState.Confirm && item.TransactionId == null)
            {
                item.PaymentVisibility = true;
            }

            if ((item.PayStatus == null || item.PayStatus == (byte)Enums.ArticlepayState.PayWait ||
                 item.PayStatus == (byte)Enums.ArticlepayState.PayDenial) &&
                config.ArticlePaymentStep == (byte)Enums.ArticlePaymentSteps.BeforSendArticle &&
                item.TransactionId == null)
            {
                item.PaymentVisibility = true;
            }
        }



        public bool UserInsert(Article article, List<ArticleAuthors> articleAuthorses,
            HttpPostedFileBase abstractFileId,
            HttpPostedFileBase orginalFileId, FormStructure formModel)
        {
            bool result = false;
            ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs = new ModelView.InFormEntitiyList<RefereeCartable>();
            ModelView.InFormEntitiyList<Article> articles = new ModelView.InFormEntitiyList<Article>();
            ArticleBO articleBo = new ArticleBO();
            try
            {

                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !articleBo.UserInsert(ConnectionHandler, FileManagerConnection,
                        FormGeneratorConnection, article, articleAuthorses, abstractFileId, orginalFileId,
                        formModel))
                {
                    return false;
                }

                if (!articleBo.SendArticle(ConnectionHandler, FileManagerConnection, article, keyValuePairs))
                {
                    return false;
                }

                ConnectionHandler.CommitTransaction();
                FileManagerConnection.CommitTransaction();
                FormGeneratorConnection.CommitTransaction();
                result = true;
                articles.Add(
                article, Resources.Congress.ArticleInsertEmail,
                    Resources.Congress.ArticleInsertSMS
                );
            }
            catch (KnownException)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                articleBo.InformArticle(ConnectionHandler, article.CongressId, articles);
                new RefereeBO().InformRefereeAddArticle(ConnectionHandler, article.CongressId, keyValuePairs);
            }
            catch (Exception)
            {

            }
            return result;
        }


        public bool UserInsert(ref Article article, List<ArticleAuthors> articleAuthorses,
            HttpPostedFileBase abstractFileId,
            HttpPostedFileBase orginalFileId, List<DiscountType> transactionDiscountAttaches, string callbackurl,
            FormStructure formModel)
        {

            try
            {
                ArticleBO articleBo = new ArticleBO();
                ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs = new ModelView.InFormEntitiyList<RefereeCartable>();
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!articleBo.UserInsert(ConnectionHandler, FileManagerConnection,
                        FormGeneratorConnection, article, articleAuthorses, abstractFileId, orginalFileId,
                        formModel))
                {
                    return false;
                }

                if (!articleBo.SendArticle(ConnectionHandler, FileManagerConnection, article, keyValuePairs))
                {
                    return false;
                }

                article = new ArticleBO().Get(ConnectionHandler, article.Id);
                bool articlePaymnet = new ArticleBO().ArticlePaymnet(ConnectionHandler, PaymentConnection,
                   ref article, transactionDiscountAttaches, callbackurl + article.Id);
                if (!articlePaymnet)
                {
                    throw new Exception(string.Format(Resources.Congress.ErrorInPayArticleAmount,
                        Extention.GetAtricleTitle(article.CongressId)));
                }

                ConnectionHandler.CommitTransaction();
                FileManagerConnection.CommitTransaction();
                PaymentConnection.CommitTransaction();
                FormGeneratorConnection.CommitTransaction();
                return true;


            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                PaymentConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                PaymentConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }


        }

        public IEnumerable<ModelView.UserArticleAbstract> SearchArticle(Guid congressId, Article article, string serachvalue, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow)
        {

            try
            {

                ArticleBO articleBo = new ArticleBO();
                List<Article> list = articleBo.Search(ConnectionHandler, congressId, article, serachvalue, ascendingDescending, articleflow, formStructure);
                if (!list.Any())
                {
                    return null;
                }

                List<ModelView.UserArticleAbstract> outlist = new List<ModelView.UserArticleAbstract>();
                List<ArticleAuthors> articleAuthorses = new ArticleAuthorsBO().Where(ConnectionHandler, c => c.ArticleId.In(list.Select(i => i.Id)));
                Homa homa = new HomaBO().Get(ConnectionHandler, congressId);
                ConfigurationContent configcontent = new ConfigurationContentBO().Get(ConnectionHandler, congressId, homa.Configuration.CardLanguageId);
                List<Guid> @select = new RefereeCartableBO().Select(ConnectionHandler, x => x.ArticleId, x => x.ArticleId.In(list.Select(i => i.Id)));
                foreach (Article item in list)
                {
                    ModelView.UserArticleAbstract model = new ModelView.UserArticleAbstract();
                    item.HasRefereeOpinion = @select.Any(x => x.Equals(item.Id));
                    item.HasRefereeAttachment = item.HasRefereeOpinion;
                    item.AllowPrintCertificate = true;

                    List<ArticleAuthors> authorbo = articleAuthorses.Where(c => c.ArticleId == item.Id).OrderBy(c => c.Order).ToList();
                    string auters = "";
                    if (string.IsNullOrEmpty(item.Title))
                    {
                        foreach (ArticleAuthors author in authorbo)
                        {
                            auters += "," + author.Name + ":" + (author.IsDirector == false ? "" : "Director" + ":") + author.Address;
                        }
                        if (!string.IsNullOrEmpty(auters))
                        {
                            model.Authors = auters.Substring(1);
                        }
                    }
                    if (string.IsNullOrEmpty(auters))
                    {
                        foreach (ArticleAuthors author in authorbo)
                        {
                            auters += "," + author.Name + ":" + (author.IsDirector == false ? "" : "مسوول" + ":") + author.Address;
                        }
                        if (!string.IsNullOrEmpty(auters))
                        {
                            model.Authors = auters.Substring(1);
                        }
                    }
                    if (configcontent != null && configcontent.LogoId.HasValue && configcontent.Logo != null)
                    {
                        model.CongressLogo = configcontent.Logo.Content;
                    }

                    model.Id = item.Id.ToString();
                    model.OrginalTextFile = item.ArticleOrginalText == null ? "" : item.ArticleOrginalText.RemoveHtml();
                    model.Abstract = item.Abstract == null ? "" : item.Abstract.RemoveHtml();
                    model.CongressTitle = homa.CongressTitle;
                    model.Description = item.Description;
                    model.Title = item.Title;
                    model.Keyword = item.Keyword;
                    if (item.Pivot != null)
                    {
                        model.Pivot = item.Pivot.Title;
                    }

                    outlist.Add(model);

                }
                return outlist;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


        public bool ArticlePaymnet(Article article, List<DiscountType> transactionDiscountAttaches, string callbackurl)
        {
            bool Resultpaymnet;
            ModelView.InFormEntitiyList<Article> articles = new ModelView.InFormEntitiyList<Article>();

            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new ArticleBO().ArticlePaymnet(ConnectionHandler, PaymentConnection, ref article,
                        transactionDiscountAttaches, callbackurl))
                {
                    return false;
                }

                ConnectionHandler.CommitTransaction();
                PaymentConnection.CommitTransaction();
                Resultpaymnet = true;
                articles.Add(article, Resources.Congress.ArticlePaymentEmail, Resources.Congress.ArticlePaymentSMS);
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (Resultpaymnet)
                {
                    new ArticleBO().InformArticle(ConnectionHandler, article.CongressId, articles);
                }
            }
            catch (Exception)
            {


            }
            return Resultpaymnet;
        }
        public bool AdminUpdate(Article obj)
        {

            try
            {
                ArticleBO articleBo = new ArticleBO();
                articleBo.AdminSetStatus(obj);
                return base.Update(obj);
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


        public bool AdminUpdate(Guid adminId, Article obj, List<ArticleAuthors> articleAuthorses,
             string comment, HttpPostedFileBase flowFile, HttpPostedFileBase orginalFileId, HttpPostedFileBase abstractFileId, FormStructure formModel)
        {
            bool result;
            ArticleBO articleBo = new ArticleBO();
            ModelView.InFormEntitiyList<Article> articles = new ModelView.InFormEntitiyList<Article>();

            try
            {

                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                Article oldsatus = articleBo.Get(ConnectionHandler, obj.Id);
                articleBo.AdminUpdate(ConnectionHandler, FileManagerConnection, FormGeneratorConnection,adminId, obj, articleAuthorses, orginalFileId, abstractFileId,comment,flowFile, formModel);
                ConnectionHandler.CommitTransaction();
                FileManagerConnection.CommitTransaction();
                FormGeneratorConnection.CommitTransaction();
                result = true;
                if (oldsatus.Status != obj.Status)
                {
                    articles.Add(obj, Resources.Congress.ArticleChangeStatusEmail, Resources.Congress.ArticleChangeStatusSMS);
                }
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                articleBo.InformArticle(ConnectionHandler, obj.CongressId, articles);
            }
            catch (Exception)
            {


            }
            return result;

        }

        


        public bool UserUpdate(Article article, List<ArticleAuthors> articleAuthorses,
            HttpPostedFileBase abstractFileId,
            HttpPostedFileBase orginalFileId, FormStructure formModel)
        {
            ArticleBO articleBo = new ArticleBO();

            try
            {

                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                if (
                    !articleBo.UserUpdate(ConnectionHandler, FileManagerConnection,
                        FormGeneratorConnection, article, articleAuthorses, abstractFileId, orginalFileId,
                        formModel))
                {
                    return false;
                }

                ConnectionHandler.CommitTransaction();
                FileManagerConnection.CommitTransaction();
                FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool UpdateStatus(Guid congressId, List<Guid> guids, FinalState status)
        {
            ModelView.InFormEntitiyList<Article> articles = new ModelView.InFormEntitiyList<Article>();
            bool result;
            ArticleBO articleBo = new ArticleBO();
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!guids.Any())
                    return true;

                List<Article> list = articleBo.Where(ConnectionHandler, x => x.Id.In(guids));
                foreach (Article item in list)
                {
                    item.FinalState = (byte)status;
                    articleBo.AdminSetStatus(item);
                    articleBo.Update(ConnectionHandler, item);
                    articles.Add(item, Resources.Congress.ArticleChangeStatusEmail, Resources.Congress.ArticleChangeStatusSMS);
                }
                ConnectionHandler.CommitTransaction();
                result = true;
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                {
                    articleBo.InformArticle(ConnectionHandler, congressId, articles);
                }
            }
            catch (Exception)
            {


            }
            return result;
        }
        public bool AdminUpdateArticles(Guid congressId, List<Article> model)
        {

            ModelView.InFormEntitiyList<Article> articles = new ModelView.InFormEntitiyList<Article>();
            bool result;
            ArticleBO articleBo = new ArticleBO();
            
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var transactionTransactionalFacade = PaymentComponenets.Instance.TransactionTransactionalFacade(PaymentConnection);
                var list = articleBo.Where(ConnectionHandler, x => x.Id.In(model.Select(article => article.Id)));
                foreach (Article obj in list)
                {

                    articleBo.AdminSetStatus(obj);
                    if (obj.PayStatus.HasValue)
                    {
                        obj.PayStatus = obj.PayStatus;
                        if (obj.PayStatus == (byte)Enums.ArticlepayState.PayConfirm&& obj.TransactionId != null)
                            transactionTransactionalFacade.Done((Guid) obj.TransactionId);

                    }
                    articleBo.Update(ConnectionHandler, obj);
                    articles.Add(obj, Resources.Congress.ArticleChangeStatusEmail, Resources.Congress.ArticleChangeStatusSMS);
                }
                ConnectionHandler.CommitTransaction();
                result = true;
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                {
                    articleBo.InformArticle(ConnectionHandler, congressId, articles);
                }
            }
            catch (Exception)
            {


            }
            return result;

        }








        public bool SentArticle(Article article, HttpPostedFileBase orginalFileId, FormStructure formModel)
        {


            ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs = new ModelView.InFormEntitiyList<RefereeCartable>();
            bool result;
            RefereeBO refereeBo = new RefereeBO();
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                if (orginalFileId != null)
                {
                    article.OrginalFileId =
                       FileManagerComponent.Instance.FileTransactionalFacade(FileManagerConnection)
                           .Insert(orginalFileId);

                }
                new ArticleBO().SetStatus(article);
                ArticleBO articleBo = new ArticleBO();
                FormGeneratorComponent.Instance.FormDataTransactionalFacade(FormGeneratorConnection).ModifyFormData(formModel);
                articleBo.Update(ConnectionHandler, article);
                articleBo.SendArticle(ConnectionHandler, FileManagerConnection, article, keyValuePairs);
                ConnectionHandler.CommitTransaction();
                FileManagerConnection.CommitTransaction();
                FormGeneratorConnection.CommitTransaction();
                result = true;
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                FileManagerConnection.RollBack();
                FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                {
                    refereeBo.InformRefereeAddArticle(ConnectionHandler, article.CongressId, keyValuePairs);
                }
            }
            catch (Exception)
            {


            }
            return result;

        }



        public List<Article> Search(Guid congressId, Article article, string serachvalue,
            FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow)
        {
            try
            {



                List<Article> searchArticleList = new ArticleBO().Search(ConnectionHandler, congressId, article, serachvalue, ascendingDescending, articleflow);
                if (!searchArticleList.Any())
                {
                    return searchArticleList;
                }

                List<Guid> @select = new RefereeCartableBO().Select(ConnectionHandler, x => x.ArticleId,
                    x => x.ArticleId.In(searchArticleList.Select(i => i.Id)), true);
                foreach (Article item in searchArticleList)
                {
                    item.HasRefereeOpinion = @select.Any(x => x.Equals(item.Id));
                    item.HasRefereeAttachment = item.HasRefereeOpinion;
                    item.AllowPrintCertificate = true;
                    item.Abstract = item.Abstract == null ? "" : item.Abstract.RemoveHtml();
                    item.ArticleOrginalText = item.ArticleOrginalText == null ? "" : item.ArticleOrginalText.RemoveHtml();
                    if (!string.IsNullOrEmpty(item.Title))
                    {
                        continue;
                    }
                }
                return searchArticleList;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }


        }

        public List<dynamic> SearchDynamic(Guid congressId, Article article, string serachvalue,
          FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow)
        {
            try
            {



                List<dynamic> searchArticleList = new ArticleBO().SearchDynamic(ConnectionHandler, congressId, article, serachvalue, ascendingDescending, articleflow);
                if (!searchArticleList.Any())
                {
                    return searchArticleList;
                }

                List<Guid> @select = new List<Guid>();
                if (searchArticleList.Any())
                {
                    List<Guid> idlist = searchArticleList.Select(user1 => (Guid)user1.Id).ToList();
                    @select = new RefereeCartableBO().Select(ConnectionHandler, x => x.ArticleId,
                  x => x.ArticleId.In(idlist), true);


                }

                foreach (dynamic item in searchArticleList)
                {
                    item.HasRefereeOpinion = @select.Any(x => x.Equals(item.Id));
                    item.HasRefereeAttachment = item.HasRefereeOpinion;
                    item.AllowPrintCertificate = true;
                    item.Abstract = item.Abstract == null ? "" : ((string)item.Abstract).RemoveHtml();
                    item.ArticleOrginalText = item.ArticleOrginalText == null ? "" : ((string)item.ArticleOrginalText).RemoveHtml();

                }
                return searchArticleList;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }


        }
        public List<Tools.ModelView.ArticleCertificateModel> GetArticleCertificate(Homa homa, Guid id,
            bool isadmin)
        {
            try
            {

                return new ArticleBO().GetArticleCertificate(ConnectionHandler, id, homa, isadmin);

            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public List<Tools.ModelView.ArticleCertificateModel> GetAllArticleCertificate(Homa homa, Article article,
            string serachvalue, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow)
        {
            try
            {

                List<ModelView.ArticleCertificateModel> list = new List<Tools.ModelView.ArticleCertificateModel>();

                ArticleBO articleBo = new ArticleBO();
                List<Article> search = articleBo.Search(ConnectionHandler, homa.Id, article, serachvalue, ascendingDescending, articleflow, formStructure);
                foreach (Article item in search)
                {
                    list.AddRange(articleBo.GetArticleCertificate(ConnectionHandler, item.Id, homa, true));
                }
                return list;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<Article> GetByUserId(Configuration configuration, Guid id)
        {
            try
            {
                ArticleBO articleBo = new ArticleBO();
                List<Article> list = articleBo.Where(ConnectionHandler, x => x.UserId == id);
                foreach (Article article in list)
                {
                    UserFillComplex(article, configuration);

                }

                return list.OrderByDescending(x => x.Code);

            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Article> GetAllForZipFile(Guid congressId, Article article, string serachvalue,
            FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow)
        {
            try
            {


                List<Article> search = new ArticleBO().Search(ConnectionHandler, congressId, article, serachvalue, ascendingDescending, articleflow, formStructure);
                if (!search.Any())
                {
                    return null;
                }

                List<Guid> @select = new RefereeCartableBO().Select(ConnectionHandler, x => x.ArticleId,
                   x => x.ArticleId.In(search.Select(i => i.Id)), true);
                foreach (Article item in search)
                {
                    if (!string.IsNullOrEmpty(item.Abstract))
                    {
                        item.Abstract = Regex.Replace(item.Abstract, "<.*?>", "");
                    }

                    item.HasRefereeOpinion = @select.Any(x => x.Equals(item.Id));
                    item.HasRefereeAttachment = item.HasRefereeOpinion;
                    item.AllowPrintCertificate = true;
                    item.Abstract = item.Abstract == null ? "" : item.Abstract.RemoveHtml();
                    item.ArticleOrginalText = item.ArticleOrginalText == null ? "" : item.ArticleOrginalText.RemoveHtml();
                }
                return search;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


        public Guid UpdateStatusAfterTransaction(Guid congressId, Guid id)
        {
            ModelView.ModifyResult<Article> afterTransactionModel;
            ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs = new ModelView.InFormEntitiyList<RefereeCartable>();
            try
            {

                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                Article article = new ArticleBO().Get(ConnectionHandler, id);
                if (article == null || article.TempId == null)
                {
                    return Guid.Empty;
                }

                afterTransactionModel = new ArticleBO().UpdateStatusAfterTransaction(ConnectionHandler, PaymentConnection, FileManagerConnection, (Guid)article.TempId, keyValuePairs);
                ConnectionHandler.CommitTransaction();
                PaymentConnection.CommitTransaction();
                FileManagerConnection.CommitTransaction();


            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                PaymentConnection.RollBack();
                FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                PaymentConnection.RollBack();
                FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {

                new RefereeBO().InformRefereeAddArticle(ConnectionHandler, congressId, keyValuePairs);


            }
            catch (Exception)
            {

            }
            try
            {
                if (afterTransactionModel.SendInform)
                {
                    new ArticleBO().InformArticle(ConnectionHandler, congressId, afterTransactionModel.InformList);

                }

            }
            catch (Exception)
            {

            }
            return afterTransactionModel == null ? Guid.Empty : afterTransactionModel.TransactionId;

        }

        public override bool Delete(Article obj)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new ArticleBO().Delete(ConnectionHandler,obj))
                {
                    throw new Exception(Resources.Congress.ErrorInDeleteArticle);
                }
                ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }


        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticleDayCount(Guid congressId, string moth,
            string year)
        {
            try
            {
                return new ArticleBO().ChartArticleDayCount(ConnectionHandler, congressId, moth, year);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticleMothCount(Guid congressId, string year)
        {
            try
            {
                return new ArticleBO().ChartArticleMothCount(ConnectionHandler, congressId, year);

            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticlePivotCount(Guid congressId)
        {
            try
            {
                return new ArticleBO().ChartArticlePivotCount(ConnectionHandler, congressId);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticlePivotCategoryCount(Guid congressId)
        {
            try
            {
                return new ArticleBO().ChartArticlePivotCategoryCount(ConnectionHandler, congressId);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticleStatusCount(Guid congressId)
        {
            try
            {
                return new ArticleBO().ChartArticleStatusCount(ConnectionHandler, congressId);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartArticleTypeCount(Guid congressId)
        {
            try
            {
                return new ArticleBO().ChartArticleTypeCount(ConnectionHandler, congressId);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


    }
}
