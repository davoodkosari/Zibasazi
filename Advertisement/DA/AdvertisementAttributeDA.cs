using Radyn.Advertisements.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.DA
{
    public sealed class AdvertisementAttributeDA : DALBase<AdvertisementAttribute>
    {
        public AdvertisementAttributeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class AdvertisementAttributeCommandBuilder
    {
    }
}
