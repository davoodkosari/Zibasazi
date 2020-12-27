using Radyn.Advertisements.BO;
using Radyn.Advertisements.DataStructure;
using Radyn.Advertisements.Facade.Interface;
using Radyn.EnterpriseNode;
using Radyn.Framework.DbHelper;

namespace Radyn.Advertisements.Facade
{
    internal sealed class CustomerAdvertisementFacade : AdvertisementsBaseFacade<CustomerAdvertisement>, ICustomerAdvertisementFacade
    {
        internal CustomerAdvertisementFacade() { }

        internal CustomerAdvertisementFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

        protected override CustomerAdvertisement FillComplex(CustomerAdvertisement item)
        {
            if (item == null) return null;
            item.EnterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(item.CustomerId);
            item.Advertisement = new AdvertisementBO().Get(base.ConnectionHandler, item.AdvertisementId);
            return item;
        }
    }
}
