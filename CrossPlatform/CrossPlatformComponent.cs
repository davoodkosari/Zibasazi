using Radyn.CrossPlatform.Facade;
using Radyn.CrossPlatform.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform
{
    public class CrossPlatformComponent
    {

        internal CrossPlatformComponent()
        {

        }
        private static CrossPlatformComponent _instance;
        public static CrossPlatformComponent Instance
        {
            get
            {
                return _instance ?? new CrossPlatformComponent();
            }

        }

        private BaseInfoComponents _baseInfoComponents;
        public BaseInfoComponents BaseInfoComponents
        {
            get
            {
                return _baseInfoComponents ?? new BaseInfoComponents();
            }
        }

        public ISyncAdapterFacade SyncAdapterFacadeTransactional(IConnectionHandler connectionHandler)
        {
            return new SyncAdapterFacade(connectionHandler);
        }

        public void Initialize()
        {
        }

    }
}
