using System;
using System.Data.SqlClient;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class RefereeCartableDA : DALBase<RefereeCartable>
    {
        public RefereeCartableDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public bool DeleteFromRefreeCartable(Guid articleId, Guid refereId)
        {
            var commandBuilder = new RefereeCartableCommandBuilder();
            var query = commandBuilder.DeleteFromRefreeCartable(articleId, refereId);
            return DBManager.ExecuteNonQuery(base.ConnectionHandler, query) > 0;
        }

      
    }
    internal class RefereeCartableCommandBuilder
    {

        public SqlCommand DeleteFromRefreeCartable(Guid articleId, Guid refereId)
        {
            var query=new SqlCommand();
            query.Parameters.Add(new SqlParameter("@articleId", articleId));
            query.Parameters.Add(new SqlParameter("@refereId", refereId));
            query.CommandText= "DELETE FROM Congress.RefereeCartable " +
                "WHERE RefereeCartable.ArticleId = @articleId AND RefereeCartable.RefereeId = @refereId";
            return query;
        }


      
    }

}
