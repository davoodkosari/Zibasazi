using System;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class CongressDefinitionBO : BusinessBase<CongressDefinition>
    {
        public CongressDefinition GetValidDefinition(IConnectionHandler connectionHandler, Guid congressId)
        {

            var congressDefinition = new CongressDefinitionBO().Get(connectionHandler, congressId);
            if (congressDefinition != null) return congressDefinition;
            congressDefinition = new CongressDefinition { CongressId = congressId };
            if (!new CongressDefinitionBO().Insert(connectionHandler, congressDefinition))
                throw new Exception(Resources.Congress.ErrorInSaveConfiguartion);
            return congressDefinition;

        }
    }
}
