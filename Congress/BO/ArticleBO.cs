using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.FormGenerator.BO;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using static Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.BO
{
    internal class ArticleBO : BusinessBase<Article>
    {
        public override bool Delete(IConnectionHandler connectionHandler, Article obj)
        {
            var cartablebo = new RefereeCartableBO();
            var usercommentbo = new ArticleUserCommentBO();

            var cartble = cartablebo.Where(connectionHandler, x => x.ArticleId == obj.Id);
            foreach (var refereeCartable in cartble)
            {
                if (!cartablebo.Delete(connectionHandler, refereeCartable))
                    return false;
            }
            var articleFlowBo = new ArticleFlowBO();
            var @where = articleFlowBo.Where(connectionHandler, x => x.ArticleId == obj.Id);
            foreach (var refereeCartable in @where)
            {
                if (!articleFlowBo.Delete(connectionHandler, refereeCartable))
                    return false;
            }
            var comments = usercommentbo.Where(connectionHandler, x => x.ArticleId == obj.Id);
            foreach (var articleUserComment in comments)
            {
                if (!usercommentbo.Delete(connectionHandler, articleUserComment))
                    return false;
            }

            return base.Delete(connectionHandler, obj);
        }


        public List<Article> Search(IConnectionHandler connectionHandler, Guid congressId, Article article, string serachvalue, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow, FormStructure formStructure = null)
        {

            List<Article> outlist = new List<Article>();
            ArticleFlowBO articleFlowBo = new ArticleFlowBO();
            PredicateBuilder<Article> predicateBuilder = PredicateBuilder(connectionHandler, congressId, article, serachvalue, formStructure);
            switch (articleflow)
            {
                case Enums.SortAccordingToArticle.TitleArticle:
                    outlist = ascendingDescending == Enums.AscendingDescending.Descending
                        ? OrderByDescending(connectionHandler, x => x.Title, predicateBuilder.GetExpression())
                        : OrderBy(connectionHandler, x => x.Title, predicateBuilder.GetExpression());

                    break;

                case Enums.SortAccordingToArticle.DateOfSendingArticle:
                case Enums.SortAccordingToArticle.DateOfSendingAbstract:
                case Enums.SortAccordingToArticle.DateOfLastFlowChanges:
                    List<Guid> nothasflowidlist = new List<Guid>();
                    PredicateBuilder<ArticleFlow> builder = new PredicateBuilder<ArticleFlow>();
                    List<Guid> items = Select(connectionHandler, x => x.Id, predicateBuilder.GetExpression(), true);
                    if (items.Any())
                    {
                        List<Guid> guids1 = articleFlowBo.Select(connectionHandler, x => x.ArticleId, x => x.Article.CongressId == congressId, true);
                        if (guids1.Any())
                        {
                            nothasflowidlist = items.Where(x => x.NotIn(guids1)).ToList();
                        }

                        builder.And(x => x.ArticleId.In(items));
                    }
                    List<Guid> hasflowidlist = ascendingDescending == Enums.AscendingDescending.Descending
                        ? articleFlowBo.Select(connectionHandler, x => x.ArticleId, builder.GetExpression(),
                            new OrderByModel<ArticleFlow>()
                            {
                                Expression = x => x.SaveDate + "" + x.SaveTime,
                                OrderType = OrderType.DESC
                            })
                        : articleFlowBo.Select(connectionHandler, x => x.ArticleId, builder.GetExpression(),
                            new OrderByModel<ArticleFlow>() { Expression = x => x.SaveDate + "" + x.SaveTime });




                    List<Guid> guids = new List<Guid>();
                    List<Guid> enumerable = hasflowidlist.Distinct().ToList();
                    foreach (Guid guid in enumerable)
                    {
                        if (guids.Contains(guid))
                        {
                            continue;
                        }

                        guids.Add(guid);
                    }
                    foreach (Guid item in nothasflowidlist)
                    {
                        if (guids.Contains(item))
                        {
                            continue;
                        }

                        guids.Add(item);
                    }

                    outlist = Where(connectionHandler, x => x.Id.In(guids));
                    break;
                default:
                    outlist = ascendingDescending == Enums.AscendingDescending.Descending
                       ? OrderByDescending(connectionHandler, x => x.Code, predicateBuilder.GetExpression())
                       : OrderBy(connectionHandler, x => x.Code, predicateBuilder.GetExpression());
                    break;


            }

            return outlist;






        }




        public List<dynamic> SearchDynamic(IConnectionHandler connectionHandler, Guid congressId, Article article, string serachvalue, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToArticle articleflow, FormStructure formStructure = null)
        {
            List<dynamic> outlist = new List<dynamic>();
            ArticleFlowBO articleFlowBo = new ArticleFlowBO();
            PredicateBuilder<Article> predicateBuilder = PredicateBuilder(connectionHandler, congressId, article, serachvalue, formStructure);
            Expression<Func<Article, object>>[] columns = new Expression<Func<Article, object>>[]
            {
                x =>x.Id,
                x =>x.Title,
                x =>x.Code,
                x =>x.Status,
                x =>x.TypeId,
                x =>x.FinalState,
                x =>x.PayStatus,
                x =>x.TransactionId,
                x =>x.OrginalFileId,
                x =>x.AbstractFileId,
                x=>x.ArticleOrginalText,
                x =>x.UserId,
                x =>x.Abstract,
                x => x.User.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.User.EnterpriseNode.RealEnterpriseNode.LastName,
            };
            switch (articleflow)
            {
                case Enums.SortAccordingToArticle.TitleArticle:
                    outlist = ascendingDescending == Enums.AscendingDescending.Descending
                        ? Select(connectionHandler, columns, predicateBuilder.GetExpression(), new OrderByModel<Article>() { Expression = x => x.Title, OrderType = OrderType.DESC })
                        : Select(connectionHandler, columns, predicateBuilder.GetExpression(), new OrderByModel<Article>() { Expression = x => x.Title });
                    break;
                case Enums.SortAccordingToArticle.DateOfSendingArticle:
                case Enums.SortAccordingToArticle.DateOfSendingAbstract:
                case Enums.SortAccordingToArticle.DateOfLastFlowChanges:
                    List<Guid> nothasflowidlist = new List<Guid>();

                    PredicateBuilder<ArticleFlow> builder = new PredicateBuilder<ArticleFlow>();
                    List<Guid> items = Select(connectionHandler, x => x.Id, predicateBuilder.GetExpression(), true);
                    if (items.Any())
                    {
                        List<Guid> guids1 = articleFlowBo.Select(connectionHandler, x => x.ArticleId, x => x.Article.CongressId == congressId, true);
                        if (guids1.Any())
                        {
                            nothasflowidlist = items.Where(x => x.NotIn(guids1)).ToList();
                        }

                        builder.And(x => x.ArticleId.In(items));
                    }
                    List<Guid> hasflowidlist = ascendingDescending == Enums.AscendingDescending.Descending
                        ? articleFlowBo.Select(connectionHandler, x => x.ArticleId, builder.GetExpression(),
                            new OrderByModel<ArticleFlow>()
                            {
                                Expression = x => x.SaveDate + "" + x.SaveTime,
                                OrderType = OrderType.DESC
                            })
                        : articleFlowBo.Select(connectionHandler, x => x.ArticleId, builder.GetExpression(),
                            new OrderByModel<ArticleFlow>() { Expression = x => x.SaveDate + "" + x.SaveTime });




                    Dictionary<Guid, int> guids = new Dictionary<Guid, int>();
                    int index = 0;
                    List<Guid> enumerable = hasflowidlist.Distinct().ToList();
                    foreach (Guid guid in enumerable)
                    {
                        if (guids.ContainsKey(guid))
                        {
                            continue;
                        }

                        guids.Add(guid, index);
                        index++;
                    }
                    foreach (Guid item in nothasflowidlist)
                    {
                        if (guids.ContainsKey(item))
                        {
                            continue;
                        }

                        guids.Add(item, index);
                        index++;
                    }
                    IEnumerable<Guid> @select = guids.OrderBy(x => x.Value).Select(x => x.Key);
                    outlist = Select(connectionHandler, columns, x => x.Id.In(@select));
                    break;
                default:
                    outlist = ascendingDescending == Enums.AscendingDescending.Descending
                        ? Select(connectionHandler, columns, predicateBuilder.GetExpression(), new OrderByModel<Article>() { Expression = x => x.Code, OrderType = OrderType.DESC })
                        : Select(connectionHandler, columns, predicateBuilder.GetExpression(), new OrderByModel<Article>() { Expression = x => x.Code });
                    break;


            }

            return outlist;






        }





        private static PredicateBuilder<Article> PredicateBuilder(IConnectionHandler connectionHandler, Guid congressId, Article article,
            string serachvalue, FormStructure formStructure)
        {

            PredicateBuilder<Article> predicateBuilder = new PredicateBuilder<Article>();
            predicateBuilder.And(x => x.CongressId == congressId);
            predicateBuilder.And(x => (x.IsArchive == null || x.IsArchive == false));
            if (!string.IsNullOrEmpty(serachvalue))
            {
                predicateBuilder.And(
                    (x =>
                        x.Title.Contains(serachvalue) || x.Code.ToString().Contains(serachvalue) ||
                        x.Keyword.Contains(serachvalue) || x.User.Username.Contains(serachvalue) ||
                        x.User.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(serachvalue) ||
                        x.User.EnterpriseNode.RealEnterpriseNode.LastName.Contains(serachvalue)
                        || x.User.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(serachvalue) ||
                        x.User.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(serachvalue) ||
                        x.User.EnterpriseNode.Address.Contains(serachvalue)
                        || x.User.EnterpriseNode.Website.Contains(serachvalue) ||
                        x.User.EnterpriseNode.Email.Contains(serachvalue) || x.User.EnterpriseNode.Tel.Contains(serachvalue)));

                List<Guid> @select = new ArticleAuthorsBO().Select(connectionHandler, x => x.ArticleId,
                    x => x.Name.Contains(serachvalue) || x.Address.Contains(serachvalue), true);
                if (@select.Any())
                {
                    predicateBuilder.And(x => x.Id.In(@select));
                }
            }
            if (formStructure != null)
            {
                IEnumerable<string> @where = FormGeneratorComponent.Instance.FormDataFacade.Search(formStructure);
                if (@where.Any())
                {
                    predicateBuilder.And(x => x.Id.In(@where.Select(s => s.ToGuid())));
                }
            }
            if (!string.IsNullOrEmpty(article.RefreeTitle))
            {
                List<Guid> refree = new RefereeCartableBO().Select(connectionHandler, x => x.ArticleId, x => (x.Referee.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.Referee.EnterpriseNode.RealEnterpriseNode.LastName).Contains(article.RefreeTitle));

                if (refree.Any())
                {
                    predicateBuilder.And(x => x.Id.In(refree));
                }

            }
            if (article.StateNullable.HasValue && article.PayStatus != 0)
            {
                predicateBuilder.And(x => x.Status == article.StateNullable);
            }

            if (article.FinalStateNullable.HasValue && article.PayStatus != 0)
            {
                predicateBuilder.And(x => x.FinalState == article.FinalStateNullable);
            }

            if (article.PivotId != Guid.Empty)
            {
                predicateBuilder.And(x => x.PivotId == article.PivotId);
            }

            if (article.Pivot.PivotCategoryId != Guid.Empty)
            {
                predicateBuilder.And(x => x.Pivot.PivotCategoryId == article.Pivot.PivotCategoryId);
            }

            if (article.TypeId.HasValue && article.PayStatus != 0)
            {
                predicateBuilder.And(x => x.TypeId == article.TypeId);
            }

            if (article.PayStatus.HasValue && article.PayStatus != 0)
            {
                predicateBuilder.And(x => x.PayStatus == article.PayStatus);
            }

            return predicateBuilder;
        }

        public override bool Insert(IConnectionHandler connectionHandler, Article obj)
        {
            Guid id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (obj.PublishDate == null)
            {
                obj.PublishDate = DateTime.Now.ShamsiDate();
            }

            if (obj.PayStatus == 0)
            {
                obj.PayStatus = (byte)Enums.ArticlepayState.PayWait;
            }

            return base.Insert(connectionHandler, obj);
        }

        public IEnumerable<ModelView.ReportChartModel> ChartArticleStatusCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.Status },
                new GroupByModel<Article>[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.Status,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId);
            IEnumerable<KeyValuePair<byte, string>> enums = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.ArticleState>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.ArticleState>(),
                        keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in enums)
            {
                dynamic first = list.FirstOrDefault(x => (x.Status is byte) && (byte)x.Status == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountStatus ?? 0,
                    Value = ((Enums.ArticleState)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartArticleMothCount(IConnectionHandler connectionHandler, Guid congressId, string year)
        {

            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.PublishDate.Substring(5, 2) },
                new GroupByModel<Article>[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.PublishDate.Substring(5, 2),
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId && x.PublishDate.Substring(0, 4) == year);
            IEnumerable<KeyValuePair<byte, string>> months = EnumUtils.ConvertEnumToIEnumerableInLocalization<Common.Definition.Enums.PersianMonth>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Common.Definition.Enums.PersianMonth>(),
                        keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in months)
            {
                dynamic first = list.FirstOrDefault(x => (x.PublishDate is string) && ((string)x.PublishDate).ToByte() == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountPublishDate ?? 0,
                    Value = ((Common.Definition.Enums.PersianMonth)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartArticleDayCount(IConnectionHandler connectionHandler, Guid congressId, string moth, string year)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.PublishDate.Substring(8, 2) },
                new[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.PublishDate.Substring(8, 2),
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId && x.PublishDate.Substring(5, 2) == moth && x.PublishDate.Substring(0, 4) == year);

            for (int x = 1; x <= (moth.CompareTo("06") <= 0 ? 31 : 30); x++)
            {
                string number = x > 10 ? x.ToString() : "0" + x;
                dynamic first = list.FirstOrDefault(i => (i.PublishDate is string) && (string)i.PublishDate == number);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountPublishDate ?? 0,
                    Value = number
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartArticlePivotCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.Pivot.Title },
                new GroupByModel<Article>[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.PivotId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId);
            List<dynamic> allType = new PivotBO().Select(connectionHandler, new Expression<Func<Pivot, object>>[]
            {
                x => x.Title,
             }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Title is string) && (string)x.Title == (string)item.Title);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountPivotId ?? 0,
                    Value = (string)item.Title
                });
            }
            return listout;
        }

        public IEnumerable<ModelView.ReportChartModel> ChartArticlePivotCategoryCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.Pivot.PivotCategory.Title },
                new GroupByModel<Article>[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.Pivot.PivotCategoryId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId);
            List<dynamic> allType = new PivotCategoryBO().Select(connectionHandler, new Expression<Func<PivotCategory, object>>[]
            {
                x => x.Title,
             }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Title is string) && (string)x.Title == (string)item.Title);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountPivotCategoryId ?? 0,
                    Value = (string)item.Title
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartArticleTypeCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<Article, object>>[] { x => x.ArticleType.Title },
                new GroupByModel<Article>[]
                {
                    new GroupByModel<Article>()
                    {
                        Expression = x => x.TypeId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId);
            List<dynamic> allType = new ArticleTypeBO().Select(connectionHandler, new Expression<Func<ArticleType, object>>[]
            {
                x => x.Title,
             }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Title is string) && (string)x.Title == (string)item.Title);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountTypeId ?? 0,
                    Value = (string)item.Title
                });
            }
            return listout;
        }



        public bool SendArticleForAdmin(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, Article article)
        {
            Guid lastSenderId = new ArticleFlowBO().SelectFirstOrDefault(connectionHandler, x => x.SenderId,
                x => x.ReceiverId == article.UserId && x.ArticleId == article.Id,
                new OrderByModel<ArticleFlow>()
                {
                    Expression = x => x.SaveDate + "" + x.SaveTime,
                    OrderType = OrderType.DESC
                });
            if (!new ArticleFlowBO().AddFlow(connectionHandler, filemanagerconnection, article.UserId,
                lastSenderId != Guid.Empty ? lastSenderId : (Guid?)null, article.Id, null,
                article.Status.ToString().ToEnum<Enums.ArticleState>().GetDescriptionInLocalization()))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInSendArticle, Extention.GetAtricleTitle(article.CongressId)));
            }

            return true;
        }
        public List<ModelView.ArticleCertificateModel> GetArticleCertificate(IConnectionHandler connectionHandler, Guid id, Homa homa, bool isadmin)
        {
            List<ModelView.ArticleCertificateModel> list = new List<Tools.ModelView.ArticleCertificateModel>();
            if (homa == null || homa.Configuration == null)
            {
                return list;
            }

            List<ArticleAuthors> authorses = new ArticleAuthorsBO().Where(connectionHandler, x => x.ArticleId == id);
            Article article = Get(connectionHandler, id);
            bool hasarticletype = article.TypeId != null || (!homa.Configuration.RequireArticleTypeForCertificate);
            if (!hasarticletype)
            {
                return list;
            }

            if (isadmin)
            {
                GetReport(homa, authorses, article, article.ArticleType, ref list);
            }
            else if (homa.Configuration.ArticleCertificateState == null || article.FinalState == homa.Configuration.ArticleCertificateState)
            {
                GetReport(homa, authorses, article, article.ArticleType, ref list);
            }

            return list;
        }

        private static void GetReport(Homa homa, List<ArticleAuthors> authorses, Article article, ArticleType articleType,
           ref List<ModelView.ArticleCertificateModel> list)
        {
            Configuration config = homa.Configuration;
            switch (config.ArticleCertificateTypeId)
            {
                case (byte)Enums.ArticleCertificateType.ForeachCertificate:
                    {
                        foreach (ArticleAuthors articleAuthorse in authorses)
                        {
                            ModelView.ArticleCertificateModel articleCertificateModel = new Tools.ModelView.ArticleCertificateModel
                            {
                                Id = article.Id,
                                ArticleCode = article.Code.ToString(),
                                CongressTitle = homa.CongressTitle,
                                ArtileTypeTitle = articleType != null ? articleType.Title : "",
                                ArticleTitle = article.Title,
                                AuthorDirector = articleAuthorse.Percentage != 0 ? $"{articleAuthorse.Name}  {Resources.Congress.Withpercentage}  {articleAuthorse.Percentage}" : articleAuthorse.Name
                            };
                            list.Add(articleCertificateModel);
                        }
                    }

                    break;
                case (byte)Enums.ArticleCertificateType.AllsingleCertificate:
                    {
                        ModelView.ArticleCertificateModel articleCertificateModel = new Tools.ModelView.ArticleCertificateModel
                        {
                            Id = article.Id,
                            ArticleCode = article.Code.ToString(),
                            CongressTitle = homa.CongressTitle,
                            ArtileTypeTitle = articleType != null ? articleType.Title : "",
                            ArticleTitle = article.Title,
                            Author = authorses.Aggregate("", (current, articleAuthorse) => current + (articleAuthorse.Percentage != 0 ? $"{articleAuthorse.Name} {Resources.Congress.Withpercentage} {articleAuthorse.Percentage}" + " ," : $"{articleAuthorse.Name}" + " ,"))
                        };
                        ArticleAuthors firstOrDefault = authorses.FirstOrDefault(x => x.IsDirector);
                        if (firstOrDefault != null)
                        {
                            articleCertificateModel.AuthorDirector = firstOrDefault.Name;
                        }

                        list.Add(articleCertificateModel);
                    }
                    break;
            }
        }

        public bool UserInsert(IConnectionHandler connectionHandler, IConnectionHandler filemanagerConnection, IConnectionHandler formgeneratorconnection, Article article, List<ArticleAuthors> articleAuthorses, HttpPostedFileBase abstractFileId,
           HttpPostedFileBase orginalFileId, FormStructure formModel)
        {
            if (article == null)
            {
                return false;
            }

            FileManager.Facade.Interface.IFileFacade fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerConnection);
            if (abstractFileId != null)
            {
                article.AbstractFileId = fileTransactionalFacade.Insert(abstractFileId);
            }

            if (orginalFileId != null)
            {
                article.OrginalFileId = fileTransactionalFacade.Insert(orginalFileId);
            }

            SetStatus(article);
            if (!Insert(connectionHandler, article))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticle, Extention.GetAtricleTitle(article.CongressId)));
            }

            formModel.RefId = article.Id.ToString();
            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticle, Extention.GetAtricleTitle(article.CongressId)));
            }

            if (new ConfigurationBO().SelectFirstOrDefault(connectionHandler, x => x.AllowUserAddAuthor,
                x => x.CongressId == article.CongressId))
            {
                if (articleAuthorses == null || articleAuthorses.Count == 0)
                {
                    throw new Exception(Resources.Congress.Pleaseenteraminimumorauthorforthearticle);
                }
                if (!articleAuthorses.Any(authors => authors.IsDirector))
                {
                    throw new Exception(Resources.Congress.ThisArticleNotHaveDirectorAuthor);
                }
                ArticleAuthorsBO articleAuthorsBo = new ArticleAuthorsBO();
                foreach (ArticleAuthors articleAuthorse in articleAuthorses)
                {
                    articleAuthorse.ArticleId = article.Id;
                    if (!articleAuthorsBo.Insert(connectionHandler, articleAuthorse))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticleAuthor, Extention.GetAtricleTitle(article.CongressId)));
                    }
                }
               
                

            }

            return true;

        }

        public void SetStatus(Article article)
        {
            bool hasAbstract = (!string.IsNullOrEmpty(article.Abstract) || article.AbstractFileId.HasValue);
            bool hasorginal = (!string.IsNullOrEmpty(article.ArticleOrginalText) || article.OrginalFileId.HasValue);
            if (hasorginal && hasAbstract)
            {
                article.Status = (byte)Enums.ArticleState.ArticleSended;
                return;
            }
            if (hasorginal)
            {
                article.Status = (byte)Enums.ArticleState.OrginalSended;
                return;
            }

            if (hasAbstract)
            {
                article.Status = (byte)Enums.ArticleState.AbstractSended;
            }
        }

        public void AdminSetStatus(Article obj)
        {
            switch ((Enums.FinalState)obj.FinalState)
            {

                case Enums.FinalState.AbstractConfirm:
                    obj.Status = (byte)Enums.ArticleState.AbstractConfirm;
                    break;
                case Enums.FinalState.Confirm:
                    obj.Status = (byte)Enums.ArticleState.Confirm;
                    break;
                case Enums.FinalState.Denial:
                    obj.Status = (byte)Enums.ArticleState.Denial;
                    break;
                case Enums.FinalState.DenialAbstarct:
                    obj.Status = (byte)Enums.ArticleState.DenialAbstarct;
                    break;
                case Enums.FinalState.ConfirmandEdit:
                    obj.Status = (byte)Enums.ArticleState.ConfirmandEdit;
                    break;




            }

        }

        public bool UserUpdate(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, IConnectionHandler formgeneratorconnection, Article obj, List<ArticleAuthors> articleAuthorses, HttpPostedFileBase abstractFileId, HttpPostedFileBase orginalFileId, FormStructure formModel)
        {
            if (obj == null)
            {
                return false;
            }

            FileManager.Facade.Interface.IFileFacade fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerconnection);
            if (abstractFileId != null)
            {
                if (obj.AbstractFileId == null)
                {
                    obj.AbstractFileId = fileTransactionalFacade.Insert(abstractFileId);
                }
                else
                {
                    if (!fileTransactionalFacade.Update(abstractFileId, (Guid)obj.AbstractFileId))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleAbstractFile, Extention.GetAtricleTitle(obj.CongressId)));
                    }
                }
            }
            if (orginalFileId != null)
            {
                if (obj.OrginalFileId == null)
                {
                    obj.OrginalFileId = fileTransactionalFacade.Insert(orginalFileId);
                }
                else
                {
                    if (!fileTransactionalFacade.Update(orginalFileId, (Guid)obj.OrginalFileId))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleOrginalFile, Extention.GetAtricleTitle(obj.CongressId)));
                    }
                }
            }
            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticle, Extention.GetAtricleTitle(obj.CongressId)));
            }

            SetStatus(obj);
            return UpdateArticle(connectionHandler, obj, articleAuthorses);
        }


        public bool AdminUpdate(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, IConnectionHandler formgeneratorconnection, Guid adminId, Article obj, List<ArticleAuthors> articleAuthorses, HttpPostedFileBase orginalFileId, HttpPostedFileBase abstractFileId, string comment, HttpPostedFileBase flowFile,  FormStructure formModel)
        {
            obj.EditDate = DateTime.Now.ShamsiDate();
            obj.EditTime = DateTime.Now.GetTime();
            this.AdminSetStatus(obj);
            FileManager.Facade.Interface.IFileFacade fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerconnection);
            if (abstractFileId != null)
            {
                if (obj.AbstractFileId == null)
                {
                    obj.AbstractFileId = fileTransactionalFacade.Insert(abstractFileId);
                }
                else
                {
                    if (!fileTransactionalFacade.Update(abstractFileId, (Guid)obj.AbstractFileId))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleAbstractFile, Extention.GetAtricleTitle(obj.CongressId)));
                    }
                }
            }
            if (orginalFileId != null)
            {
                if (obj.OrginalFileId == null)
                {
                    obj.OrginalFileId = fileTransactionalFacade.Insert(orginalFileId);
                }
                else
                {
                    if (!fileTransactionalFacade.Update(orginalFileId, (Guid)obj.OrginalFileId))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleOrginalFile, Extention.GetAtricleTitle(obj.CongressId)));
                    }
                }
            }

            
            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticle, Extention.GetAtricleTitle(obj.CongressId)));
            }
            if (!string.IsNullOrEmpty(comment) || flowFile != null)
            {
                ArticleFlowBO articleFlowBo = new ArticleFlowBO();
                if (obj.SendForReferee)
                {
                    var cartables = new RefereeCartableBO().Where(connectionHandler, x => x.ArticleId == obj.Id);
                    foreach (var refereeCartable in cartables)
                    {
                        articleFlowBo.AddFlow(connectionHandler, filemanagerconnection, adminId, refereeCartable.RefereeId, obj.Id, null, comment, flowFile);
                    }
                }
                else
                    articleFlowBo.AddFlow(connectionHandler, filemanagerconnection, adminId, obj.UserId, obj.Id, null,
                        comment, flowFile);
            }
            return UpdateArticle(connectionHandler, obj, articleAuthorses);
           
        }

        protected override void CheckConstraint(IConnectionHandler connectionHandler, Article item)
        {
            base.CheckConstraint(connectionHandler, item);
            if(this.Any(connectionHandler,x=>x.Id!=item.Id&&x.Title==item.Title&&x.CongressId==item.CongressId))
                throw new KnownException("رکوردی با این عنوان ثبت شده است");
        }

        public bool UpdateArticle(IConnectionHandler connectionHandler,  Article obj, List<ArticleAuthors> articleAuthorses)
        {


            if (!UpdateArticleAuthors(connectionHandler, obj, articleAuthorses))
            {
                return false;
            }

            if (!Update(connectionHandler, obj))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleCongress, Extention.GetAtricleTitle(obj.CongressId)));
            }

            return true;
        }
        public bool ArticlePaymnet(IConnectionHandler connectionHandler, IConnectionHandler paymentconnection, ref Article article, List<DiscountType> transactionDiscountAttaches, string callbackurl)
        {
            if (article.PaymentTypeId == null)
            {
                return false;
            }

            User user = article.User;
            ArticlePaymentType articlePaymentType = new ArticlePaymentTypeBO().Get(connectionHandler, article.PaymentTypeId);
            Temp temp = new Temp();
            decimal calulateAmountNew = new CongressDiscountTypeBO().CalulateAmountNew(paymentconnection, articlePaymentType.ValidAmount.ToDecimal(), transactionDiscountAttaches);
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentconnection);
            string additionalData = new CongressDiscountTypeBO().FillTempAdditionalData(connectionHandler, article.CongressId);
            if (articlePaymentType.ValidAmount.ToDecimal() > 0)
            {

                if (article.TempId.HasValue)
                {
                    temp = PaymentComponenets.Instance.TempFacade.Get(article.TempId);
                    temp.Amount = calulateAmountNew;
                    temp.AdditionalData = additionalData;
                    temp.CurrencyType =
                        (byte)articlePaymentType.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    if (
                        !tempTransactionalFacade
                            .Update(temp, transactionDiscountAttaches))
                    {
                        return false;
                    }
                }
                else
                {

                    temp.PayerId = user.Id;
                    temp.CallBackUrl = callbackurl;
                    temp.PayerTitle = user.DescriptionField;
                    temp.Description = string.Format(Resources.Congress.PaymentArticlePayment, Extention.GetAtricleTitle(article.CongressId)) + " " + article.Title;
                    temp.Amount = calulateAmountNew;
                    temp.AdditionalData = additionalData;
                    temp.CurrencyType =
                        (byte)articlePaymentType.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    if (
                        !tempTransactionalFacade
                            .Insert(temp, transactionDiscountAttaches))
                    {
                        return false;
                    }

                    article.TempId = temp.Id;


                }
            }
            else
            {
                if (article.TempId.HasValue)
                {
                    if (
                        !tempTransactionalFacade
                            .Delete(article.TempId))
                    {
                        return false;
                    }
                }
                article.TempId = null;
            }
            if (article.TempId == null)
            {
                article.Status = (byte)Enums.ArticlepayState.PayConfirm;
            }

            if (!Update(connectionHandler, article))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleCongress, Extention.GetAtricleTitle(article.CongressId)));
            }

            return true;
        }

        public bool UpdateArticleAuthors(IConnectionHandler connectionHandler, Article article, List<ArticleAuthors> articleAuthorses)
        {
            ArticleAuthorsBO articleAuthorsBo = new ArticleAuthorsBO();
            if (articleAuthorses == null)
            {
                return true;
            }

            List<ArticleAuthors> oldConPart = articleAuthorsBo.Where(connectionHandler, c => c.ArticleId == article.Id);
            foreach (ArticleAuthors contractPart in articleAuthorses)
            {
                if (oldConPart.Any(x => x.Id == contractPart.Id))
                {
                    articleAuthorsBo.Update(connectionHandler, contractPart);
                }
                else
                {
                    contractPart.ArticleId = article.Id;
                    if (!articleAuthorsBo.Insert(connectionHandler, contractPart))
                    {
                        throw new Exception(string.Format(Resources.Congress.ErrorInSaveArticleAuthor, Extention.GetAtricleTitle(article.CongressId)));
                    }
                }
            }

            foreach (ArticleAuthors contractPart in oldConPart)
            {
                if (articleAuthorses.Any(x => x.Id == contractPart.Id))
                {
                    continue;
                }

                articleAuthorsBo.Delete(connectionHandler, contractPart.Id);
            }
            return true;
        }



        public ModelView.ModifyResult<Article> UpdateStatusAfterTransaction(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler fileManagerConnection, Guid id, ModelView.InFormEntitiyList<RefereeCartable> formEntitiys)
        {

            var result = new ModelView.ModifyResult<Article>();
            Article article = FirstOrDefault(connectionHandler, x => x.TempId == id);
            if (article == null)
            {
                return result;
            }

            Transaction tr = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection).RemoveTempAndReturnTransaction(id);
            if (tr == null)
            {
                return result;
            }

            article.TransactionId = tr.Id;
            article.Transaction = tr;
            article.TempId = null;
            if (tr.PreDone)
            {
                article.PayStatus = (byte)Enums.ArticlepayState.Pay;
                if (!SendArticle(connectionHandler, fileManagerConnection, article, formEntitiys))
                {
                    throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleCongress, Extention.GetAtricleTitle(article.CongressId)));
                }
                result.AddInform(article, Resources.Congress.ArticlePaymentEmail, Resources.Congress.ArticlePaymentSMS);

            }
            if (!Update(connectionHandler, article))
            {
                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleCongress, Extention.GetAtricleTitle(article.CongressId)));
            }
            result.TransactionId = tr.Id;
            result.SendInform = true;
            return result;
        }
        public void InformArticle(IConnectionHandler connectionHandler, Guid congressid, ModelView.InFormEntitiyList<Article> articles)
        {

            if (!articles.Any())
            {
                return;
            }
            Homa homa1 = new HomaBO().Get(connectionHandler, congressid);
            Configuration config = homa1.Configuration;
            if (config.ArticleInformType == null) return;
            var custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == homa1.Id && x.Type == MessageInformType.Article);
            var @where = this.Where(connectionHandler, x => x.Id.In(articles.Select(i => i.obj.Id)));
            foreach (var article in articles)
            {
                var firstOrDefault = @where.FirstOrDefault(x => x.Id == article.obj.Id);
                if (firstOrDefault == null) continue;
                var status = ((Enums.ArticleState)firstOrDefault.Status).GetDescriptionInLocalization();
                var homaCompleteUrl = homa1.GetHomaCompleteUrl();
                var name = firstOrDefault.User.EnterpriseNode.DescriptionFieldWithGender;

                string sms = string.Format(article.EmailBody, homa1.CongressTitle, name, firstOrDefault.Code, status);
                string email = string.Format(article.SmsBody, homa1.CongressTitle, name, firstOrDefault.Title, homaCompleteUrl, status);
                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{ArticleMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        email = email.Replace($"[{ArticleMessageKey.ArticleTitle.ToString()}]", firstOrDefault.Title);
                        email = email.Replace($"[{ArticleMessageKey.UsersName.ToString()}]", name);
                        email = email.Replace($"[{ArticleMessageKey.ArticleCode.ToString()}]", firstOrDefault.Code.ToString());
                        email = email.Replace($"[{ArticleMessageKey.ArticleStatus.ToString()}]", status);
                        email = email.Replace($"[{ArticleMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{ArticleMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        sms = sms.Replace($"[{ArticleMessageKey.ArticleTitle.ToString()}]", firstOrDefault.Title);
                        sms = sms.Replace($"[{ArticleMessageKey.UsersName.ToString()}]", name);
                        sms = sms.Replace($"[{ArticleMessageKey.ArticleCode.ToString()}]", firstOrDefault.Code.ToString());
                        sms = sms.Replace($"[{ArticleMessageKey.ArticleStatus.ToString()}]", status);
                        sms = sms.Replace($"[{ArticleMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                    }





                }



                Message.Tools.ModelView.MessageModel inform = new Message.Tools.ModelView.MessageModel()
                {
                    Email = firstOrDefault.User.EnterpriseNode.Email,
                    Mobile = firstOrDefault.User.EnterpriseNode.Cellphone,
                    EmailTitle = homa1.CongressTitle,
                    EmailBody = email,
                    SMSBody = sms
                };
                new HomaBO().SendInform((byte)config.ArticleInformType, inform, config, homa1.CongressTitle);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa1.OwnerId, config.CongressId,
                 new[] { firstOrDefault.User.EnterpriseNode.Id.ToString() }, homa1.CongressTitle, inform.SMSBody);
            }



        }

        public bool SendArticle(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, Article article, ModelView.InFormEntitiyList<RefereeCartable> keyValuePairs)
        {

            Configuration config = new ConfigurationBO().ValidConfig(connectionHandler, article.CongressId);
            if (config.AutoAssigneArticleToReferee)
            {
                RefereeCartableBO refereeCartableBo = new RefereeCartableBO();
                var @select = refereeCartableBo.Select(connectionHandler, x => x.RefereeId, x => x.ArticleId == article.Id,true);
                var refereeListForArticle =new RefereePivotBO().Select(connectionHandler, x => x.RefereeId, x => x.PivotId == article.PivotId, true);
                if (refereeListForArticle != null)
                    @select.AddRange(refereeListForArticle.Where(x=>x.NotIn(select)));
                if (!@select.Any())
                {
                    if (!SendArticleForAdmin(connectionHandler, filemanagerconnection, article))
                        throw new Exception(string.Format(Resources.Congress.ErrorInSendArticle,
                            Extention.GetAtricleTitle(article.CongressId)));
                }
                else
                {
                    
                    foreach (var guid1 in @select)
                    {
                        if (!refereeCartableBo.AssigneToCartable(connectionHandler, filemanagerconnection, article.UserId, guid1, article))
                        {
                            throw new Exception(string.Format(Resources.Congress.ErrorInSendArticle, Extention.GetAtricleTitle(article.CongressId)));
                        }
                        keyValuePairs.Add(new RefereeCartable() { ArticleId = article.Id, RefereeId = guid1 }, Resources.Congress.RefereeInformArticleEmail, Resources.Congress.RefereeInformArticleSMS);
                    }




                }

                if (config.SentArticleSpecialReferee)
                    article.Status = (byte)Enums.ArticleState.SentToSpecialReferee;
                else
                    article.Status = (byte)Enums.ArticleState.WaitForRefereeOpinion;

                if (!Update(connectionHandler, article))
                    throw new Exception(string.Format(Resources.Congress.ErrorInSendArticle,
                        Extention.GetAtricleTitle(article.CongressId)));
            }
            else
            {
                if (!SendArticleForAdmin(connectionHandler, filemanagerconnection, article))
                    throw new Exception(string.Format(Resources.Congress.ErrorInSendArticle,
                        Extention.GetAtricleTitle(article.CongressId)));
            }

            return true;
        }


        public decimal GetTransactionId(IConnectionHandler connectionHandler, Guid congressId, string year = "", string moth = "")
        {
            PredicateBuilder<Article> predicateBuilder = new PredicateBuilder<Article>();
            predicateBuilder.And(x => x.CongressId == congressId && x.TransactionId.HasValue && x.Transaction.Done);
            if (!string.IsNullOrEmpty(moth) && !string.IsNullOrEmpty(year))
            {
                predicateBuilder.And(
                    x => x.PublishDate.Substring(5, 2) == moth && x.PublishDate.Substring(0, 4) == year);
            }
            else if (!string.IsNullOrEmpty(year))
            {
                predicateBuilder.And(x => x.PublishDate.Substring(0, 4) == year);
            }

            return Sum(connectionHandler, x => x.Transaction.Amount, predicateBuilder.GetExpression());



        }




    }
}
