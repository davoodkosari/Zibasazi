using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment.DataStructure;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Payment.DA
{
    public sealed class DiscountTypeDA : DALBase<DiscountType>
    {
        public DiscountTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<DiscountType> GetDiscountTypes(string modualName, byte section)
        {
            DiscountTypeCommandBuilder commandBuilder = new DiscountTypeCommandBuilder();
            SqlCommand query = commandBuilder.GetDiscountTypes(modualName, section);
            return DBManager.GetCollection<DiscountType>(base.ConnectionHandler, query);
        }


    }
    internal class DiscountTypeCommandBuilder
    {
        public SqlCommand GetDiscountTypes(string modualName, byte section)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Modul", modualName));
            query.Parameters.Add(new SqlParameter("@Section", section));
            query.CommandText =
                "SELECT     Payment.DiscountType.*" +
                " FROM         Payment.DiscountType INNER JOIN " +
                " Payment.DiscountTypeSection ON Payment.DiscountType.Id = Payment.DiscountTypeSection.DiscountTypeId" +
                " WHERE Payment.DiscountTypeSection.MoudelName=@Modul AND Payment.DiscountTypeSection.Section=@Section ";
            return query;

        }


    }
}
