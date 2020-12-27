using System;
using System.Configuration;
using System.Linq;
using Radyn.Payment.DataStructure;
using Radyn.Payment.Facade;
using Radyn.Utility;

namespace Radyn.Payment.Tools
{
    public static class Extentions
    {
        public static ModelView.TerminalInfo TerminalInfo
        {
            get
            {
                var jobScheduler = (ModelView.TerminalInfo)ConfigurationManager.GetSection("radyn/GetWayInfo");
                if (jobScheduler == null)
                    throw new Exception("لطفا تنظیمات ترمینال را در قسمت GetwayInfo در config  وارد نمایید");
                return jobScheduler;
            }
        }
       
        public static string FillTempAdditionalData(string payType, byte? bankId, string paymentUrl, int terminal , string username , string password, string certificatePath, string certificatePassword, string merchantId, string merchantPublicKey, string merchantPrivateKey)
        {
            var textToBeDecrypted = payType + "," + (bankId!=null?bankId.ToString(): String.Empty) + "," + paymentUrl + "," + terminal + "," + username + "," + password + "," + (certificatePath ?? String.Empty) + "," + (certificatePassword ?? String.Empty) + "," + (merchantId ?? String.Empty) + "," + (merchantPublicKey ?? String.Empty) + "," + (merchantPrivateKey ?? String.Empty);
            return StringUtils.Encrypt(textToBeDecrypted);          
        }
        public static string PrepaymenyUrl(Guid tempId)
        {
            return Constants.DefualCallerUrlGetway + tempId;
        }
       
        public static Temp DecryptVariables(Guid tempId)
        {
           var  temp = PaymentComponenets.Instance.TempFacade.Get(tempId);
            if (temp == null) return new Temp();
            if (string.IsNullOrEmpty(temp.AdditionalData)) return temp;
            var decrypt = StringUtils.Decrypt(temp.AdditionalData);
            var strings = decrypt.Split(',');
            temp.PayType = strings[0];
            temp.BankId =  string.IsNullOrEmpty(strings[1]) ? (byte?)null : strings[1].ToByte();
            temp.PaymentUrl = strings[2];
            temp.TerminalId = strings[3];
            temp.TerminalUserName =  strings[4];
            temp.TerminalPassword = strings[5];
            if (strings.Length >= 7)
            {
                temp.CertificatePath = strings[6];
            }
            if (strings.Length >= 8)
            {
                temp.CertificatePassword = strings[7];
            }
            if (strings.Length >= 9)
            {
                temp.MerchantId = strings[8];
            }
            if (strings.Length >= 10)
            {
                temp.MerchantPublicKey = strings[9];
            }
            if (strings.Length >= 11)
            {
                temp.MerchantPrivateKey = strings[10];
            }

            if (temp.Description != null)
                temp.Description = temp.Description.Replace("\r\n", "<br />");
            return temp;
        }
        
    }
}
