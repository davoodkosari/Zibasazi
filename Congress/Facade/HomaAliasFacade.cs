using System;
using System.Collections.Generic;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class HomaAliasFacade : CongressBaseFacade<HomaAlias>, IHomaAliasFacade
    {
        internal HomaAliasFacade()
        {
        }

        internal HomaAliasFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      


        public List<HomaAlias> GetByCongressId(Guid congressId)
        {
            try
            {
                return new HomaAliasBO().GetByCongressId(this.ConnectionHandler, congressId);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
