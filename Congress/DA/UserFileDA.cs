using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class UserFileDA : DALBase<UserFile>
    {
        public UserFileDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class UserFileCommandBuilder
    {
    }
}
