using Radyn.Advertisements.BO;
using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class AdvertisementHistoryFacade : AdvertisementsBaseFacade<AdvertisementHistory>, IAdvertisementHistoryFacade
    {
        internal AdvertisementHistoryFacade() { }

        internal AdvertisementHistoryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        protected override AdvertisementHistory FillComplex(AdvertisementHistory item)
        {
            if (item == null) return null;
            item.Advertisement = new AdvertisementBO().Get(base.ConnectionHandler, item.AdvertisementId);
            item.TariffClassHistory = new TariffClassHistoryBO().Get(base.ConnectionHandler, item.TarrifClassHistoryId);
            return item;
        }
    }
}
