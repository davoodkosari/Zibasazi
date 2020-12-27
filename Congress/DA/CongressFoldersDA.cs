using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressFoldersDA : DALBase<CongressFolders>
    {
        public CongressFoldersDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class CongressFoldersCommandBuilder
    {
    }
}
