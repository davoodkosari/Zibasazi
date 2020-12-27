using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
//using Radyn.FCM;
using Enums = Radyn.Congress.Tools.Enums;
using ModelView = Radyn.Congress.Tools.ModelView;

namespace Radyn.Congress.Facade
{
    internal sealed class UserFacade : CongressBaseFacade<User>, IUserFacade
    {
        internal UserFacade()
        {
        }

        internal UserFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public override User Get(params object[] keys)
        {
            var item = base.Get(keys);
            if (item != null)
            {
                item.RegisterInNewsLetter = item.EnterpriseNode != null
                   && !string.IsNullOrEmpty(item.EnterpriseNode.Email) && new NewsLetterBO().Any(this.ConnectionHandler, x => x.CongressId == item.CongressId && x.Email == item.EnterpriseNode.Email);
                var userBo = new UserBO();
                item.HasChild = userBo.Any(this.ConnectionHandler, x => x.ParentId == item.Id);
                item.HasSuccedPayment = PaymentComponenets.Instance.TransactionFacade.Any(x => x.PayerId == item.Id && (x.Done || x.PayTypeId == 1));
                item.UserChairStatus = item.ChairId.HasValue ? Enums.UserChairStatus.HasChair : Enums.UserChairStatus.NoHasChair;
            }
            return item;
        }

    


        public DataTable ReportFormDataForExcel(string url, List<object> obj, string objectName, string[] refcolumnNames)
        {
            try
            {
                var result = new UserBO().ReportFormDataForExcel(this.ConnectionHandler, url, obj, objectName, refcolumnNames);
                return result;
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


        public List<KeyValuePair<string, string>> GetUserInforms(Guid congressId)
        {
            try
            {
                var articleTypeFacade = new ArticleTypeBO();
                var list = new List<KeyValuePair<string, string>>();
                list = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.SentUserTypes>();
                var pivots = new PivotBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                var articleType = articleTypeFacade.Where(this.ConnectionHandler, x => x.CongressId == congressId);
                foreach (var pivot in pivots)
                {
                    list.Add(new KeyValuePair<string, string>($"{typeof(Pivot).Name}/{pivot.Id}",
                        string.Format(Resources.Congress.SentInformByPivot, pivot.Title,Extention.GetAtricleTitle(congressId))));
                }

                foreach (var type in articleType)
                {
                    list.Add(new KeyValuePair<string, string>($"{typeof(ArticleType).Name}/{type.Id}",
                        string.Format(Resources.Congress.SentInformByType, type.Title, Extention.GetAtricleTitle(congressId))));
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


        #region BooleanResult
        public bool Insert(User user,
            FormGenerator.DataStructure.FormStructure formModel, HttpPostedFileBase fileBase)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new UserBO().Insert(this.ConnectionHandler, this.EnterpriseNodeConnection,
                        this.FormGeneratorConnection, user, formModel, fileBase))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(User user, 
            FormGenerator.DataStructure.FormStructure formModel)
        {

            var userBo = new UserBO();

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               if (
                    !userBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, this.FormGeneratorConnection,
                        user, formModel))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }
        public bool Update(User user, 
            FormGenerator.DataStructure.FormStructure formModel, HttpPostedFileBase fileBase)
        {

            var userBo = new UserBO();

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               if (
                    !userBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, this.FormGeneratorConnection,
                        user, formModel, fileBase))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }
        public bool Update(User user,
            FormGenerator.DataStructure.FormStructure formModel, HttpPostedFileBase fileBase,
           List<User> childs)
        {

            var userBo = new UserBO();
            ModelView.ModifyResult<User> inform;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               if (
                    !userBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, this.FormGeneratorConnection,
                        user, formModel, fileBase))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                inform = new UserBO().UpdateList(this.ConnectionHandler, this.PaymentConnection,this.ReservationConnection, childs);
                  
                this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (inform.SendInform)
                {
                    new UserBO().InformUserRegister(ConnectionHandler, user.CongressId, inform.InformList);
                }
            }
            catch
            {

            }

            return inform.Result;

        }
        public bool CompleteRegister(User user, 
            FormStructure postFormData, HttpPostedFileBase file)
        {
            var userBo = new UserBO();
            var formEntitiys = new ModelView.InFormEntitiyList<User>();
            bool result;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                user.Status = (byte)Enums.UserStatus.Register;
                if (
                     !userBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, this.FormGeneratorConnection,
                         user, postFormData, file))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                formEntitiys.Add( user,Resources.Congress.UserInsertEmail, Resources.Congress.UserInsertSMS);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                result = true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                    userBo.InformUserRegister(this.ConnectionHandler,user.CongressId, formEntitiys);
            }
            catch (Exception)
            {


            }
            return result;


        }


     

        public User Login(string mail, string password, Guid congressId)
        {
            try
            {
                var hashPassword = StringUtils.HashPassword(password);
                return new UserBO().FirstOrDefault(this.ConnectionHandler, x => x.Username.ToLower() == mail && x.Password == hashPassword && x.CongressId == congressId);


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
        public async Task<User> LoginAsync(string mail, string password, Guid congressId)
        {
            try
            {
                var hashPassword = StringUtils.HashPassword(password);
                return await new UserBO().FirstOrDefaultAsync(this.ConnectionHandler, x => x.Username.ToLower() == mail && x.Password == hashPassword && x.CongressId == congressId);


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
        public bool MergUsers(Guid sourceUserId, List<Guid> list)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new UserBO().MergUsers(this.ConnectionHandler, this.EnterpriseNodeConnection,
                        this.FormGeneratorConnection, this.PaymentConnection, this.ReservationConnection, sourceUserId, list))
                    throw new Exception(Resources.Congress.ErrorInSaveUser);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                this.PaymentConnection.CommitTransaction();

                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        

        public bool ChangePassword(Guid userId, string password)
        {
            try
            {
                var userBO = new UserBO();
                var user = userBO.Get(this.ConnectionHandler, userId);
                user.Password = StringUtils.HashPassword(password);
                return userBO.Update(this.ConnectionHandler, user);
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

        public bool CheckOldPassword(Guid userId, string password)
        {
            try
            {
                var referee = new UserBO().Get(this.ConnectionHandler, userId);
                return referee.Password.Equals(StringUtils.HashPassword(password));

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
        public User GetByEmail(string userName, Guid congressId)
        {
            try
            {
                var byUserName = new UserBO().FirstOrDefault(this.ConnectionHandler, x => x.EnterpriseNode.Email == userName && x.CongressId == congressId);
                return byUserName;
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





        public bool SendConfirmLink(string mail, string rerquestUrl, Guid congressId, string name)
        {
            try
            {

                var user = new UserBO().FirstOrDefault(this.ConnectionHandler, x => x.EnterpriseNode.Email == mail && x.CongressId == congressId);
                if (user == null)
                    throw new Exception(Resources.Congress.UserNameNotRegisterInCongress);
                var congress = new HomaBO().Get(this.ConnectionHandler, congressId);
                var configuration = congress.Configuration;
                var title = congress.CongressTitle;
                string body =
                    string.Format(Resources.Congress.UserSubscribeMailContent
                        , name, rerquestUrl + "?Id=" + user.Id + "&hid=" + congressId,
                        congress.CongressTitle);
                return MessageComponenet.Instance.MailFacade.SendMail(configuration.MailHost, configuration.MailPassword,
                    configuration.MailUserName, configuration.MailFrom, configuration.MailPort, congress.CongressTitle,
                    configuration.EnableSSL, mail, Resources.Congress.Register + title, body);
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

        public bool ForgotPassword(string email, string requestUrl, Guid congressId)
        {
            try
            {
                var user = new UserBO().FirstOrDefault(this.ConnectionHandler, x => x.EnterpriseNode.Email == email && x.CongressId == congressId);
                if (user == null)
                    throw new Exception(Resources.Congress.UserNameNotRegisterInCongress);

                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
                var configuration = homa.Configuration;
                var body =
                    string.Format(Resources.Congress.UserForgatPasswordMailContent
                        , user.EnterpriseNode.DescriptionField, requestUrl + "?Id=" + user.Id + "&hid=" + congressId,
                        user.Homa.CongressTitle);
                if (
                    !MessageComponenet.Instance.MailFacade.SendMail(configuration.MailHost, configuration.MailPassword,
                        configuration.MailUserName, configuration.MailFrom, configuration.MailPort, homa.CongressTitle,
                        configuration.EnableSSL, user.EnterpriseNode.Email, Resources.Congress.PasswordRecovery, body))
                    return false;
                return true;
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

        public bool RegisterWithOutSendMail(Guid congressId, string email, string name, string lastName)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var any = new UserBO().Any(this.ConnectionHandler, x => x.EnterpriseNode.Email == email && x.CongressId == congressId);
                if (!any)
                {
                    var user = new User { Username = email.ToLower(), CongressId = congressId };
                    var enterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        EnterpriseNodeTypeId = (int)EnterpriseNode.Tools.Enums.EnterpriseNodeType.RealEnterPriseNode,
                        RealEnterpriseNode = new RealEnterpriseNode { FirstName = name, LastName = lastName },
                        Email = email,
                    };
                    user.EnterpriseNode = enterpriseNode;
                    user.Status = (byte)Enums.UserStatus.PreRegister;
                    if (
                        !new UserBO().Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, user,
                            null))
                        throw new Exception(Resources.Congress.ErrorInSaveUser);
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


        public bool InformUser(Guid congressId, Message.Tools.ModelView.MessageModel messageModel, List<Guid> listuserId)
        {
            try
            {

                var informTypes = new List<Message.Tools.ModelView.MessageModel>();
                if (listuserId.Count == 0) return false;
               
                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
               var users=new UserBO().Where(ConnectionHandler,x=>x.Id.In(listuserId));
                foreach (var guid in users)
                {
                   

                    var informType = new Message.Tools.ModelView.MessageModel
                    {
                        Email = guid.EnterpriseNode.Email,
                        Mobile = guid.EnterpriseNode.Cellphone,
                        Id = guid.ToString(),
                    };
                    informTypes.Add(informType);
                }

                var configuration = homa.Configuration;
                if (messageModel.SendEmail)
                {
                    var strings =
                        informTypes.Where(model => !string.IsNullOrEmpty(model.Email))
                            .Select(type => type.Email)
                            .ToArray();
                    if (
                        !MessageComponenet.Instance.MailFacade.SendGroupMailWithInterval(configuration.MailHost,
                            configuration.MailPassword, configuration.MailUserName, configuration.MailFrom,
                            configuration.MailPort, homa.CongressTitle, configuration.EnableSSL,
                            strings, messageModel.EmailTitle,
                            messageModel.EmailBody, intervalSecond: configuration.GroupEmailInterval))
                        return false;
                }
                if (messageModel.SendSMS)
                {
                    var strings =
                        informTypes.Where(model => !string.IsNullOrEmpty(model.Mobile) && (model.Mobile.StartsWith("0098") || model.Mobile.StartsWith("+98") || model.Mobile.StartsWith("09")))
                            .Select(type => type.Mobile)
                            .ToArray();
                    if (!MessageComponenet.Instance.SMSFacade.SendGroupSms(configuration.SMSAccountId,
                        configuration.SMSAccountUserName, configuration.SMSAccountPassword,
                        strings, messageModel.SMSBody))
                        return false;
                }
                if (messageModel.SendIntrenalMessage)
                {

                    var strings =
                        informTypes
                            .Select(type => type.Id)
                            .ToArray();


                    if (!MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa.OwnerId, configuration.CongressId, strings, messageModel.EmailTitle, messageModel.EmailBody))
                        return false;



                    foreach (var item in strings)
                    {
                        //var tokenId = Get(item).FbTokenId;
                        var token = Select(c => c.FbTokenId, c => c.Id.ToString() == item).FirstOrDefault();
                        var plainTextBody = Utils.ConvertHtmlToString(messageModel.EmailBody);
                        var Plain2 = Utils.ConvertStringToHTML(messageModel.EmailBody);
                        if (!string.IsNullOrWhiteSpace(token))
                            FCMRepositiry.Work(messageModel.EmailTitle, Utils.ConvertHtmlToString(messageModel.EmailBody), token);

                    }

                }
                return true;
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

        public bool Paymnet(User user, List<DiscountType> discountAttaches, string callbackurl, Dictionary<int, decimal> paymentamounts)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new UserBO().Paymnet(this.ConnectionHandler, this.PaymentConnection, user, discountAttaches, callbackurl, paymentamounts))
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool SelectChair(User user)
        {
            var userBO = new UserBO();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!userBO.SelectChair(this.ConnectionHandler, this.ReservationConnection, user))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool UpdateList(Guid congressId,List<User> users)
        {
            var userBO = new UserBO();
            ModelView.ModifyResult<User> inform;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                inform = userBO.UpdateList(this.ConnectionHandler, this.PaymentConnection, this.ReservationConnection,users);
                 this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (inform.SendInform)
                {
                    new UserBO().InformUserRegister(ConnectionHandler, congressId, inform.InformList);
                }
            }
            catch
            {

            }

            return inform.Result;

        }

        #endregion

        public IEnumerable<ModelView.UserCardModel> GetUserCards(Guid userId, bool isadmin)
        {

            try
            {
                var list = new List<ModelView.UserCardModel>();
                var userBo = new UserBO();
                var user = userBo.Get(this.ConnectionHandler, userId);


                var homa = new HomaBO().Get(this.ConnectionHandler, user.CongressId);
                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, user.CongressId, homa.Configuration.CardLanguageId);
                var listchild = userBo.Where(this.ConnectionHandler, x => x.ParentId == userId);
                listchild.Add(user);
                var articles = new ArticleBO().Where(ConnectionHandler, x => x.CongressId == homa.Id && x.UserId == userId, true);
                var articleTypes = new ArticleTypeBO().Where(this.ConnectionHandler, x => x.CongressId == user.CongressId);
                foreach (var user1 in listchild)
                {
                    list.AddRange(userBo.GetUserCard(this.ConnectionHandler, user1, configcontent, homa, articleTypes, articles, isadmin));
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

        public IEnumerable<ModelView.UserCardModel> GetAllUserCards(Guid congressId)
        {
            try
            {
                var list = new List<ModelView.UserCardModel>();
                var userBo = new UserBO();
                var userlist = userBo.Where(this.ConnectionHandler, x => x.CongressId == congressId);
                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, congressId, homa.Configuration.CardLanguageId);
                var articleTypes = new ArticleTypeBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                var articles = new ArticleBO().Where(ConnectionHandler, x => x.CongressId == homa.Id, true);
                foreach (var user in userlist)
                {

                    list.AddRange(userBo.GetUserCard(this.ConnectionHandler, user, configcontent, homa, articleTypes, articles, true));
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


        public IEnumerable<ModelView.UserCardModel> SearchCards(Guid congressId, string txtSearch, User user,
            Guid? articleTypeId, EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure)
        {
            try
            {

                var userBo = new UserBO();
                return userBo.SearchCards(this.ConnectionHandler, congressId, txtSearch, user, articleTypeId, gender, formStructure);
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


        public Enums.SubscribeStatus Register(Guid congressId, string mail, string name, string lastName,
            string rerquestUrl, string culture)
        {
            var userBo = new UserBO();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congress = new HomaBO().Get(this.ConnectionHandler, congressId);
                var user = userBo.FirstOrDefault(this.ConnectionHandler, x => x.EnterpriseNode.Email == mail && x.CongressId == congressId);
                var configuration = congress.Configuration;
                Enums.SubscribeStatus subscribeStatus;
                if (user == null)
                {
                    user = new User { Username = mail.ToLower(), CongressId = congressId };
                    var enterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        EnterpriseNodeTypeId = (int)EnterpriseNode.Tools.Enums.EnterpriseNodeType.RealEnterPriseNode,
                        RealEnterpriseNode = new RealEnterpriseNode { FirstName = name, LastName = lastName },
                        Email = mail,
                    };
                    user.EnterpriseNode = enterpriseNode;
                    user.Status = (byte)Enums.UserStatus.PreRegister;
                    user.Culture = culture;
                    if (
                        !userBo.Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, user,
                            null))
                        throw new Exception(Resources.Congress.ErrorInSaveUser);
                    var title = congress.CongressTitle;
                    string body =
                        string.Format(Resources.Congress.UserSubscribeMailContent
                            , name + " " + lastName, rerquestUrl + "?Id=" + user.Id + "&hid=" + congressId,
                            congress.CongressTitle);
                    subscribeStatus =
                        !MessageComponenet.Instance.MailFacade.SendMail(configuration.MailHost,
                            configuration.MailPassword, configuration.MailUserName, configuration.MailFrom,
                            configuration.MailPort, congress.CongressTitle, configuration.EnableSSL, mail,
                            Resources.Congress.Register + "  " + title, body)
                            ? Enums.SubscribeStatus.NotConfirmed
                            : Enums.SubscribeStatus.MailSent;

                }
                else if (user.Status != (byte)Enums.UserStatus.PreRegister)
                    subscribeStatus = Enums.SubscribeStatus.Registered;
                else
                    subscribeStatus = Enums.SubscribeStatus.NotConfirmed;
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return subscribeStatus;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<User> GetSimilarUser(Guid congresId)
        {
            try
            {

                return new UserBO().GetSimilarUser(this.ConnectionHandler, congresId);

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


        public Dictionary<User, List<string>> ImportFromXml(HttpPostedFileBase file)
        {
            try
            {

                return new UserBO().ImportFromXml(this.ConnectionHandler, file);
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

        public Dictionary<User, List<string>> ImportFromExcel(HttpPostedFileBase fileBase, Guid congressId,
            Guid? parentId)
        {
            try
            {

                return new UserBO().ImportFromExcel(this.ConnectionHandler, fileBase, congressId, parentId);
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

        public IEnumerable<User> GetChildUsersForWorkShops(User parent, Guid workshopId)
        {
            try
            {
                var list = new UserBO().Where(this.ConnectionHandler, x => x.ParentId == parent.Id);
                var outlist = new List<User>();
                list.Add(parent);
                var workShopUserBo = new WorkShopUserBO();
                var @select = workShopUserBo.Select(ConnectionHandler,
                    new Expression<Func<WorkShopUser, object>>[] { x => x.UserId, x => x.Status },
                    x => x.WorkShopId == workshopId);
                foreach (var user in list)
                {
                    var firstOrDefault = @select.FirstOrDefault(x => x.UserId == user.Id);


                    if (firstOrDefault != null &&
                        (firstOrDefault.Status != (byte)Enums.WorkShopRezervState.RegisterRequest &&
                         firstOrDefault.Status != (byte)Enums.WorkShopRezervState.DenialPay))
                        continue;
                    user.ReservedItem = firstOrDefault != null;
                    outlist.Add(user);
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

        public IEnumerable<User> GetChildUsersForHotel(User parent, Guid hotelId)
        {
            try
            {
                var outlist = new List<User>();
                var list = new UserBO().Where(this.ConnectionHandler, x => x.ParentId == parent.Id);
                list.Add(parent);
                var hotelUserBo = new HotelUserBO();
                var @select = hotelUserBo.Select(ConnectionHandler,
                   new Expression<Func<HotelUser, object>>[] { x => x.UserId, x => x.Status },
                   x => x.HotelId == hotelId);
                foreach (var user in list)
                {
                    var firstOrDefault = @select.FirstOrDefault(x => x.UserId == user.Id);


                    if (firstOrDefault != null &&
                        (firstOrDefault.Status != (byte)Enums.RezervState.RegisterRequest &&
                         firstOrDefault.Status != (byte)Enums.RezervState.DenialPay))
                        continue;
                    user.ReservedItem = firstOrDefault != null;
                    outlist.Add(user);
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

        public IEnumerable<User> GenerateGuest(Guid congressId, int count)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var list = new List<User>();
                var max = 1;
                for (var i = 1; i <= count; i++)
                {
                    var user = new User();
                    var enterpriseNode =
                        new EnterpriseNode.DataStructure.EnterpriseNode()
                        {
                            RealEnterpriseNode = new RealEnterpriseNode(),
                            EnterpriseNodeTypeId =
                                (int)EnterpriseNode.Tools.Enums.EnterpriseNodeType.RealEnterPriseNode
                        };
                    user.Username = "UserName-" + DateTime.Now.ShamsiDate() + "-" + DateTime.Now.GetTime() + "-" + max;
                    user.CongressId = congressId;
                    enterpriseNode.RealEnterpriseNode.FirstName = " ";
                    enterpriseNode.RealEnterpriseNode.LastName = " ";
                    user.EnterpriseNode = enterpriseNode;
                    if (!new UserBO().InsertGuestUser(this.ConnectionHandler, this.EnterpriseNodeConnection, user, null))
                        throw new Exception(Resources.Congress.ErrorInSaveUser);
                    user.EnterpriseNode = enterpriseNode;
                    list.Add(user);
                    max++;
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return list;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


        public bool InsertList(List<User> users)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var @select=new UserBO().Select(ConnectionHandler,x=>x.Id);
                var userBo = new UserBO();
                foreach (var user in users)
                {
                    if (@select.All(x => x != user.Id))
                    {
                        if (!userBo.Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, user,
                            null))
                            throw new Exception(Resources.Congress.ErrorInSaveUser);
                    }
                    else
                    {
                        if (!userBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, user,
                            null))
                            throw new Exception(Resources.Congress.ErrorInSaveUser);
                    }
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return true;

            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }


        public bool UpdateUserAttendance(List<User> users)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
               
                var @select = new UserBO().Select(ConnectionHandler, x => x.Id);
                var userBo = new UserBO();
                foreach (var user in users)
                {
                    if (@select.All(x => x != user.Id)) continue;
                    if (!userBo.Update(this.ConnectionHandler,  user))
                        throw new Exception(Resources.Congress.ErrorInSaveUser);

                }
                this.ConnectionHandler.CommitTransaction();
                return true;

            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }


        public User UserAttendance(Guid congressId, long number)
        {
            ModelView.ModifyResult<User> modifyResult;
            try
            {
                modifyResult= new UserBO().UserAttendance(this.ConnectionHandler, congressId, number);
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
            try
            {
                if (modifyResult.SendInform)
                {
                    new UserBO().InformUserRegister(ConnectionHandler,congressId,  modifyResult.InformList);
                }
            }
            catch (Exception)
            {

            }

            return  modifyResult.RefObject;
        }



        public IEnumerable<User> Search(Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender,
            FormStructure formStructure)
        {
            try
            {
                var enumerable = new UserBO().Search(this.ConnectionHandler, congressId, txtSearch, user, ascendingDescending, sortuser, gender, formStructure);
                var list = new List<Transaction>();
                if (enumerable.Any())
                    list = PaymentComponenets.Instance.TransactionFacade.OrderByDescending(x => x.PayDate, x => x.PayerId.In(enumerable.Select(user1 => (Guid?)user1.Id)));
                var userlist = new UserBO().Select(ConnectionHandler, x => x.ParentId, x => x.CongressId == congressId);
                var @select = new NewsLetterBO().Select(ConnectionHandler, x => x.Email, x => x.CongressId == congressId);
                var outlist = new List<User>();
                foreach (var item in enumerable)
                {
                    var listTransaction = list.Where(x => x.PayerId == item.Id).ToList();
                    item.HasSuccedPayment = listTransaction.Any(x => x.Done || x.PayTypeId == 1);
                    if (item.HasSuccedPayment && item.TransactionId != null)
                    {
                        var tran = listTransaction.FirstOrDefault(x => x.Id == item.TransactionId);
                        item.Done = tran!=null&&(tran.Done || tran.PayTypeId == 1);
                    }
                    item.HasChild = userlist.Any(x => x == item.Id);
                    item.UserChairStatus = item.ChairId.HasValue ? Enums.UserChairStatus.HasChair : Enums.UserChairStatus.NoHasChair;
                    item.RegisterInNewsLetter = item.EnterpriseNode!=null&& !string.IsNullOrEmpty(item.EnterpriseNode.Email) && @select.Any(x => x == item.EnterpriseNode.Email);
                    item.HasOtherPay = listTransaction.Any(x => x.Id != item.TransactionId && (x.Done || (x.PayTypeId == 1)));
                    switch (user.HasOtherPayState)
                    {
                        case Enums.HasValue.None:
                            outlist.Add(item);
                            break;
                        case Enums.HasValue.Has:
                            if (item.HasOtherPay)
                                outlist.Add(item);
                            break;
                        case Enums.HasValue.NotHas:
                            if (item.HasOtherPay == false)
                                outlist.Add(item);
                            break;

                    }
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
        public List<dynamic> SearchDynamic(Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender,
                FormStructure formStructure)
        {
            try
            {
                var enumerable = new UserBO().SearchDynamic(this.ConnectionHandler, congressId, txtSearch, user, ascendingDescending, sortuser, gender, formStructure);
                return FillOnDynamic(congressId, enumerable, user);

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
        public List<dynamic> GetChildsDynamic(Guid congressId, Guid parentuserId)
        {
            try
            {
                var enumerable = new UserBO().GetChildsDynamic(this.ConnectionHandler, parentuserId);
                return FillOnDynamic(congressId, enumerable);


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

        private List<dynamic> FillOnDynamic(Guid congressId, List<dynamic> enumerable, User user = null)
        {
            var list = new List<dynamic>();
            if (enumerable.Any())
            {
                var idlist = enumerable.Select(user1 => (Guid?)user1.Id).ToList();
                list = PaymentComponenets.Instance.TransactionFacade.Select(new Expression<Func<Transaction, object>>[] { x => x.Id, x => x.PayDate, x => x.Done, x => x.PayTypeId, x => x.PayerId }, x => x.PayerId.In(idlist), new OrderByModel<Transaction>() { Expression = x => x.PayDate, OrderType = OrderType.DESC });
            }
            var userlist = new UserBO().Select(ConnectionHandler, x => x.ParentId, x => x.CongressId == congressId);
            var @select = new NewsLetterBO().Select(ConnectionHandler, x => x.Email, x => x.CongressId == congressId);
            var outlist = new List<dynamic>();
            foreach (var item in enumerable)
            {
                item.HasChildUser = userlist.Any(x => x == item.Id);
                item.UserChairStatus = item.ChairId != null ? Enums.UserChairStatus.HasChair : Enums.UserChairStatus.NoHasChair;
                item.RegisterInNewsLetter = !string.IsNullOrEmpty(item.Email) && @select.Any(x => x == item.Email);
                var listTransaction = list.Where(x => x.PayerId == item.Id);
                item.HasSuccedPayment = listTransaction.Any(x => x.Done || (x.PayTypeId is byte && x.PayTypeId == 1));
                item.HasOtherPay = listTransaction.Any(x =>
                    x.Id != item.TransactionId && (x.Done == true || (x.PayTypeId is byte && x.PayTypeId == 1)));
                item.Done = item.HasSuccedPayment && item.TransactionId != null && listTransaction.Any(x =>
                                x.Id == item.TransactionId && (x.Done == true || (x.PayTypeId is byte && x.PayTypeId == 1)));
                if (user == null)
                    outlist.Add(item);
                else
                {
                    switch (user.HasOtherPayState)
                    {
                        case Enums.HasValue.None:
                            outlist.Add(item);
                            break;
                        case Enums.HasValue.Has:
                            if (item.HasOtherPay == true)
                                outlist.Add(item);
                            break;
                        case Enums.HasValue.NotHas:
                            if (item.HasOtherPay == false)
                                outlist.Add(item);
                            break;
                    }
                }

            }

            return outlist;
        }




        public IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchText(Guid congressId, string txtSearch, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser)
        {
            try
            {

                return new UserBO().SearchText(this.ConnectionHandler, congressId, txtSearch, ascendingDescending, sortuser);


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





        public KeyValuePair<bool, Guid> GroupPayment(Guid congressId, User parentUser, Guid paymentype, List<User> users,
            List<DiscountType> discountTypes, string callbackurl, Dictionary<int, decimal> paymentamounts)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var groupPayment = new UserBO().GroupPayment(this.ConnectionHandler, this.PaymentConnection, congressId,
                    parentUser,
                    paymentype, users, discountTypes, callbackurl, paymentamounts);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                return groupPayment;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public Guid UpdateStatusAfterTransactionGroupPay(Guid congressId,User parentuser, Guid tempId)
        {
            var userBo = new UserBO();
            ModelView.ModifyResult<User> informUser;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                informUser = userBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                     this.ReservationConnection, parentuser.Id, tempId);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
               
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

            try
            {
                if (informUser.SendInform)
                {
                   new UserBO().InformUserRegister(ConnectionHandler, congressId,  informUser.InformList);
                }
            }
            catch (Exception)
            {


            }

            return informUser != null ? informUser.TransactionId : Guid.Empty;
        }

        public Guid UpdateStatusAfterTransaction(Guid congressId,Guid id)
        {
            var userBO = new UserBO();
            ModelView.ModifyResult<User> informUser;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var user = userBO.Get(this.ConnectionHandler, id);
                if (user == null || user.TempId == null) return Guid.Empty;
                informUser = userBO.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                     this.ReservationConnection, id, (Guid)user.TempId);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
               

            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (informUser.SendInform)
                {
                    new UserBO().InformUserRegister(ConnectionHandler, congressId, informUser.InformList);
                }
            }
            catch (Exception)
            {


            }

            return informUser != null ? informUser.TransactionId : Guid.Empty;


        }



        public IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchUserWithInformType(Guid congressId, string informType)
        {
            try
            {

                return new UserBO().SearchUserWithInformType(this.ConnectionHandler, informType, congressId);


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

        public override bool Delete(params object[] keys)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new UserBO().Delete(this.ConnectionHandler, this.PaymentConnection, this.ReservationConnection, keys))
                    throw new Exception("خطایی در حذف کاربر وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<ModelView.ReportChartModel> ChartUserStatusCount(Guid congressId)
        {
            try
            {
                return new UserBO().ChartUserStatusCount(this.ConnectionHandler, congressId);
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

        public IEnumerable<ModelView.ReportChartModel> ChartUserMothCount(Guid congressId, string year)
        {
            try
            {
                return new UserBO().ChartUserMothCount(this.ConnectionHandler, congressId, year);

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

        public IEnumerable<ModelView.ReportChartModel> ChartUserDayCount(Guid congressId, string moth, string year)
        {
            try
            {
                return new UserBO().ChartUserDayCount(this.ConnectionHandler, congressId, moth, year);
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
