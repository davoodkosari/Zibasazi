using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IGroupRegisterDiscountFacade : IBaseFacade<GroupRegisterDiscount>
{
    IEnumerable<GroupRegisterDiscount> GetValidList(Guid congressId);
  
}
}
