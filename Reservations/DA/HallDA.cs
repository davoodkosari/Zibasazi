using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Reservation.DA
{
    public sealed class HallDA : DALBase<Hall>
    {
        public HallDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public int DeleteChairs(Guid hallId)
        {
            HallCommandBuilder commandBuilder = new HallCommandBuilder();
            SqlCommand query = commandBuilder.DeleteChairs(hallId);
            return DBManager.ExecuteNonQuery(base.ConnectionHandler, query);
        }

        public IEnumerable<Hall> GetParents()
        {
            HallCommandBuilder commandBuilder = new HallCommandBuilder();
            SqlCommand query = commandBuilder.GetParents();
            return DBManager.GetCollection<Hall>(base.ConnectionHandler, query);

        }

        public decimal DeleteChairtypes(Guid hallId)
        {
            HallCommandBuilder commandBuilder = new HallCommandBuilder();
            SqlCommand query = commandBuilder.DeleteChairtypes(hallId);
            return DBManager.ExecuteNonQuery(base.ConnectionHandler, query);
        }
    }
    internal class HallCommandBuilder
    {
        public SqlCommand DeleteChairs(Guid hallId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Hall", hallId));
            query.CommandText = "Delete FROM [Reservation].[Chair] where HallId=@Hall ";
            return query;
        }

        public SqlCommand GetParents()
        {
            SqlCommand query = new SqlCommand()
            {
                CommandText = "Select * FROM [Reservation].[Hall] where ParentId Is null and IsExternal=0 "
            };
            return query;
        }

        public SqlCommand DeleteChairtypes(Guid hallId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Hall", hallId));
            query.CommandText = "Delete FROM [Reservation].[ChairType] where HallId=@Hall ";
            return query;
        }
    }
}
