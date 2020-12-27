using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.WebDesign.DA
{
    public sealed class FAQDA : DALBase<DataStructure.FAQ>
    {
        public FAQDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class FAQCommandBuilder
    {
    }
}
