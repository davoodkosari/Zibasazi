using System;
using Radyn.Congress.Facade;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;


namespace Radyn.Congress
{
    public class BaseInfoComponents
    {
        internal BaseInfoComponents()
        {

        }
        public IHomaFacade HomaFacade
        {
            get { return new HomaFacade(); }
        }
        public ICongressMenuHtmlFacade CongressMenuHtmlFacade
        {
            get { return new CongressMenuHtmlFacade(); }
        }

      
        public IPivotCategoryFacade PivotCategoryFacade
        {
            get { return new PivotCategoryFacade(); }
        }

        public IArticleUserCommentFacade ArticleUserCommentFacade
        {
            get { return new ArticleUserCommentFacade(); }
        }

        public IHomaAliasFacade HomaAliasFacade
        {
            get { return new HomaAliasFacade(); }
        }
        public IArticleAuthorsFacade ArticleAuthorsFacade
        {
            get { return new ArticleAuthorsFacade(); }
        }
        public IBoothOfficerFacade BoothOfficerFacade
        {
            get { return new BoothOfficerFacade(); }
        }

        public IUserForms UserFormsFacade
        {
            get { return new UserFormsFacade(); }
        }
        public ISupportTypeFacade SupportTypeFacade
        {
            get { return new SupportTypeFacade(); }
        }
        public ISecurityUserFacade SecurityUserFacade
        {
            get { return new SecurityUserFacade(); }
        }
        public IVIPFacade VipFacade
        {
            get { return new VIPFacade(); }
        }
        public ISupporterFacade SupporterFacade
        {
            get { return new SupporterFacade(); }
        }
        public IBoothFacade BoothFacade
        {
            get { return new BoothFacade(); }
        }
        public IResourceFacade ResourceFacade
        {
            get { return new ResourceFacade(); }
        }
        public IBoothCatgoryFacade BoothCatgoryFacade
        {
            get { return new BoothCatgoryFacade(); }
        }
        public ITeacherFacade TeacherFacade
        {
            get { return new TeacherFacade(); }
        }
        public IWorkShopFacade WorkShopFacade
        {
            get { return new WorkShopFacade(); }
        }
        public IRefereeFacade RefereeFacade
        {
            get { return new RefereeFacade(); }
        }
        public INewsLetterFacade NewsLetterFacade
        {
            get { return new NewsLetterFacade(); }
        }
        public IUserFacade UserFacade
        {
            get { return new UserFacade(); }
        }
        public IArticleTypeFacade ArticleTypeFacade
        {
            get { return new ArticleTypeFacade(); }
        }
        public IPivotFacade PivotFacade
        {
            get { return new PivotFacade(); }
        }

        public ICustomMessageFacade CustomMessageFacade
        {
            get { return new CustomMessageFacade(); }
        }

        public IArticleFacade ArticleFacade
        {
            get { return new ArticleFacade(); }
        }
        public IHotelFacade HotelFacade
        {
            get { return new HotelFacade(); }
        }
        public IHotelUserFacade HotelUserFacade
        {
            get { return new HotelUserFacade(); }
        }
        public IWorkShopUserFacade WorkShopUserFacade
        {
            get { return new WorkShopUserFacade(); }
        }
        public IArticleFlowFacade ArticleFlowFacade
        {
            get { return new ArticleFlowFacade(); }
        }
        public ICongressSlideFacade CongessSlideFacade
        {
            get { return new CongressSlideFacade(); }
        }
        public IRefereeCartableFacade RefereeCartableFacade
        {
            get { return new RefereeCartableFacade(); }
        }
        public IConfigurationFacade ConfigurationFacade
        {
            get { return new ConfigurationFacade(); }
        }

        

        public IWorkShopTeacherFacade WorkShopTeacherFacade
        {
            get { return new WorkShopTeacherFacade(); }
        }
        public IUserRegisterPaymentTypeFacade UserRegisterPaymentTypeFacade
        {
            get { return new UserRegisterPaymentTypeFacade(); }
        }
        public IConfigurationSupportTypeFacade ConfigurationSupportTypeFacade
        {
            get { return new ConfigurationSupportTypeFacade(); }
        }
        public ICongressNewsFacade CongressNewsFacade
        {
            get { return new CongressNewsFacade(); }
        }

        public ICongessGalleryFacade CongessGalleryFacade
        {
            get { return new CongessGalleryFacade(); }
        }
        public ICongressContentFacade CongressContentFacade
        {
            get { return new CongressContentFacade(); }
        }
        public ICongressMenuFacade CongressMenuFacade
        {
            get { return new CongressMenuFacade(); }
        }
        public ICongressHtmlFacade CongressHtmlFacade
        {
            get { return new CongressHtmlFacade(); }
        }
        public ICongressContainerFacade CongressContainerFacade
        {
            get { return new CongressContainerFacade(); }
        }
        public ICongressHallFacade CongressHallFacade
        {
            get { return new CongressHallFacade(); }
        }
        public IConfigurationContentFacade ConfigurationContentFacade
        {
            get { return new ConfigurationContentFacade(); }
        }
        public ICongressFoldersFacade CongressFoldersFacade
        {
            get { return new CongressFoldersFacade(); }
        }
        public IArticlePaymentTypeFacade ArticlePaymentTypeFacade
        {
            get { return new ArticlePaymentTypeFacade(); }
        }
        public ICongressFormsFacade CongressFormsFacade
        {
            get { return new CongressFormsFacade(); }
        }
        public ICongressLanguageFacade CongressLanguageFacade
        {
            get { return new CongressLanguageFacade(); }
        }


        public ICongressLanguageFacade CongressLanguageTransactionalFacade(IConnectionHandler connectionHandler)
        {
            return new CongressLanguageFacade(connectionHandler);
        }

        public IUserForms UserFormsTransactionalFacade(IConnectionHandler connectionHandler)
        {
            return new UserFormsFacade(connectionHandler);
        }


        public ICongressDefinitionFacade CongressDefinitionFacade
        {
            get { return new CongressDefinitionFacade(); }
        }
        public ICongressDiscountTypeFacade CongressDiscountTypeFacade
        {
            get { return new CongressDiscountTypeFacade(); }
        }
        public ICongressAccountFacade CongressAccountFacade
        {
            get { return new CongressAccountFacade(); }
        }
        public ICongressFAQFacade CongressFaqFacade
        {
            get { return new CongressFAQFacade(); }
        }
        public IUserBoothFacade UserBoothFacade
        {
            get { return new UserBoothFacade(); }
        }
        public IRefereePivotFacade RefereePivotFacade
        {
            get { return new RefereePivotFacade(); }
        }
        public IUserFileFacade UserFileFacade
        {
            get { return new UserFileFacade(); }
        }
        public IGroupRegisterDiscountFacade GroupRegisterDiscountFacade
        {
            get { return new GroupRegisterDiscountFacade(); }
        }
        public IChipsFoodFacade ChipsFoodFacade
        {
            get { return new ChipsFoodFacade(); }
        }
        public IChipsFoodUserFacade ChipsFoodUserFacade
        {
            get { return new ChipsFoodUserFacade(); }
        }
        public ICongressTypeFacade CongressTypeFacade
        {
            get { return new CongressTypeFacade(); }
        }

       
    }
}
