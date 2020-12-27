using Radyn.EnterpriseNode.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Radyn.EnterpriseNode.DAL
{
    public sealed class EnterpriseNodeDA : DALBase<DataStructure.EnterpriseNode>
    {
        public EnterpriseNodeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public List<DataStructure.EnterpriseNode> Search(DataStructure.EnterpriseNode filter)
        {
            EnterpriseNodeCommandBuilder enterpriseNodeCommandBuilder = new EnterpriseNodeCommandBuilder();
            SqlCommand query = enterpriseNodeCommandBuilder.Search(filter);
            return DBManager.GetCollection<DataStructure.EnterpriseNode>(base.ConnectionHandler, query);
        }

        public List<DataStructure.EnterpriseNode> Search(string filter, Enums.EnterpriseNodeType enterpriseNodeTypeId = Enums.EnterpriseNodeType.RealEnterPriseNode)
        {
            EnterpriseNodeCommandBuilder enterpriseNodeCommandBuilder = new EnterpriseNodeCommandBuilder();
            SqlCommand query = enterpriseNodeCommandBuilder.Search(filter, enterpriseNodeTypeId);
            return DBManager.GetCollection<DataStructure.EnterpriseNode>(base.ConnectionHandler, query);
        }

    }
    internal class EnterpriseNodeCommandBuilder
    {
        public SqlCommand Search(DataStructure.EnterpriseNode obj)
        {
            SqlCommand q = new SqlCommand();
            string query = string.Empty;
            string whereCluase = "";
            if (obj.EnterpriseNodeTypeId == (int)Enums.EnterpriseNodeType.RealEnterPriseNode || obj.RealEnterpriseNode != null)
            {

                query = string.Format("SELECT     EnterpriseNode.EnterpriseNode.* FROM         EnterpriseNode.EnterpriseNode INNER JOIN " +
                      " EnterpriseNode.RealEnterpriseNode ON EnterpriseNode.EnterpriseNode.Id = EnterpriseNode.RealEnterpriseNode.Id ");

                if (!string.IsNullOrEmpty(obj.RealEnterpriseNode.FirstName))
                {
                    q.Parameters.Add(new SqlParameter("@FNAME", obj.RealEnterpriseNode.FirstName));
                    whereCluase += "EnterpriseNode.RealEnterpriseNode.FirstName LIKE N'%@FNAME%' OR ";
                }

                if (!string.IsNullOrEmpty(obj.RealEnterpriseNode.LastName))
                {
                    q.Parameters.Add(new SqlParameter("@LNAME", obj.RealEnterpriseNode.LastName));
                    whereCluase += "EnterpriseNode.RealEnterpriseNode.LastName LIKE N'%@LNAME%' OR ";
                }

                if (!string.IsNullOrEmpty(obj.RealEnterpriseNode.NationalCode))
                {
                    q.Parameters.Add(new SqlParameter("@NationalCode", obj.RealEnterpriseNode.NationalCode));
                    whereCluase += "EnterpriseNode.RealEnterpriseNode.NationalCode LIKE '%@NationalCode%' OR ";
                }

                if (!string.IsNullOrEmpty(obj.RealEnterpriseNode.IDNumber))
                {
                    q.Parameters.Add(new SqlParameter("@IDNumber", obj.RealEnterpriseNode.IDNumber));
                    whereCluase += "EnterpriseNode.RealEnterpriseNode.IDNumber LIKE '%@IDNumber%' OR ";
                }
            }
            if (obj.EnterpriseNodeTypeId == (int)Enums.EnterpriseNodeType.LegalEnterPriseNode || obj.LegalEnterpriseNode != null)
            {
                query = string.Format("SELECT     EnterpriseNode.EnterpriseNode.* FROM         EnterpriseNode.EnterpriseNode INNER JOIN " +
                      " EnterpriseNode.LegalEnterpriseNode ON EnterpriseNode.EnterpriseNode.Id = EnterpriseNode.LegalEnterpriseNode.Id ");
                if (!string.IsNullOrEmpty(obj.LegalEnterpriseNode.Title))
                {
                    q.Parameters.Add(new SqlParameter("@TITLE", obj.LegalEnterpriseNode.Title));
                    whereCluase += "EnterpriseNode.LegalEnterpriseNode.Title LIKE N'%@TITLE%' OR ";
                }

                if (!string.IsNullOrEmpty(obj.LegalEnterpriseNode.RegisterNo))
                {
                    q.Parameters.Add(new SqlParameter("@RegisterNo", obj.LegalEnterpriseNode.RegisterNo));
                    whereCluase += "EnterpriseNode.LegalEnterpriseNode.RegisterNo LIKE '%@RegisterNo%' OR ";
                }

                if (!string.IsNullOrEmpty(obj.LegalEnterpriseNode.NationalId))
                {
                    q.Parameters.Add(new SqlParameter("@NationalId", obj.LegalEnterpriseNode.NationalId));
                    whereCluase += "EnterpriseNode.LegalEnterpriseNode.NationalId LIKE '%@NationalId%' OR ";
                }
            }
            if (!string.IsNullOrEmpty(obj.Address))
            {
                q.Parameters.Add(new SqlParameter("@Address", obj.Address));
                whereCluase += "EnterpriseNode.[Address] LIKE '%@Address%' OR ";
            }

            if (!string.IsNullOrEmpty(obj.Website))
            {
                q.Parameters.Add(new SqlParameter("@Website", obj.Website));
                whereCluase += "EnterpriseNode.Website LIKE '%@Website%' OR ";
            }

            if (!string.IsNullOrEmpty(obj.Cellphone))
            {
                q.Parameters.Add(new SqlParameter("@Cellphone", obj.Cellphone));
                whereCluase += "EnterpriseNode.Cellphone LIKE '%@Cellphone%' OR ";
            }

            if (!string.IsNullOrEmpty(obj.Tel))
            {
                q.Parameters.Add(new SqlParameter("@Tel", obj.Tel));
                whereCluase += "EnterpriseNode.Tel LIKE '%@Tel%' OR ";
            }

            if (!string.IsNullOrEmpty(whereCluase))
            {
                whereCluase = whereCluase.Substring(0, whereCluase.Length - 3);
                q.CommandText = string.Format("{0} WHERE {1}  ", query, whereCluase);
            }
            return q;

        }

        public SqlCommand Search(string filter, Enums.EnterpriseNodeType enterpriseNodeTypeId = Enums.EnterpriseNodeType.RealEnterPriseNode)
        {
            SqlCommand q = new SqlCommand();
            string query = string.Empty;
            string whereCluase = "";
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
            }

            q.Parameters.Add(new SqlParameter("@FILTER", filter));
            if (enterpriseNodeTypeId == Enums.EnterpriseNodeType.RealEnterPriseNode)
            {

                query = string.Format("SELECT     EnterpriseNode.EnterpriseNode.* FROM         EnterpriseNode.EnterpriseNode INNER JOIN " +
                      " EnterpriseNode.RealEnterpriseNode ON EnterpriseNode.EnterpriseNode.Id = EnterpriseNode.RealEnterpriseNode.Id ");


                whereCluase += "lower(EnterpriseNode.RealEnterpriseNode.FirstName) LIKE N'%@FILTER%' OR ";
                whereCluase += "lower(EnterpriseNode.RealEnterpriseNode.LastName) LIKE N'%@FILTER%' OR ";
                whereCluase += "lower(EnterpriseNode.RealEnterpriseNode.NationalCode) LIKE '%@FILTER%' OR ";
                whereCluase += "lower(EnterpriseNode.RealEnterpriseNode.IDNumber) LIKE '%@FILTER%' OR ";

            }
            if (enterpriseNodeTypeId == Enums.EnterpriseNodeType.LegalEnterPriseNode)
            {
                query = string.Format("SELECT     EnterpriseNode.EnterpriseNode.* FROM         EnterpriseNode.EnterpriseNode INNER JOIN " +
                      " EnterpriseNode.LegalEnterpriseNode ON EnterpriseNode.EnterpriseNode.Id = EnterpriseNode.LegalEnterpriseNode.Id ");

                whereCluase += "lower(EnterpriseNode.LegalEnterpriseNode.Title) LIKE N'%@FILTER%' OR ";
                whereCluase += "lower(EnterpriseNode.LegalEnterpriseNode.RegisterNo) LIKE '%@FILTER%' OR ";
                whereCluase += "lower(EnterpriseNode.LegalEnterpriseNode.NationalId) LIKE '%@FILTER%' OR ";

            }

            whereCluase += "lower(EnterpriseNode.[Address]) LIKE '%@FILTER%' OR ";
            whereCluase += "lower(EnterpriseNode.Website) LIKE '%@FILTER%' OR ";
            whereCluase += "lower(EnterpriseNode.Cellphone) LIKE '%@FILTER%' OR ";
            whereCluase += "lower(EnterpriseNode.Tel) LIKE '%@FILTER%' OR ";
            whereCluase += "lower(EnterpriseNode.Email) LIKE '%@FILTER%' OR ";

            if (!string.IsNullOrEmpty(whereCluase))
            {
                whereCluase = whereCluase.Substring(0, whereCluase.Length - 3);
                q.CommandText = string.Format("{0} WHERE {1}  ", query, whereCluase);
            }
            return q;

        }

    }
}
