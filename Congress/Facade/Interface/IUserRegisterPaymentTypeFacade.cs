using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IUserRegisterPaymentTypeFacade : IBaseFacade<UserRegisterPaymentType>
    {
        bool Insert(UserRegisterPaymentType registerPaymentType, Dictionary<int, decimal> keyValuePairs);
        bool Update(UserRegisterPaymentType registerPaymentType, Dictionary<int, decimal> keyValuePairs);
        IEnumerable<UserRegisterPaymentType> GetValidListUser(Guid congressId);
        Dictionary<int, decimal> GetDaysInfo(Guid congressId, string culture, Guid? registerPaymentTypeId);
        Dictionary<int, decimal> GetRegiterTypeInfo(Guid id);
    }
}
