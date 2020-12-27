using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Excel;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class RefereeBO : BusinessBase<Referee>
    {

        public List<Referee> SearchRefree(IConnectionHandler connectionHandler, string text, Guid congressId)
        {
            var predicateBuilder = new PredicateBuilder<Referee>();
            predicateBuilder.And(x => x.CongressId == congressId);
            if (!string.IsNullOrEmpty(text))
            {
                var txtSearch = text.ToLower();
                predicateBuilder.And((x => x.Username.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.LastName.Contains(txtSearch)
                || x.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(txtSearch) || x.EnterpriseNode.Address.Contains(txtSearch)
                || x.EnterpriseNode.Website.Contains(txtSearch) || x.EnterpriseNode.Email.Contains(txtSearch) || x.EnterpriseNode.Tel.Contains(txtSearch)));

            }

            return new RefereeBO().Where(connectionHandler, predicateBuilder.GetExpression());

        }

        public override Referee Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var item = base.Get(connectionHandler, keys);
            if (item != null)
                SetArticleCount(connectionHandler, new List<Referee>() { item }, item.CongressId);
            return item;
        }

        public void SetArticleCount(IConnectionHandler connectionHandler, List<Referee> list, Guid congressId)
        {

            var groupBy = new RefereeCartableBO().GroupBy(connectionHandler,
              new Expression<Func<RefereeCartable, object>>[] { x => x.RefereeId },
              new GroupByModel<RefereeCartable>[]
              {
                    new GroupByModel<RefereeCartable>()
                    {
                        Expression = x => x.ArticleId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    }
              }, x => x.Referee.CongressId == congressId);


            var groupBy2 = new RefereeCartableBO().GroupBy(connectionHandler,
             new Expression<Func<RefereeCartable, object>>[] { x => x.RefereeId },
             new GroupByModel<RefereeCartable>[]
             {
                    new GroupByModel<RefereeCartable>()
                    {
                        Expression = x => x.ArticleId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    }
             }, x => x.Referee.CongressId == congressId && x.Status == (byte)Enums.FinalState.WaitForAnswer);
            foreach (var item in list)
            {
                var firstOrDefault = groupBy.FirstOrDefault(x => x.RefereeId == item.Id);
                if (firstOrDefault != null)
                    item.AllArticleCount = firstOrDefault.CountArticleId;
                var @default = groupBy2.FirstOrDefault(x => x.RefereeId == item.Id);
                if (@default != null)
                    item.WaitForAnswerArticleCount = @default.CountArticleId;
            }



        }

        internal bool Insert(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, Referee referee,  System.Web.HttpPostedFileBase fileBase=null, List<Guid> pivots=null)
        {

            var id = referee.Id;
            BOUtility.GetGuidForId(ref id);
            referee.Id = id;
            referee.EnterpriseNode.Id = referee.Id;
            if (Any(connectionHandler1, x => x.EnterpriseNode.Email == referee.EnterpriseNode.Email && x.CongressId == referee.CongressId))
                throw new Exception(Resources.Congress.RefereeEmailIsRepeat);
            if (Any(connectionHandler1, x => x.Username == referee.Username.ToLower() && x.CongressId == referee.CongressId))
                throw new Exception(Resources.Congress.RefereeUserNameIsRepeat);
            if (!EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeconnection).Insert(referee.EnterpriseNode, fileBase))
                throw new Exception(Resources.Congress.ErrorInSaveReferee);
            if (!string.IsNullOrEmpty(referee.Password))
                referee.Password = StringUtils.HashPassword(referee.Password);
            if (!base.Insert(connectionHandler1, referee))
                throw new Exception(Resources.Congress.ErrorInSaveReferee);
            var refereePivotBo = new RefereePivotBO();
            refereePivotBo.Insert(connectionHandler1, referee.Id, pivots);
              
            return true;
        }

        internal bool Update(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, Referee referee,  System.Web.HttpPostedFileBase fileBase=null, List<Guid> pivots=null)
        {
            var enterpriseNodeTransactionalFacade = EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeconnection);
            var node = enterpriseNodeTransactionalFacade.Get(referee.Id);
            var referee1 = this.Get(connectionHandler1, referee.Id);
            if (node != null && node.Email.ToLower() != referee.EnterpriseNode.Email.ToLower() &&
                Any(connectionHandler1, x => x.EnterpriseNode.Email == referee.EnterpriseNode.Email && x.CongressId == referee.CongressId && x.Id != referee1.Id))
                throw new Exception(Resources.Congress.YouhavealreadyregisteredCongress);
            if (referee1 != null && referee1.Username.ToLower() != referee.Username.ToLower() &&
               Any(connectionHandler1, x => x.Username == referee.Username.ToLower() && x.CongressId == referee.CongressId && x.Id != referee.Id))
                throw new Exception(Resources.Congress.UserNameIsRepeate);
            if (!enterpriseNodeTransactionalFacade.Update(referee.EnterpriseNode, fileBase))
                throw new Exception(Resources.Congress.ErrorInEditReferee);
            if (!base.Update(connectionHandler1, referee)) throw new Exception(Resources.Congress.ErrorInEditReferee);
            var refereePivotBo = new RefereePivotBO();
            refereePivotBo.Update(connectionHandler1, referee.Id, pivots);
            return true;

        }



      
        public void InformRefereeRegister(IConnectionHandler connectionHandler,Guid congressId, ModelView.InFormEntitiyList<Referee> valuePairs)
        {
            if (!valuePairs.Any()) return;
            
            var config = new ConfigurationBO().Get(connectionHandler, congressId);
            if (config.RefereeInformType == null) return;
            var homa1 = new HomaBO().Get(connectionHandler, config.CongressId);
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Type == Enums.MessageInformType.Referee);
            var @where = this.Where(connectionHandler, x => x.Id.In(valuePairs.Select(i => i.obj.Id)));
            foreach (var valuePair in valuePairs)
            {
                var referee = @where.FirstOrDefault(x => x.Id == valuePair.obj.Id);
                if (referee == null) continue;
                var name = referee.EnterpriseNode.DescriptionFieldWithGender;
                var homaRefereePanelUrl = homa1.GetHomaRefereePanelUrl();
                var homaCompleteUrl = homa1.GetHomaCompleteUrl();
                var email = string.Format(valuePair.EmailBody, homa1.CongressTitle, name, homaCompleteUrl, homaRefereePanelUrl, valuePair.obj.Username, valuePair.obj.PasswordWithoutHash);
                var sms = string.Format(valuePair.SmsBody, homa1.CongressTitle, name, homaRefereePanelUrl, valuePair.obj.Username, valuePair.obj.PasswordWithoutHash);

                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{Enums.RefereeMessageKey.FullName.ToString()}]", name);
                        email = email.Replace($"[{Enums.RefereeMessageKey.Username.ToString()}]", referee.Username);
                        email = email.Replace($"[{Enums.RefereeMessageKey.Email.ToString()}]", referee.EnterpriseNode.Email);
                        email = email.Replace($"[{Enums.RefereeMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        email = email.Replace($"[{Enums.RefereeMessageKey.Password.ToString()}]", valuePair.obj.PasswordWithoutHash);
                        email = email.Replace($"[{Enums.RefereeMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);




                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{Enums.RefereeMessageKey.FullName.ToString()}]", name);
                        sms = sms.Replace($"[{Enums.RefereeMessageKey.Username.ToString()}]", referee.Username);
                        sms = sms.Replace($"[{Enums.RefereeMessageKey.Email.ToString()}]", referee.EnterpriseNode.Email);
                        sms = sms.Replace($"[{Enums.RefereeMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        sms = sms.Replace($"[{Enums.RefereeMessageKey.Password.ToString()}]", valuePair.obj.PasswordWithoutHash);
                        sms = sms.Replace($"[{Enums.RefereeMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);

                    }

                }


                var inform = new Message.Tools.ModelView.MessageModel()
                {
                    Email = referee.EnterpriseNode.Email,
                    Mobile = referee.EnterpriseNode.Cellphone,
                    EmailTitle = homa1.DescriptionField,
                    EmailBody = email,
                    SMSBody = sms
                };
                new HomaBO().SendInform((byte)config.RefereeInformType, inform, config, homa1.CongressTitle);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa1.OwnerId, config.CongressId,
              new[] { referee.Id.ToString() }, homa1.CongressTitle, inform.SMSBody);
            }
        }
        public void InformRefereeAddArticle(IConnectionHandler connectionHandler,Guid congressId, ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs)
        {

            if (!keyValuePairs.Any()) return;
            var refereeBo = new RefereeBO();
            var arti = keyValuePairs.FirstOrDefault();
            var referee = refereeBo.Get(connectionHandler, arti.obj.RefereeId);
            var homa1 = new HomaBO().Get(connectionHandler, congressId);
            var articleBo = new ArticleBO();
            var config = homa1.Configuration;
            if (config.RefereeInformType == null) return;
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Type == Enums.MessageInformType.RefereeArticle);
            foreach (var article in keyValuePairs)
            {
                var article1 = articleBo.Get(connectionHandler, article.obj.ArticleId);
                if (article1 == null) continue;
                var name = referee.EnterpriseNode.DescriptionFieldWithGender;
                var homaCompleteUrl = homa1.GetHomaCompleteUrl();
                var homaArticleRefereePanelUrl = homa1.GetHomaArticleRefereePanelUrl(article1.Id, referee.Id);
                var email = string.Format(article.EmailBody, homa1.CongressTitle, name, article1.Title, homaCompleteUrl,homaArticleRefereePanelUrl);
                var sms = string.Format(article.SmsBody, homa1.CongressTitle, name, article1.Code);

                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{Enums.RefereeArticleMessageKey.FullName.ToString()}]", name);
                        email = email.Replace($"[{Enums.RefereeArticleMessageKey.Username.ToString()}]", referee.Username);
                        email = email.Replace($"[{Enums.RefereeArticleMessageKey.ArticleCode.ToString()}]", article1.Code.ToString());
                        email = email.Replace($"[{Enums.RefereeArticleMessageKey.ArticleTitle.ToString()}]", article1.Title);
                        email = email.Replace($"[{Enums.RefereeArticleMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        email = email.Replace($"[{Enums.RefereeArticleMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);




                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{Enums.RefereeArticleMessageKey.FullName.ToString()}]", name);
                        sms = sms.Replace($"[{Enums.RefereeArticleMessageKey.Username.ToString()}]", referee.Username);
                        sms = sms.Replace($"[{Enums.RefereeArticleMessageKey.ArticleCode.ToString()}]", article1.Code.ToString());
                        sms = sms.Replace($"[{Enums.RefereeArticleMessageKey.ArticleTitle.ToString()}]", article1.Title);
                        sms = sms.Replace($"[{Enums.RefereeArticleMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        sms = sms.Replace($"[{Enums.RefereeArticleMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);

                    }

                }


                var inform = new Message.Tools.ModelView.MessageModel()
                {
                    Email = referee.EnterpriseNode.Email,
                    Mobile = referee.EnterpriseNode.Cellphone,
                    EmailTitle = homa1.CongressTitle,
                    EmailBody = email,
                    SMSBody = sms

                };
                new HomaBO().SendInform((byte)config.RefereeInformType, inform, config, homa1.CongressTitle);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa1.OwnerId, config.CongressId,
                   new[] { referee.EnterpriseNode.Id.ToString() }, homa1.CongressTitle, inform.SMSBody);
            }
        }

    
       
        internal Dictionary<Referee, List<string>> ImportFromExcel(IConnectionHandler connectionHandler, HttpPostedFileBase fileBase, Guid congressId)
        {
            try
            {
                var keyValuePairs = new Dictionary<Referee, List<string>>();
                if (fileBase == null) return keyValuePairs;
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileBase.InputStream);
                excelReader.IsFirstRowAsColumnNames = true;
                var result = excelReader.AsDataSet();
                if (result == null) return keyValuePairs;
                var refereeBo = new RefereeBO();
                foreach (DataTable table in result.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var resultStatus = new List<string>();
                        var referee = new Referee()
                        {
                            Id = Guid.NewGuid(),
                            CongressId = congressId,
                            EnterpriseNode =
                                new EnterpriseNode.DataStructure.EnterpriseNode()
                                {
                                    RealEnterpriseNode = new RealEnterpriseNode()
                                }
                        };

                        if (string.IsNullOrEmpty(row[0].ToString())) resultStatus.Add(Resources.Congress.PleaseEnterYourEmail);
                        else
                        {
                            referee.EnterpriseNode.Email = row[0].ToString();
                            referee.Username = row[0].ToString();
                            if (!Utils.IsEmail(referee.EnterpriseNode.Email))
                                resultStatus.Add(Resources.Congress.UnValid_Enter_Email);
                            var byUserName = refereeBo.FirstOrDefault(connectionHandler, x => x.EnterpriseNode.Email == referee.EnterpriseNode.Email & x.CongressId == congressId);
                            if (byUserName != null)
                            {
                                resultStatus.Add(Resources.Congress.UserNameIsRepeate);
                                referee = byUserName;
                                referee.State = Framework.ObjectState.Dirty;
                                
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(referee.Username))
                                    referee.Password = StringUtils.HashPassword(referee.Username.Substring(0, 5));
                            }
                        }
                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourName);
                            referee.EnterpriseNode.RealEnterpriseNode.FirstName = string.Empty;
                        }
                        else referee.EnterpriseNode.RealEnterpriseNode.FirstName = row[1].ToString();
                        if (string.IsNullOrEmpty(row[2].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourLastName);
                            referee.EnterpriseNode.RealEnterpriseNode.LastName = string.Empty;
                        }
                        else referee.EnterpriseNode.RealEnterpriseNode.LastName = row[2].ToString();
                        if (string.IsNullOrEmpty(row[3].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourGender);
                            referee.EnterpriseNode.RealEnterpriseNode.Gender = null;
                        }
                        else
                        {
                            switch (row[3].ToString().ToLower())
                            {
                                case "men":
                                case "مرد":
                                    referee.EnterpriseNode.RealEnterpriseNode.Gender = true;
                                    break;
                                case "women":
                                case "زن":
                                    referee.EnterpriseNode.RealEnterpriseNode.Gender = false;
                                    break;
                                default:
                                    referee.EnterpriseNode.RealEnterpriseNode.Gender = null;
                                    break;
                            }
                        }
                        if (string.IsNullOrEmpty(row[4].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourMobile);
                            referee.EnterpriseNode.Cellphone = string.Empty;
                        }
                        else referee.EnterpriseNode.Cellphone = row[4].ToString();

                        if (!string.IsNullOrEmpty(row[5].ToString()))
                        {
                            var NationalCode = row[5].ToString().ToLong();
                            var national = string.Format("{0:D10}", NationalCode);
                            if (!Radyn.Utility.Utils.ValidNationalID(national))
                                resultStatus.Add("کد ملی صحیح نمیباشد");
                            else
                                referee.EnterpriseNode.RealEnterpriseNode.NationalCode = national;
                        }
                        else referee.EnterpriseNode.RealEnterpriseNode.NationalCode = string.Empty;
                        referee.EnterpriseNode.Address = !string.IsNullOrEmpty(row[6].ToString()) ? row[6].ToString() : string.Empty;

                        keyValuePairs.Add(referee, resultStatus);
                    }
                }
                return keyValuePairs;
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); throw new KnownException(ex.Message, ex);
            }
        }
    }
}
