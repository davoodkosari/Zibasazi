using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.WebDesign.DA
{
    public sealed class SliderDA : DALBase<DataStructure.Slider>
    {
        public SliderDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class SliderCommandBuilder
    {
    }
}
