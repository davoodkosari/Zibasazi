using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DA;
using Radyn.Security.DataStructure;

namespace Radyn.Security.BO
{
    internal class OperationBO : BusinessBase<Operation>
    {
        public List<Operation> GetAllByUserId(IConnectionHandler connectionHandler, Guid userId)
        {
            var da = new OperationDA(connectionHandler);
            return da.GetAllByUserId(userId);
        }
        public async Task<List<Operation>> GetAllByUserIdAsync(IConnectionHandler connectionHandler, Guid userId)
        {
            var da = new OperationDA(connectionHandler);
            return await da.GetAllByUserIdAsync(userId);
        }
        public IEnumerable<Operation> GetNotAddedInRole(IConnectionHandler connectionHandler, Guid roleId)
        {
            var da = new OperationDA(connectionHandler);
            return da.GetNotAddedInRole(roleId);
        }

        public IEnumerable<Operation> GetNotAddedInUser(IConnectionHandler connectionHandler, Guid userId)
        {
            var da = new OperationDA(connectionHandler);
            return da.GetNotAddedInUser(userId);
        }
    }
}
