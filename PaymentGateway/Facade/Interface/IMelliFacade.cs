using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
   public interface IMelliFacade
    {
       Transaction MelliCallBackPayRequest(Guid Id, string oredrId, string timeStamp, string fp);

    }
}
