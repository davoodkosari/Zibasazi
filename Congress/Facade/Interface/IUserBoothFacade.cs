using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface IUserBoothFacade : IBaseFacade<UserBooth>
    {

        bool UserBoothInsert(Guid congressId, UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl, FormStructure formModel, List<BoothOfficer> boothOfficers);
        bool UserBoothUpdate(Guid congressId, UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl, FormStructure formModel,List<BoothOfficer> boothOfficers );


        bool UserBoothInsert(Guid congressId, UserBooth userBooth,  FormStructure formModel, List<BoothOfficer> boothOfficers);
        bool UserBoothUpdate(Guid congressId, UserBooth userBooth,  FormStructure formModel, List<BoothOfficer> boothOfficers);
        IEnumerable<UserBooth> Search(Guid id, byte? status, string registerDate, string serachvalue, FormStructure formStructure);
        Guid UpdateStatusAfterTransaction(Guid congressId,Guid user, Guid boothId);
        bool UpdateList(Guid congressId,List<UserBooth> list);

        KeyValuePair<bool,Guid> InsertGuest(EnterpriseNode.DataStructure.EnterpriseNode enterpriseNode, List<Guid> boothIdlist,
            HttpPostedFileBase file, List<DiscountType> discountAttaches,
            string callBackurl, FormGenerator.DataStructure.FormStructure postFormData, Guid congressId);

        void UpdateStatusAfterTransactionGuest(Guid congressId,Guid id);

        
    }
}
