using System;
using System.Collections.Generic;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.DA;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.BO
{
    internal class ContentsBO : BusinessBase<Contents>
    {
        

        public IEnumerable<Contents> GetListByCategory(IConnectionHandler connectionHandler, Guid congressId, Guid categoryId)
        {
            var list = new ContentsDA(connectionHandler).GetContentByCategory(congressId, categoryId);

            return list;
        }

      
    }
}
