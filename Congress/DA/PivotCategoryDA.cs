using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Congress.DA
{
    public sealed class PivotCategoryDA : DALBase<PivotCategory>
    {
        public PivotCategoryDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        {
        }

        internal class PivotCategoryCommandBuilder
        {
        }
    }
}
