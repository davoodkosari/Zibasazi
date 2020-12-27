using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface IMelliApiFacade
    {
        Transaction MelliCallBackPayRequest(Guid Id,string token);
    }
}
