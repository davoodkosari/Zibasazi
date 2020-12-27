using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface ISaderatFacade
    {
        Transaction SaderatCallBackPayRequest(Guid Id, string orederId, string resultCode, string referenceId);

    }
}
