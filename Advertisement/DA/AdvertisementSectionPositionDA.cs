using Radyn.Advertisements.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.DA
{
    public sealed class AdvertisementSectionPositionDA : DALBase<AdvertisementSectionPosition>
    {
        public AdvertisementSectionPositionDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public AdvertisementSectionPosition GetByKeyWord(string keyWord)
        {
            var advertisementSectionPositionCommandBuilder = new AdvertisementSectionPositionCommandBuilder();
            var query = advertisementSectionPositionCommandBuilder.GetByKeyWord(keyWord);
            return DBManager.GetObject<AdvertisementSectionPosition>(base.ConnectionHandler, query);
        }
    }
    internal class AdvertisementSectionPositionCommandBuilder
    {
        public string GetByKeyWord(string keyWord)
        {
            return string.Format("SELECT     top(1) *" +
                                 " FROM         Advertisement.AdvertisementSectionPosition where KeyWord='{0}'", keyWord);
        }
    }
}
