using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface IMabnaFacade
    {
        Transaction MabnaCallBackPayRequest(Guid Id, string resCode, string trn, string crn, int amount);

    }
}
