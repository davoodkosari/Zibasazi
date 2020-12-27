using System.Configuration;
using Radyn.Common;
using Radyn.ContentManager;

using Radyn.EnterpriseNode;
using Radyn.FAQ;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Gallery;
using Radyn.Message;
using Radyn.News;
using Radyn.Payment;
using Radyn.Reservation;
using Radyn.Security;
using Radyn.Slider;
using Radyn.Statistics;

namespace Radyn.Congress
{
    public abstract class CongressBaseFacade<T> : BaseFacade<T> where T : class
    {
        protected CongressBaseFacade()
            : base(new CongressConnectionHandler(), false)
        {

        }

        protected CongressBaseFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {

        }

        public IConnectionHandler FileManagerConnection
        {
            get
            {
                var fileManagerConnection = new FileManagerConnectionHandler();
                return fileManagerConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && fileManagerConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : fileManagerConnection;
            }
        }
        public IConnectionHandler PaymentConnection
        {
            get
            {
                var paymentConnection = new PaymentConnectionHandler();
                return paymentConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && paymentConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : paymentConnection;
            }
        }
        public IConnectionHandler EnterpriseNodeConnection
        {
            get
            {
                var enterpriseNodeConnection = new EnterpriseNodeConnectionHandler();
                return enterpriseNodeConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && enterpriseNodeConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : enterpriseNodeConnection;
            }
        }
        public IConnectionHandler ContentManagerConnection
        {
            get
            {
                var contentManagerConnection = new ContentManagerConnectionHandler();
                return contentManagerConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && contentManagerConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : contentManagerConnection;
            }
        }
        public IConnectionHandler NewsConnection
        {
            get
            {
                var newsConnectionHandler = new NewsConnectionHandler();
                return newsConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && newsConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : newsConnectionHandler;
            }
        }
        public IConnectionHandler GalleryConnection
        {
            get
            {
                var galleryConnectionHandler = new GalleryConnectionHandler();
                return galleryConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && galleryConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : galleryConnectionHandler;
            }
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
        public IConnectionHandler SliderConnection
        {
            get
            {
                var sliderConnectionHandler = new SliderConnectionHandler();
                return sliderConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && sliderConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : sliderConnectionHandler;
            }
        }
        public IConnectionHandler SecurityConnection
        {
            get
            {
                var securityConnectionHandler = new SecurityConnectionHandler();
                return securityConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && securityConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : securityConnectionHandler;
            }
        }
        public IConnectionHandler FormGeneratorConnection
        {
            get
            {
                var formGeneratorrConnection = new FormGeneratorConnectionHandler();
                return formGeneratorrConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && formGeneratorrConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : formGeneratorrConnection;
            }
        }

        public IConnectionHandler FaqConnection
        {
            get
            {
                var faqConnectionHandler = new FAQConnectionHandler();
                return faqConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && faqConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : faqConnectionHandler;
            }
        }
        public IConnectionHandler StatisticConnection
        {
            get
            {
                var statisticConnection = new StatisticConnectionHandler();
                return statisticConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && statisticConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : statisticConnection;
            }
        }
        public IConnectionHandler ReservationConnection
        {
            get
            {
                var reservationConnection = new ReservationConnectionHandler();
                return reservationConnection.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && reservationConnection.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : reservationConnection;
            }
        }
        public IConnectionHandler MessageConnection
        {
            get
            {
                var messageConnectionHandler = new MessageConnectionHandler();
                return messageConnectionHandler.Connection.DataSource == this.ConnectionHandler.Connection.DataSource
                     && messageConnectionHandler.Connection.Database == this.ConnectionHandler.Connection.Database
                    ? this.ConnectionHandler
                    : messageConnectionHandler;
            }
        }
       
    }

    public class CongressConnectionHandler : ConnectionHandler
    {

        public CongressConnectionHandler()
        {
            base.ConnectionString = ConfigurationManager.ConnectionStrings["CongressEntities"].ConnectionString; 
        }

    }
}
