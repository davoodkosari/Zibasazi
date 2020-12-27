using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Congress.Facade
{
    internal sealed class PivotCategoryFacade : CongressBaseFacade<PivotCategory>, IPivotCategoryFacade
    {
        internal PivotCategoryFacade()
        {
        }

        internal PivotCategoryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
