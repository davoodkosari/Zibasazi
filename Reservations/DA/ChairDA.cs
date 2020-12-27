using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Radyn.Reservation.DA
{
    public sealed class ChairDA : DALBase<Chair>
    {
        public ChairDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public Chair GetByPosition(Guid hallId, int column, int row)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            string query = builder.GetByPosition(hallId, column, row);
            return DBManager.GetObject<Chair>(base.ConnectionHandler, query);
        }
        public int UpdateChairs(List<string> chairs)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            var query = builder.UpdateChairs(chairs);
            return DBManager.ExecuteNonQuery(base.ConnectionHandler, query);
        }
        public int InsertChairs(List<Chair> chairs)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            SqlCommand query = builder.InsertChairs(chairs);
            return DBManager.ExecuteNonQuery(base.ConnectionHandler, query);
        }
        public List<Chair> GetHallChairs(Guid hallId)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            var query = builder.GetHallChairs(hallId);
            return DBManager.GetCollection<Chair>(base.ConnectionHandler, query);
        }

        public int AllowDelete(Guid hallId)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            SqlCommand query = builder.AllowDelete(hallId);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }

        public int HasInChairType(Guid chairtypeId)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            SqlCommand query = builder.HasInChairType(chairtypeId);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }

        public IEnumerable<Chair> GetListChairByIdList(List<Guid> list)
        {
            ChairCommandBuilder builder = new ChairCommandBuilder();
            SqlCommand query = builder.GetListChairByIdList(list);
            return DBManager.GetCollection<Chair>(base.ConnectionHandler, query);
        }
    }
    internal class ChairCommandBuilder
    {
        public string GetByPosition(Guid hallId, int column, int row)
        {
            return
                string.Format(
                    "select top(1)* from [Reservation].[Chair] where [HallId]='{0}' and [Row]={1} and [Column]={2}", hallId,
                    row, column);
        }

        public SqlCommand UpdateChairs(List<string> chairs)
        {
            var query=new SqlCommand();
            var counter = 100;
            string queryList = "";
            foreach (string[] split in from chair in chairs where !string.IsNullOrEmpty(chair) select chair.Split(','))
            {
                var id = $"@Id{++counter}";
                var number = $"@Number{++counter}";
                var status = $"@Status{++counter}";
                var type = $"@Type{++counter}";
                query.Parameters.Add(new SqlParameter(id, split[2]));
                query.Parameters.Add(new SqlParameter(number, split[4]));
                query.Parameters.Add(new SqlParameter(status, split[3]));
                query.Parameters.Add(new SqlParameter(type,
                    (split[5] != Guid.Empty.ToString() ? "'" + split[5] + "'" : "null")));
                queryList +=
                    string.Format(
                        "update [Reservation].[Chair] set [ChairTypeId]={1},[Status]={2},[Number]={3} where   Id={0}  ",
                        id, type, status, number);
                queryList += "\n";
            }

            query.CommandText = queryList;
            return query;
        }
        //(Row,Column,Id,Status,Number,Amount)
        public SqlCommand GetHallChairs(Guid hallId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Hall", hallId));
            query.CommandText =
                 string.Format(
                   "select   [Reservation].[Chair].*, cast([Row] as varchar(100))+'-'+cast([Column] as varchar(100)) as CellValue,cast([Row] as varchar(100))+','+" +
                   "cast([Column] as varchar(100))+','+cast((cast([Id] as varchar(100))+','+cast([Status] as varchar(100))+','+" +
                   "cast([Number] as varchar(100))+','+cast(coalesce(nullif([ChairTypeId],'{0}'),'{0}') as varchar(200))+','+cast(coalesce(nullif([OwnerId],'{0}'),'{0}') as varchar(200))) as varchar(1000)) as ColumValue" +
                   "  from [Reservation].[Chair] where [HallId]=@Hall order by [Row],[Column]",
                     Guid.Empty);
            return query;
        }

        public SqlCommand InsertChairs(List<Chair> chairs)
        {
            string queryList = "";
            SqlCommand query = new SqlCommand();
            int counter = 100;
            foreach (Chair chair in chairs)
            {
                string id = $"@Id{++counter}";
                string hall = $"@Hall{++counter}";
                string nubmer = $"@Number{++counter}";
                string type = $"@Type{++counter}";
                string row = $"@Row{++counter}";
                string col = $"@Col{++counter}";
                string status = $"@Status{++counter}";
                query.Parameters.Add(new SqlParameter(id, chair.Id));
                query.Parameters.Add(new SqlParameter(hall, chair.Hall));
                query.Parameters.Add(new SqlParameter(nubmer, chair.Number));
                query.Parameters.Add(new SqlParameter(type, (chair.ChairTypeId != null ? "'" + chair.ChairTypeId + "'" : "null")));
                query.Parameters.Add(new SqlParameter(row, chair.Row));
                query.Parameters.Add(new SqlParameter(col, chair.Column));
                query.Parameters.Add(new SqlParameter(status, chair.Status));
                queryList +=
                    string.Format(
                        "INSERT INTO [Reservation].[Chair]([Id],[HallId],[Number],[ChairTypeId],[Row],[Column],[Status]) " +
                        " VALUES({0},{1},{2},{3},{4},{5},{6})",
                        id, hall, nubmer, type, row, col, status);
                queryList += "\n";
            }
            query.CommandText = queryList;
            return query;
        }

        public SqlCommand AllowDelete(Guid hallId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Hall", hallId));
            query.CommandText =
                string.Format(
                    "select count(Id) from [Reservation].[Chair] where [HallId]=@Hall and ([Status]={0} or [Status]={1}) ",
                    (byte)Enums.ReservStatus.Saled, (byte)Enums.ReservStatus.Reserved);
            return query;
        }

        public SqlCommand HasInChairType(Guid chairtypeId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Chair", chairtypeId));
            query.CommandText =
              "select count(Id) from [Reservation].[Chair] where [ChairTypeId]= @Chair";
            return query;
        }

        public SqlCommand GetListChairByIdList(List<Guid> list)
        {
            SqlCommand query = new SqlCommand();
            string str = string.Empty;
            foreach (Guid guid in list)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }

                str += "'" + guid + "'";

            }
            query.CommandText =
                string.Format(
                    "SELECT  * FROM    Reservation.Chair  where Reservation.Chair.Id in ({0})", str);

            return query;
        }
    }
}
