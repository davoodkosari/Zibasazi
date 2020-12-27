using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class AdvertisementSectionFacade : AdvertisementsBaseFacade<AdvertisementSection>, IAdvertisementSectionFacade
    {
        internal AdvertisementSectionFacade() { }

        internal AdvertisementSectionFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

    }
}
