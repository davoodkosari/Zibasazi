using Radyn.Advertisements.Facade;
using Radyn.Advertisements.Facade.Interface;

namespace Radyn.Advertisements
{
    public sealed class AdvertisementsComponent
    {
        private AdvertisementsComponent()
        {

        }

        private static AdvertisementsComponent _instance;
        public static AdvertisementsComponent Instance
        {
            get { return _instance ?? (_instance = new AdvertisementsComponent()); }
        }

        public IAdvertisementFacade AdvertisementFacade
        {
            get
            {
                return new AdvertisementFacade();
            }
        }


        public IAdvertisementSectionFacade AdvertisementSectionFacade
        {
            get
            {
                return new AdvertisementSectionFacade();
            }
        }


        public IAdvertisementSectionPositionFacade AdvertisementSectionPositionFacade
        {
            get
            {
                return new AdvertisementSectionPositionFacade();
            }
        }


        public IAdvertisementTypeFacade AdvertisementTypeFacade
        {
            get
            {
                return new AdvertisementTypeFacade();
            }
        }


        public ITariffFacade TariffFacade
        {
            get
            {
                return new TariffFacade();
            }
        }


        public ITariffClassFacade TariffClassFacade
        {
            get
            {
                return new TariffClassFacade();
            }
        }
    }
}
