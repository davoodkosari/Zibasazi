using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Radyn.ContentManager.DA
{
    public sealed class PartialLoadDA : DALBase<PartialLoad>
    {
        public PartialLoadDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<PartialLoad> GetForUpdatePostion(PartialLoad partialLoad, byte position)
        {
            PartialLoadCommandBuilder partialLoadCommandBuilder = new PartialLoadCommandBuilder();
            SqlCommand query = partialLoadCommandBuilder.GetForUpdatePostion(partialLoad.HtmlDesginId, partialLoad.CustomId, position);
            return DBManager.GetCollection<PartialLoad>(base.ConnectionHandler, query);
        }
    }
    internal class PartialLoadCommandBuilder
    {
        public SqlCommand GetForUpdatePostion(Guid htmlDesginId, string customId, byte position)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@htmlDesginId", htmlDesginId));
            query.Parameters.Add(new SqlParameter("@customId", customId));
            query.Parameters.Add(new SqlParameter("@position", position));
            query.CommandText =
               "SELECT        *FROM      ContentManage.PartialLoad where ContentManage.PartialLoad.HtmlDesginId=@htmlDesginId and ContentManage.PartialLoad.CustomId=@customId and ContentManage.PartialLoad.position >=@position ";
            return query;
        }
    }
}
