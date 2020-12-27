using System.Configuration;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform
{
    public abstract class CrossPlatformBaseFacade<T> : BaseFacade<T> where T : class
    {
        protected CrossPlatformBaseFacade()
            : base(new CrossPlatformConnectionHandler(), false)
        {

        }

        protected CrossPlatformBaseFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {

        }

       

        public IConnectionHandler FileManagerConnection
        {
            get
            {
                var fileMngConnectionHandler = new FileManagerConnectionHandler();
                return fileMngConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && fileMngConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : fileMngConnectionHandler;
            }
        }
    }

    public class CrossPlatformConnectionHandler : ConnectionHandler
    {
        public CrossPlatformConnectionHandler()
        {
           
            base.ConnectionString = ConfigurationManager.ConnectionStrings["CrossPlatformEntities"].ConnectionString;
        }

    }
}
