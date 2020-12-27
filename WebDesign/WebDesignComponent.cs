

using Radyn.WebDesign.Facade;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign
{
    public class WebDesignComponent
    {

        private WebDesignComponent()
        {

        }
        private static WebDesignComponent _instance;
        public static WebDesignComponent Instance
        {
            get { return _instance ?? (_instance = new WebDesignComponent()); }
        }
        public IConfigurationFacade ConfigurationFacade
        {
            get { return new ConfigurationFacade(); }
        }public IWebSiteAliasFacade WebSiteAliasFacade
        {
            get { return new WebSiteAliasFacade(); }
        }
        public IWebSiteFacade WebSiteFacade
        {
            get { return new WebSiteFacade(); }
        }
        public IResourceFacade ResourceFacade
        {
            get { return new ResourceFacade(); }
        }
        public IHtmlFacade HtmlFacade
        {
            get { return new HtmlFacade(); }
        }
      
        public IContainerFacade ContainerFacade
        {
            get { return new ContainerFacade(); }
        }
        public IContentFacade ContentFacade
        {
            get { return new ContentFacade(); }
        }
        public IFAQFacade FaqFacade
        {
            get { return new FAQFacade(); }
        }
        public IGalleryFacade GalleryFacade
        {
            get { return new GalleryFacade(); }
        }
        public IFormsFacade FormsFacade
        {
            get { return new FormsFacade(); }
        }
        public IFolderFacade FolderFacade
        {
            get { return new FolderFacade(); }
        }
        public ILanguageFacade LanguageFacade
        {
            get { return new LanguageFacade(); }
        }
        public IMenuFacade MenuFacade
        {
            get { return new MenuFacade(); }
        }
        public IMenuHtmlFacade MenuHtmlFacade
        {
            get { return new CongressMenuHtmlFacade(); }
        }
        public INewsFacade NewsFacade
        {
            get { return new NewsFacade(); }
        }
        public ISliderFacade SliderFacade
        {
            get { return new SliderFacade(); }
        }



    }
}
