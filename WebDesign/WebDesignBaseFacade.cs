using System.Configuration;
using Radyn.Common;
using Radyn.ContentManager;
using Radyn.FAQ;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Gallery;
using Radyn.News;
using Radyn.Slider;
using Radyn.Statistics;

namespace Radyn.WebDesign
{

    public abstract class WebDesignBaseFacade<T> : BaseFacade<T> where T : class
    {
        protected WebDesignBaseFacade()
            : base(new SecurityConnectionHandler(), false)
        {

        }

        protected WebDesignBaseFacade(IConnectionHandler connectionHandler)
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
       
    }


    public class SecurityConnectionHandler : ConnectionHandler
    {

        public SecurityConnectionHandler()
        {
           base.ConnectionString = ConfigurationManager.ConnectionStrings["WebDesignEntities"].ConnectionString;
        }

    }


}
