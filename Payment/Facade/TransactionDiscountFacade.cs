using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment.BO;
using Radyn.Payment.DataStructure;
using Radyn.Payment.Facade.Interface;
using Radyn.Payment.Tools;

namespace Radyn.Payment.Facade
{
    internal sealed class TransactionDiscountFacade : PaymentBaseFacade<TransactionDiscount>, ITransactionDiscountFacade
    {
        internal TransactionDiscountFacade() { }

        internal TransactionDiscountFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      
      


    }
}
