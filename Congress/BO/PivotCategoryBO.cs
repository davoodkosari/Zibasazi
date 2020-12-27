using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Congress.BO
{
    internal class PivotCategoryBO : BusinessBase<PivotCategory>
    {
        public override bool Insert(IConnectionHandler connectionHandler, PivotCategory obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }
    }
}
