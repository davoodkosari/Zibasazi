using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using Constants = Radyn.Congress.Tools.Constants;

namespace Radyn.Congress.BO
{
    internal class CongressDiscountTypeBO : BusinessBase<CongressDiscountType>
    {
        public  decimal CalulateAmountNew(IConnectionHandler paymentConnection, decimal amount, List<DiscountType> discountAttaches)
        {

            var discountTypeFacade = PaymentComponenets.Instance.DiscountTypeTransactionalFacade(paymentConnection);
            decimal outamout = amount;
            if (discountAttaches.Any())
            {
                var @where = discountTypeFacade.Where(x => x.Id.In(discountAttaches.Select(i => i.Id)));
                foreach (var transactionDiscountAttach in discountAttaches)
                {
                    var discountType = @where.FirstOrDefault(x=>x.Id==transactionDiscountAttach.Id);
                    if(discountType==null)continue;
                    if (discountType.IsPercent)
                        outamout -= (amount * discountType.ValidValue.ToDecimal()) / 100;
                    else
                        outamout -= discountType.ValidValue.ToDecimal();
                }
            }
           
            return outamout;
        }
        public string FillTempAdditionalData(IConnectionHandler connectionHandler, Guid congressId)
        {
            var configuration = new ConfigurationBO().SelectFirstOrDefault(connectionHandler,
                new Expression<Func<Configuration, object>>[]
                {
                    x => x.BankId, x => x.PaymentType, x => x.TerminalId, x => x.TerminalUserName,
                    x => x.TerminalPassword, x => x.CertificatePath, x => x.CertificatePassword, x => x.MerchantId,
                    x => x.MerchantPublicKey, x => x.MerchantPrivateKey

                }, x => x.CongressId == congressId);
            if (configuration == null) throw new Exception(Resources.Congress.Congresshasnotdonesetting);
            if (string.IsNullOrEmpty(configuration.PaymentType)) throw new Exception(Resources.Congress.Nopaymenthasbeenselectedfortheconference);
            var merchantPublicKey = (configuration.MerchantPublicKey != null ? configuration.MerchantPublicKey : String.Empty);
            var merchantPrivateKey = (configuration.MerchantPrivateKey != null ? configuration.MerchantPrivateKey : String.Empty);
            var certificatePath = (configuration.CertificatePath != null ? configuration.CertificatePath : String.Empty);
            var certificatePassword = (configuration.CertificatePassword != null ? configuration.CertificatePassword : String.Empty);
            var terminalId = (configuration.TerminalId != null ? configuration.TerminalId : String.Empty);
            var bankId = (configuration.BankId != null ? (byte)configuration.BankId : (byte?)null);
            var terminalUserName = (configuration.TerminalUserName != null ? configuration.TerminalUserName : String.Empty);
            var terminalPassword = (configuration.TerminalPassword != null ? configuration.TerminalPassword : String.Empty);
            var merchantId = (configuration.MerchantId != null ? configuration.MerchantId : String.Empty);
            var paymentType = (configuration.PaymentType != null ? configuration.PaymentType : String.Empty);
           return Radyn.Payment.Tools.Extentions.FillTempAdditionalData(paymentType, bankId, Constants.PaytypeUrl, terminalId, terminalUserName, terminalPassword, certificatePath, certificatePassword, merchantId, merchantPublicKey, merchantPrivateKey);
        }
       
    }
}
