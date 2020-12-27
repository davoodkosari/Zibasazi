using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class FormsDA : DALBase<Forms>
    {
        public FormsDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class FormsCommandBuilder
    {
    }
}
