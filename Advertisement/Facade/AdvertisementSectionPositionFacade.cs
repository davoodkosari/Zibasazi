using Radyn.Advertisements.BO;
using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class AdvertisementSectionPositionFacade : AdvertisementsBaseFacade<AdvertisementSectionPosition>, IAdvertisementSectionPositionFacade
    {
        internal AdvertisementSectionPositionFacade() { }

        internal AdvertisementSectionPositionFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        protected override AdvertisementSectionPosition FillComplex(AdvertisementSectionPosition item)
        {
            if (item == null) return null;
            item.AdvertisementSection = new AdvertisementSectionBO().Get(base.ConnectionHandler, item.SectionId);
            return item;
        }
    }
}
