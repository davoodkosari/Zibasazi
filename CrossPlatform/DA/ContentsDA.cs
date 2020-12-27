using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using Radyn.CrossPlatform.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.DA
{
    public sealed class ContentsDA : DALBase<Contents>
    {
        public ContentsDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<Contents> GetContentByCategory(Guid congressId, Guid categoryId)
        {
            var contentCommandBuilder = new ContentsCommandBuilder();
            var query = contentCommandBuilder.GetContentByCategory(congressId, categoryId);
            return DBManager.GetCollection<Contents>(this.ConnectionHandler, query);
        }

       
    }
    internal class ContentsCommandBuilder
    {
        internal string GetContentByCategory(Guid congressId, Guid categoryId)
        {
            return string.Format("SELECT * FROM CrossPlatform.Contents WHERE CongressId = '{0}' AND CategoryId = '{1}'", congressId, categoryId);
        }

        
    }
}
