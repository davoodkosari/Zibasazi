using Radyn.Advertisements.BO;
using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class TariffClassFacade : AdvertisementsBaseFacade<TariffClass>, ITariffClassFacade
    {
        internal TariffClassFacade() { }

        internal TariffClassFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        protected override TariffClass FillComplex(TariffClass item)
        {
            if (item == null) return null;
            item.AdvertisementSectionPosition = new AdvertisementSectionPositionBO().Get(base.ConnectionHandler, item.SectionPositionId);
            return item;
        }
    }
}
