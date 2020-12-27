using System;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.PaymentGateway.Tools
{
   public static class Extentions
    {
        public static string AfterCallBackUrl(this Enums.Bank bank, Guid Id,string requestAuthority)
        {
            return "http://" + requestAuthority + "/PaymentGateway/" + bank + "Payment/CallBack?Id=" + Id;
        }
        public static string CallBankUrl(this Enums.Bank bank,Guid Id, string requestAuthority)
        {
            return "http://" + requestAuthority + "/PaymentGateway/" + bank + "Payment/Invoice?Id="+Id;
        }
       
       
   }
}
