using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface IGhavaminFacade
    {
        Transaction GhavaminCallBackPayRequest(Guid Id,string resultCode, string SayanRef, string paymentID);
    }
}
