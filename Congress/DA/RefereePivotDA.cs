using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Congress.DA
{
    public sealed class RefereePivotDA : DALBase<RefereePivot>
    {
        public RefereePivotDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
        public RefereePivot GetByPivotAndRefereeId(Guid? Id, Guid pivotId)
        {
            RefereePivotCommandBuilder homaCommandBuilder = new RefereePivotCommandBuilder();
            var query = homaCommandBuilder.GetByPivotAndRefereeId(Id, pivotId);
            return DBManager.GetObject<RefereePivot>(ConnectionHandler, query);
        }

        public List<RefereePivot> GetAllByRefereeId(Guid Id)
        {
            RefereePivotCommandBuilder homaCommandBuilder = new RefereePivotCommandBuilder();
            SqlCommand query = homaCommandBuilder.GetAllByRefereeId(Id);
            return DBManager.GetCollection<RefereePivot>(ConnectionHandler, query);
        }
    }
    internal class RefereePivotCommandBuilder
    {
        public SqlCommand GetByPivotAndRefereeId(Guid? Id, Guid pivotId)
        {
            var query=new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Id", Id));
            query.Parameters.Add(new SqlParameter("@pivotId", pivotId));
            query.CommandText= "SELECT * FROM [Congress].[RefereePivot] WHERE [RefereeId]=@Id AND [PivotId]=@pivotId";
            return query;
        }

        public SqlCommand GetAllByRefereeId(Guid Id)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@ID", Id));
            query.CommandText = "SELECT * FROM [Congress].[RefereePivot] WHERE [RefereeId]=@ID";
            return query;
        }
    }
}
