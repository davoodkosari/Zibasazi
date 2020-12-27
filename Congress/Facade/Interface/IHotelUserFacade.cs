using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface IHotelUserFacade : IBaseFacade<HotelUser>
    {
        bool UpdateList(Guid congressId, List<HotelUser> list);

        Guid UpdateStatusAfterTransaction(Guid congressId, Guid userId, Guid tempId);
        IEnumerable<HotelUser> Search(Guid hotelId, byte? status, string registerDate, string searchvalue,FormStructure formStructure);
        KeyValuePair<bool, Guid> HotelUserInsert(Guid congressId, Guid hotelId, User parentUser, List<DiscountType> discountAttaches, string callBackurl, FormGenerator.DataStructure.FormStructure formModel, int dayCount, List<Guid> userId);
        bool HotelUserInsert(Guid congressId, HotelUser hotelUser, FormGenerator.DataStructure.FormStructure formModel);
        bool HotelUserUpdate(Guid congressId, HotelUser hotelUser, FormGenerator.DataStructure.FormStructure formModel);
       
       
       
      



    }
}
