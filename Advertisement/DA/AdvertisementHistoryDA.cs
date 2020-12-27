using Radyn.Advertisements.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.DA
{
    public sealed class AdvertisementHistoryDA : DALBase<AdvertisementHistory>
    {
        public AdvertisementHistoryDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class AdvertisementHistoryCommandBuilder
    {
    }
}
