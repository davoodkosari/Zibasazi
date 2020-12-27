using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CustomMessageDA : DALBase<CustomMessage>
    {
        public CustomMessageDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        {
        }

        internal class CustomMessageDACommandBuilder
        {
        }
    }
}
