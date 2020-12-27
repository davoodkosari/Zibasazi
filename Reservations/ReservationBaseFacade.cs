using System.Configuration;
using Radyn.Common;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Reservation
{

    public abstract class ReservationBaseFacade<T> : BaseFacade<T> where T : class
    {
        protected ReservationBaseFacade()
            : base(new ReservationConnectionHandler(), false)
        {

        }

        protected ReservationBaseFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {

        }

        public IConnectionHandler CommonConnection
        {
            get
            {
                var commonConnectionHandler = new CommonConnectionHandler();
                return commonConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && commonConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : commonConnectionHandler;
            }
        }
        public IConnectionHandler FileManagerConnection
        {
            get
            {
                var fileManagerConnectionHandler = new FileManagerConnectionHandler();
                return fileManagerConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && fileManagerConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : fileManagerConnectionHandler;
            }
        }
       
    }


    public class ReservationConnectionHandler : ConnectionHandler
    {

        public ReservationConnectionHandler()
        {
            base.ConnectionString = ConfigurationManager.ConnectionStrings["ReservationEntities"].ConnectionString; 
        }

    }


}
