using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radyn.CrossPlatform.Tools
{
    public class Enums
    {
        public enum QueryTypes : byte
        {
            Insert = 1,

            Update = 2,
            //[Description("PaymentArticlePayment", Type = typeof(Resources.Congress))]
            Delete = 3,
        }

        public enum ClientsTableNames
        {
            Messages,
            Contents,
            ContentCategories,
            SyncAdapter,
            News,
        }
    }
}
