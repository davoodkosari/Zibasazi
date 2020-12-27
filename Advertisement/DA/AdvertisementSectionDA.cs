using Radyn.Advertisements.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.DA
{
    public sealed class AdvertisementSectionDA : DALBase<AdvertisementSection>
    {
        public AdvertisementSectionDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class AdvertisementSectionCommandBuilder
    {
    }
}
