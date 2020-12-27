using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressDiscountTypeDA : DALBase<CongressDiscountType>
    {
        public CongressDiscountTypeDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class CongressDiscountTypeCommandBuilder
    {
    }
}
