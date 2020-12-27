using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.WebDesign.DA
{
    public sealed class NewsDA : DALBase<DataStructure.News>
    {
        public NewsDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class NewsCommandBuilder
    {
    }
}
