using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class ArticlePaymentTypeDA : DALBase<UserRegisterPaymentType>
    {
        public ArticlePaymentTypeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
       
    }
    internal class ArticlePaymentTypeCommandBuilder
    {
       
    }
}
