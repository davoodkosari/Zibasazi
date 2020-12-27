using Radyn.CrossPlatform.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.DA
{
    public sealed class ContentCategoriesDA : DALBase<ContentCategories>
    {
        public ContentCategoriesDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class ContentCategoriesCommandBuilder
    {
    }
}
