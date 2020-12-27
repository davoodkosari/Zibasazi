using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Radyn.CrossPlatform.DataStructure;
//using Radyn.Message.DataStructure;
using Radyn.News.DataStructure;

namespace Radyn.CrossPlatform.Tools
{
    public class SyncCodeGenerator
    {
        //public string GetMessageQuery(Enums.QueryTypes type, MailInfo item)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var tableName = Enums.ClientsTableNames.Messages.ToString();
        //    switch (type)
        //    {
        //        case Enums.QueryTypes.Delete:
        //            sb.Append("DELETE FROM [" + tableName + "] WHERE Id='" + item.Id + "'");
        //            break;
        //        case Enums.QueryTypes.Insert:
        //            sb.Append("INSERT INTO [" + tableName + "] ([Id],[UserId],[Subject],[Body],[RecordDate],[RecordTime],[Observed],[From],[To]) VALUES ('" + item.Id + "','" + item.To + "', '" + item.Subject + "', '" + item.Body + "', '" + DateTime.Now.ShamsiDate() + "', '" + DateTime.Now.GetTime() + "' , 0, 'سیتسم همایش ها', '" + item.To + "')");
        //            break;
        //        case Enums.QueryTypes.Update:
        //            sb.Append("UPDATE [" + tableName + "] SET " +
        //                      "  Subject='" + item.Subject + "'" +
        //                      ", Body = '" + item.Body + "'" +
        //                      "  WHERE Id = '" + item.Id + "'");
        //            break;
        //    }
        //    return removeInjection(sb.ToString());
        //}

        /*
        public string GetContentCategoryQuery(Enums.QueryTypes type, ContentCategories item)
        {
            StringBuilder sb = new StringBuilder();

            var tableName = Enums.ClientsTableNames.ContentCategories.ToString();
            switch (type)
            {
                case Enums.QueryTypes.Delete:
                    sb.Append("DELETE FROM [" + tableName + "] WHERE Id='" + item.Id + "'");
                    break;
                case Enums.QueryTypes.Insert:
                    sb.Append("INSERT INTO [" + tableName + "] ([Id], [CongressId], [Title], [Description], [OrderCategory], [Image]) VALUES ('" + item.Id + "', '" + item.CongressId + "', '" + item.Title + "', '" + item.Description + "', " + item.OrderCategory + ", '" + item.Image + "')");
                    break;
                case Enums.QueryTypes.Update:
                    sb.Append("UPDATE [" + tableName + "] SET " +
                              "  [Description]='" + item.Description + "'" +
                              ", [Title]='" + item.Title + "'" +
                              ", [OrderCategory]=" + item.OrderCategory +
                              ", [Image]='" + item.Image + "'" +
                              "  WHERE [Id]='" + item.Id + "'");
                    break;
            }

            return sb.ToString();
        }
        */

        /*
        public string GetContentQuery(Enums.QueryTypes type, Contents item)
        {
            StringBuilder sb = new StringBuilder();

            var tableName = Enums.ClientsTableNames.Contents.ToString();
            switch (type)
            {
                case Enums.QueryTypes.Delete:
                    sb.Append("DELETE FROM [" + tableName + "] WHERE Id='" + item.Id + "'");
                    break;
                case Enums.QueryTypes.Insert:
                    sb.Append("INSERT INTO [" + tableName + "] ([Id], [CongressId], [Subject], [Body], [Summary], [ObserverCount], [RecordDate], [RecordTime], [CategoryId], [Image]) VALUES ('" + item.Id + "', '" + item.CongressId + "', '" + item.Subject + "', '" + item.Body + "', '" + item.Summary + "', 0 , '" + DateTime.Now.ShamsiDate() + "' , '" + DateTime.Now.GetTime() + "' , '" + item.CategoryId + "', '" + item.Image + "')");
                    break;
                case Enums.QueryTypes.Update:
                    sb.Append("UPDATE [" + tableName + "] SET " +
                              "  [Subject] = '" + item.Subject + "'" +
                              ", [Body] = '" + item.Body + "'" +
                              ", [Summary]='" + item.Summary + "'" +
                              ", [CategoryId]='" + item.CategoryId + "'" +
                              ", [Image]='" + item.Image + "'" +
                              "  WHERE [Id] = '" + item.Id + "'");
                    break;
            }

            return sb.ToString();
        }
        */

        /*
        public string GetNewsQuery(Enums.QueryTypes type, Guid? congressId, News.DataStructure.News newsItem, NewsContent content)
        {
            StringBuilder sb = new StringBuilder();

            var tableName = Enums.ClientsTableNames.News.ToString();
            switch (type)
            {
                case Enums.QueryTypes.Delete:
                    sb.Append("DELETE FROM [" + tableName + "] WHERE Id=" + newsItem.Id);
                    break;
                case Enums.QueryTypes.Insert:
                    sb.Append("INSERT INTO [" + tableName + "] " +
                              "([Id], [CongressId], [LangId], [Lead], [Title] ,[Body], [PublishDate], [PublishTime], [NewsCategoryId], [VisitCount], [OfflineVisitCount],[Image])" +
                              " VALUES " +
                              "(" + newsItem.Id + ", '" + congressId + "', '" + content.LanguageId + "' ,'" + content.Lead + "' , '" + content.Title1 + "', '" + content.Body + "', '" + newsItem.PublishDate + "' , '" + newsItem.PublishTime + "' , '" + newsItem.NewsCategoryId + "' , " + content.VisitCount + ", 0, '" + newsItem.ThumbnailId + "')");
                    break;
                case Enums.QueryTypes.Update:
                    sb.Append("UPDATE [" + tableName + "] SET" +
                            "  [LangId]='" + content.LanguageId + "' " +
                            ", [Lead]='" + content.Lead + "' " +
                            ", [Title]='" + content.Title1 + "' " +
                            ", [Body]='" + content.Body + "' " +
                            ", [PublishDate]='" + newsItem.PublishDate + "' " +
                            ", [PublishTime]='" + newsItem.PublishTime + "' " +
                            ", [NewsCategoryId]='" + newsItem.NewsCategoryId + "' " +
                            ", [VisitCount]=" + content.VisitCount +
                            ", [Image]='" + newsItem.ThumbnailId + "'" +
                            "  WHERE [Id]=" + newsItem.Id);
                    break;
            }

            return sb.ToString();
        }
        */

        //public string CreateInsertQuery<TModel>(TModel obj, string tableName, List<Expression<Func<TModel, object>>> items)
        public string CreateInsertQuery<TModel>(TModel obj, string tableName, List<SyncAction<TModel>> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO ");
            sb.Append(tableName);
            sb.Append("(");
            int i = 0;
            foreach (var action in items)
            {
                //var propInfo = GetPropertyInfo(action);
                var propName = action.GetName();
                if (i++ > 0)
                {
                    sb.Append(", ");
                }
                sb.Append("[" + propName + "]");
            }
            sb.Append(") VALUES (");
            int j = 0;
            foreach (var action in items)
            {
                //var propValue = action(obj);
                if (j++ > 0)
                {
                    sb.Append(", ");
                }
                //var result = DoUsing(obj, action);
                var result = action.GetValue(obj);
                if (isNumeric(result))
                {
                    sb.Append(result);
                }
                else
                {
                    sb.Append("'" + result + "'");
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        public string CreateUpdateQuery<TModel>(TModel obj, string tableName, List<SyncAction<TModel>> items, List<SyncAction<TModel>> keyList)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(tableName);
            sb.Append(" SET ");

            int i = 0;
            foreach (var action in items)
            {
                if (i++ > 0)
                {
                    sb.Append(", ");
                }

                var propName = action.GetName();
                var result = action.GetValue(obj);

                sb.Append("[" + propName + "]");
                sb.Append(" = ");

                if (isNumeric(result))
                {
                    sb.Append(result);
                }
                else
                {
                    sb.Append("'" + result + "'");
                }
            }

            sb.Append(" WHERE ");
            int j = 0;
            foreach (var action in keyList)
            {
                if (j++ > 0)
                {
                    sb.Append(", ");
                }

                var propName = action.GetName();
                var result = action.GetValue(obj);

                sb.Append("[" + propName + "]");
                sb.Append(" = ");

                if (isNumeric(result))
                {
                    sb.Append(result);
                }
                else
                {
                    sb.Append("'" + result + "'");
                }
            }

            return sb.ToString();
        }

        public string CreateDeleteQuery<TModel>(TModel obj, string tableName, List<SyncAction<TModel>> keyList)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(tableName);
            sb.Append(" ");

            sb.Append(" WHERE ");
            int i = 0;
            foreach (var action in keyList)
            {
                if (i++ > 0)
                {
                    sb.Append(", ");
                }

                var propName = action.GetName();
                var result = action.GetValue(obj);

                sb.Append("[" + propName + "]");
                sb.Append(" = ");

                if (isNumeric(result))
                {
                    sb.Append(result);
                }
                else
                {
                    sb.Append("'" + result + "'");
                }
            }

            return sb.ToString();
        }

        private bool isNumeric(object obj)
        {
            if (
                    obj is int ||
                    obj is decimal ||
                    obj is byte ||
                    obj is long ||
                    obj is short ||
                    obj is float ||
                    obj is double
                    )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
        public static object DoUsing<TModel>(TModel objModel, Expression<Func<TModel, object>> action)
        {
            var a = action.Compile();
            return a(objModel);
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException(@"Not a member access", nameof(expression));
            }

            return memberExpression.Member as PropertyInfo;
        }
        */

        /*
        public PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            //MemberExpression member = propertyLambda as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }
        */
    }
}
