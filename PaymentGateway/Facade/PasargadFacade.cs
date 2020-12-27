using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Radyn.Framework;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.PaymentGateway.BO;
using Radyn.PaymentGateway.Facade.Interface;
using Radyn.Utility;
using Convert = System.Convert;

namespace Radyn.PaymentGateway.Facade
{
    public class PasargadFacade : IPasargadFacade
    {






        public bool PasargadCallBackPayRequest(Transaction transaction)
        {
            try
            {

                return new PasargadBO().PasargadCallBackPayRequest(transaction);
            }
            catch (KnownException knownException)
            {
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        
    }
}
