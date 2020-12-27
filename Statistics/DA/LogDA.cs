using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Data.SqlClient;

namespace Radyn.Statistics.DA
{
    public sealed class LogDA : DALBase<DataStructure.Log>
    {
        public LogDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public int GetDateSesstionCount(string url, string date)
        {
            LogCommandBuilder commandBuilder = new LogCommandBuilder();
            SqlCommand query = commandBuilder.GetDateSesstionCount(url, date);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }

        public int GetSesstionCountBetweenDate(string url, string minDate, string maxDate)
        {
            LogCommandBuilder commandBuilder = new LogCommandBuilder();
            SqlCommand query = commandBuilder.GetSesstionCountBetweenDate(url, minDate, maxDate);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }


        public int GetTotalSesstionCount(string url)
        {
            LogCommandBuilder commandBuilder = new LogCommandBuilder();
            SqlCommand query = commandBuilder.GetTotalSesstionCount(url);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }


    }
    internal class LogCommandBuilder
    {
        //بازدید کننده
        public SqlCommand GetDateSesstionCount(string url, string date)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@url", url));
            query.Parameters.Add(new SqlParameter("@Date", date));
            query.CommandText =
                "select count(distinct [SesstionId])  from [Statistics].[Log] where lower([Url])=@url and  CAST([Date] AS DATE)=@Date";
            return query;
        }

        //بازدید کننده
        public SqlCommand GetSesstionCountBetweenDate(string url, string mindate, string maxdate)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@url", url));
            query.Parameters.Add(new SqlParameter("@mindate", mindate));
            query.Parameters.Add(new SqlParameter("@maxdate", maxdate));
            query.CommandText =
                "select count(distinct [SesstionId])  from [Statistics].[Log] where lower([Url])=@url and CAST([Date] AS DATE) <=@maxdate and CAST([Date] AS DATE) >=@mindate  ";
            return query;
        }


        //تعدا کل بازدید کننده
        public SqlCommand GetTotalSesstionCount(string url)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@url", url));
            query.CommandText =
             "select count(distinct [SesstionId])  from [Statistics].[Log] where lower([Url])=@url ";
            return query;
        }
    }
}
