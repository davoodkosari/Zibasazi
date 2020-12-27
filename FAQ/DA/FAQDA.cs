using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.FAQ.DA
{
    public sealed class FAQDA : DALBase<FAQ.DataStructure.FAQ>
    {
        public FAQDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<DataStructure.FAQ> Search(string value)
        {
            FAQCommandBuilder faqCommandBuilder = new FAQCommandBuilder();
            SqlCommand query = faqCommandBuilder.Search(value);
            return DBManager.GetCollection<DataStructure.FAQ>(base.ConnectionHandler, query);
        }
    }
    internal class FAQCommandBuilder
    {
        public SqlCommand Search(string value)
        {
            SqlCommand query = new SqlCommand();

            string where = "";
            string q =
                string.Format(
                    "SELECT     distinct    FAQ.FAQ.Id, FAQ.FAQ.Question, cast(FAQ.FAQ.Answer as nvarchar(4000)) as Answer , FAQ.FAQ.CreatorId, FAQ.FAQ.CreateDate, FAQ.FAQ.ViewCount, FAQ.FAQ.ThumbnailId, " +
                    "FAQ.FAQ.IsExternal FROM            FAQ.FAQ INNER JOIN " +
                    " FAQ.FAQContent ON FAQ.FAQ.Id = FAQ.FAQContent.Id ");
            if (!string.IsNullOrEmpty(value))
            {
                query.Parameters.Add(new SqlParameter("@VALUE", value));
                where += "FAQ.FAQContent.Question like N'%@VALUE%' or ";
                where += "FAQ.FAQContent.Answer like N'%@VALUE%' or ";
            }
            if (!string.IsNullOrEmpty(where))
            {
                where = where.Substring(0, where.Length - 3);
                q = string.Format("{0} where {1} ", q, where);
            }
            query.CommandText = q + " order by CreateDate desc ";
            return query;
        }
    }
}
