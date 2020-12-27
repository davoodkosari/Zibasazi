using Radyn.Advertisements.DA;
using Radyn.Advertisements.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.BO
{
    internal class AdvertisementSectionPositionBO : BusinessBase<AdvertisementSectionPosition>
    {
        public AdvertisementSectionPosition GetByKeyWord(IConnectionHandler handler, string keyWord)
        {
            var advertisementSectionPositionDa = new AdvertisementSectionPositionDA(handler);
            return advertisementSectionPositionDa.GetByKeyWord(keyWord);
        }
    }
}
