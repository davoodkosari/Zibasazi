using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class FolderDA : DALBase<Folder>
    {
        public FolderDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class FolderCommandBuilder
    {
    }
}
