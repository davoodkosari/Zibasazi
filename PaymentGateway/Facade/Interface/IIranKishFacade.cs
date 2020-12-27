using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface IIranKishFacade
    {
        Transaction IranKishCallBackPayRequest(Guid Id, string privateKey, string status, string refNum);

    }
}
