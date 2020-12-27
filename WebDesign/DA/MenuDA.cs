using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class MenuDA : DALBase<Menu>
    {
        public MenuDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class MenuCommandBuilder
    {
    }
}
