using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
  public sealed class UserFormsDA : DALBase<UserForms>
    {
        public UserFormsDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        {
        }
        internal class UserFormsCommandBuilder
        {
        }
    }
}
