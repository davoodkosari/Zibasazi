using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class VIPDA : DALBase<VIP>
    {
        public VIPDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public short GetMaxOrder()
        {

            var vipCommandBuilder = new VIPCommandBuilder();
            var query = vipCommandBuilder.GetMaxOrder();
            return DBManager.ExecuteScalar<short>(base.ConnectionHandler, query);
        }
    }
    internal class VIPCommandBuilder
    {
        public string GetMaxOrder()
        {

            return string.Format("select max([Sort]) from [Congress].[VIP]");
        }
    }
}
