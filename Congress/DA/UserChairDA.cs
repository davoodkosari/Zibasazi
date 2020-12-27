using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class UserChairDA : DALBase<UserChair>
    {
        public UserChairDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class UserChairCommandBuilder
    {
    }
}
