using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface IUserFacade : IBaseFacade<User>
    {

        #region BooealnResult

        bool Insert(User user,  FormStructure formModel, HttpPostedFileBase fileBase);
        bool Update(User user,  FormStructure formModel, HttpPostedFileBase fileBase);
        bool Update(User user,  FormStructure formModel, HttpPostedFileBase fileBase, List<User> childs);
        bool CompleteRegister(User user,  FormStructure postFormData, HttpPostedFileBase file);
        bool RegisterWithOutSendMail(Guid congressId, string email, string name, string lastName);
        bool InsertList(List<User> users);
        bool UpdateUserAttendance(List<User> users);
        bool ChangePassword(Guid userId, string password);
        bool CheckOldPassword(Guid userId, string password);
      
        bool SendConfirmLink(string mail, string rerquestUrl, Guid congressId, string name);
        bool ForgotPassword(string mail, string rerquestUrl, Guid congressId);
        bool UpdateList(Guid congressId,List<User> users);
        bool InformUser(Guid congressId, Message.Tools.ModelView.MessageModel messageModel, List<Guid> listuserId);
        bool Paymnet(User user, List<DiscountType> discountAttaches, string callbackurl, Dictionary<int, decimal> paymentamounts);
        bool SelectChair(User user);


        #endregion


        #region UserResult
        User UserAttendance(Guid congressId, long number);
        User Login(string username, string password, Guid congressId);
        Task<User> LoginAsync(string username, string password, Guid congressId);
       
        User GetByEmail(string mail, Guid congressId);
       

        #endregion


        #region CollectionResult
        KeyValuePair<bool, Guid> GroupPayment(Guid congressId, User parentUser, Guid paymentype, List<User> users, List<DiscountType> discountTypes, string callbackurl, Dictionary<int, decimal> paymentamounts);
        IEnumerable<User> Search(Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender = EnterpriseNode.Tools.Enums.Gender.None, FormStructure formStructure = null);

        List<dynamic> GetChildsDynamic(Guid congressId, Guid parentuserId);
        List<dynamic> SearchDynamic(Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender = EnterpriseNode.Tools.Enums.Gender.None, FormStructure formStructure = null);
       IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchText(Guid congressId, string txtSearch, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser);
        IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchUserWithInformType(Guid congressId, string informType);
       
       
        IEnumerable<ModelView.UserCardModel> GetUserCards(Guid id, bool isadmin = false);
        IEnumerable<ModelView.UserCardModel> GetAllUserCards(Guid congressId);
        IEnumerable<ModelView.UserCardModel> SearchCards(Guid congressId, string txtSearch, User user, Guid? articleTypeId, EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure);

        List<KeyValuePair<string, string>> GetUserInforms(Guid congressId);
        Dictionary<User, List<string>> ImportFromExcel(HttpPostedFileBase fileBase, Guid congressId, Guid? parentId = null);
        IEnumerable<User> GetChildUsersForWorkShops(User parent, Guid workshopId);
        IEnumerable<User> GetChildUsersForHotel(User parent, Guid workshopId);
        IEnumerable<User> GenerateGuest(Guid congressId, int count);

        #endregion


        Guid UpdateStatusAfterTransaction(Guid congressId,Guid id);
        Guid UpdateStatusAfterTransactionGroupPay(Guid congressId,User parentuser, Guid tempId);

        DataTable ReportFormDataForExcel(string url, List<object> obj, string objectName, string[] refcolumnNames);

        Enums.SubscribeStatus Register(Guid congressId, string mail, string name, string lastName, string rerquestUrl,string culture);

        IEnumerable<User> GetSimilarUser(Guid userId);
        bool MergUsers(Guid sourceUserId, List<Guid> list);

        Dictionary<User, List<string>> ImportFromXml(HttpPostedFileBase file);
    }
}
