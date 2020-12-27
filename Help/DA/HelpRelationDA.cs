using Radyn.Contracts.DataStructure.Congress;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Help.DA
{
    public sealed class HelpRelationDA : DALBase<HelpRelation>
    {
        public HelpRelationDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class HelpRelationCommandBuilder
    {
    }
}
