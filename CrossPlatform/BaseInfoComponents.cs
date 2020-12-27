using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.Facade;
using Radyn.CrossPlatform.Facade.Interface;

namespace Radyn.CrossPlatform
{
    public class BaseInfoComponents
    {
        internal BaseInfoComponents()
        {

        }

        public ISyncAdapterFacade SyncAdapterFacade
        {
            get { return new SyncAdapterFacade(); }
        }

        public IContentCategoriesFacade ContentCategoriesFacade
        {
            get { return new ContentCategoriesFacade(); }
        }
        public IContentsFacade ContentsFacade
        {
            get { return new ContentsFacade(); }
        }
    }
}
