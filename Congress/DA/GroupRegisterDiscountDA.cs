using System.Data.SqlClient;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class GroupRegisterDiscountDA : DALBase<GroupRegisterDiscount>
    {
        public GroupRegisterDiscountDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public int AllowAdd(GroupRegisterDiscount item)
        {
            GroupRegisterDiscountCommandBuilder builder = new GroupRegisterDiscountCommandBuilder();
            var query = builder.AllowAdd(item);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }
    }
    internal class GroupRegisterDiscountCommandBuilder
    {
        public SqlCommand AllowAdd(GroupRegisterDiscount item)
        {
            var q = new SqlCommand();
            q.Parameters.Add(new SqlParameter("@HomeId", item.CongressId));
            q.Parameters.Add(new SqlParameter("@ID", item.Id));
            q.Parameters.Add(new SqlParameter("@From", item.From));
            q.Parameters.Add(new SqlParameter("@TO", item.To));
            string query = "SELECT   count(distinct Id) FROM   Congress.GroupRegisterDiscount ";
            string where = "";
            where += " Congress.GroupRegisterDiscount.CongressId=@HomeId and ";
            where += " Congress.GroupRegisterDiscount.Id!=@ID and ";
            where += " ((Congress.GroupRegisterDiscount.[From]>=@From and  Congress.GroupRegisterDiscount.[From]<=@TO) or  " +
                                " (Congress.GroupRegisterDiscount.[To]>=@From and  Congress.GroupRegisterDiscount.[To]<=@TO))  and ";
            q.CommandText= string.Format("{0} where {1} ", query, where.Substring(0, where.Length - 4));
            return q;


        }

    }
}
