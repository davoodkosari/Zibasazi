using System;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressAccountFacade : IBaseFacade<CongressAccount>
{
   
    bool Insert(Guid congressId, Radyn.Payment.DataStructure.Account account);
}
}
