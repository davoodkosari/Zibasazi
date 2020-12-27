using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class ContainerDA : DALBase<Container>
    {
        public ContainerDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class ContainerCommandBuilder
    {
    }
}
