using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface IWorkShopUserFacade : IBaseFacade<WorkShopUser>
    {
        bool UpdateList(Guid congressId,List<WorkShopUser> list);
        KeyValuePair<bool, Guid> WorkShopUserInsert(Guid congressId,Guid workShopId, User congressUser, List<DiscountType> transactionDiscountAttaches, string callbackurl, FormStructure postFormData, List<Guid> users);
        IEnumerable<WorkShopUser> Search(Guid id, byte? status, string registerDate, string serachvalue,FormStructure formStructure);
        Guid UpdateStatusAfterTransaction(Guid congressId,Guid userId, Guid tempId);

        bool WorkShopUserInsert(Guid congressId, WorkShopUser WorkShopUser, FormGenerator.DataStructure.FormStructure formModel);
        bool WorkShopUserUpdate(Guid congressId, WorkShopUser WorkShopUser, FormGenerator.DataStructure.FormStructure formModel);




    }
}
