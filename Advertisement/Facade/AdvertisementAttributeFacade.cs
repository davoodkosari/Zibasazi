using Radyn.Advertisements.BO;
using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class AdvertisementAttributeFacade : AdvertisementsBaseFacade<AdvertisementAttribute>, IAdvertisementAttributeFacade
    {
        internal AdvertisementAttributeFacade() { }

        internal AdvertisementAttributeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        protected override AdvertisementAttribute FillComplex(AdvertisementAttribute item)
        {
            if (item == null) return null;
            item.Advertisement = new AdvertisementBO().Get(base.ConnectionHandler, item.Id);
            item.Tariff = new TariffBO().Get(base.ConnectionHandler, item.TariffId);
            item.Advertisement = new AdvertisementBO().Get(base.ConnectionHandler, item.AdvertisemntId);
            return item;
        }
    }
}
