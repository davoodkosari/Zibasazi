using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressSlideDA : DALBase<CongressSlide>
    {
        public CongressSlideDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class CongressSlideCommandBuilder
    {
    }
}
