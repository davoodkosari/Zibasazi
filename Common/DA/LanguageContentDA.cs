using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Common.DA
{
    public sealed class LanguageContentDA : DALBase<LanguageContent>
    {
        public LanguageContentDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }

        public List<LanguageContent> GetByMoudelname(string key, string culture)
        {

            LanguageContentCommandBuilder builder = new LanguageContentCommandBuilder();
            var query = builder.GetByMoudelname(key, culture);
            return DBManager.GetCollection<LanguageContent>(base.ConnectionHandler, query);

        }
    }
    internal class LanguageContentCommandBuilder
    {
        public SqlCommand GetByMoudelname(string key, string culture)
        {
            var query=new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Key", key));
            query.Parameters.Add(new SqlParameter("@culture", culture));
            query.CommandText=
             "Select distinct * from [Common].[LanguageContent] where [Key] like '%@Key%' and LanguageId=@culture ";
            return query;
        }
    }
}
