using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressDefinitionDA : DALBase<CongressDefinition>
    {
        public CongressDefinitionDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressDefinitionCommandBuilder
    {
    }
}
