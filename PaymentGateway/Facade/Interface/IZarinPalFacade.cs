using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.Payment.DataStructure;

namespace Radyn.PaymentGateway.Facade.Interface
{
    public interface IZarinPalFacade
    {
        Transaction ZarinPalCallBackPayRequest(Guid transId, string status, string authority);
    }
}
