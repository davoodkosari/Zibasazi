using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class ArticlePaymentTypeBO : BusinessBase<ArticlePaymentType>
    {
        public override bool Insert(IConnectionHandler connectionHandler, ArticlePaymentType obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }
    }
}
