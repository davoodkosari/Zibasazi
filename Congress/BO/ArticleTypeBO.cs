using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class ArticleTypeBO : BusinessBase<ArticleType>
    {
        public override bool Insert(IConnectionHandler connectionHandler, ArticleType obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);

        }
    }
}
