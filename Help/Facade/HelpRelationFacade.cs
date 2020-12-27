using Radyn.Contracts.DataStructure.Congress;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Help.BO;
using Radyn.Help.Facade.Interface;

namespace Radyn.Help.Facade
{
    internal sealed class HelpRelationFacade : HelpFacade<HelpRelation>, IHelpRelationFacade
    {
        internal HelpRelationFacade() { }

        internal HelpRelationFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

       
    }
}
