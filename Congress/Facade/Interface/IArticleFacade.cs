using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using static Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.Facade.Interface
{
    public interface IArticleFacade : IBaseFacade<Article>
    {
        bool UserInsert(Article article, List<ArticleAuthors> articleAuthorses,  HttpPostedFileBase abstractFileId, HttpPostedFileBase orginalFileId, FormStructure formModel);
        bool UserInsert(ref Article article, List<ArticleAuthors> articleAuthorses, HttpPostedFileBase abstractFileId, HttpPostedFileBase orginalFileId, List<DiscountType> transactionDiscountAttaches, string updatestatusaftertransactionIdurl, FormStructure formModel);
        bool AdminUpdate(Article obj);
        bool AdminUpdateArticles(Guid congressId,List<Article> model);
        bool AdminUpdate(Guid adminId, Article article, List<ArticleAuthors> articleAuthorses,  string comment, HttpPostedFileBase flowFile, HttpPostedFileBase orginalFileId, HttpPostedFileBase abstractFileId, FormStructure formModel);
        bool UserUpdate(Article article, List<ArticleAuthors> articleAuthorses,  HttpPostedFileBase abstractFileId, HttpPostedFileBase orginalFileId, FormStructure formModel);
       
       
       
        bool SentArticle(Article article, HttpPostedFileBase orginalFileId, FormStructure formModel);
        bool ArticlePaymnet(Article article, List<DiscountType> transactionDiscountAttaches, string callbackurl);
        Guid UpdateStatusAfterTransaction(Guid congressId,Guid id);
        List<Article> Search(Guid congressId, Article article, string serachvalue, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow);
        List<dynamic> SearchDynamic(Guid congressId, Article article, string serachvalue, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow);
        IEnumerable<ModelView.UserArticleAbstract> SearchArticle(Guid congressId, Article article, string serachvalue, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow);

        List<Tools.ModelView.ArticleCertificateModel> GetArticleCertificate(Homa homa, Guid id, bool isadmin = false);
        List<Tools.ModelView.ArticleCertificateModel> GetAllArticleCertificate(Homa homa, Article article, string text, FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow);
        IEnumerable<Article> GetByUserId(Configuration configuration, Guid id);
        
        IEnumerable<Article> GetAllForZipFile(Guid congressId, Article article, string serachvalue,
            FormStructure formStructure, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow);
        bool UpdateStatus(Guid congressId, List<Guid> guids, FinalState status);
    }
}
