using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.ContentManager.DA;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.ContentManager.BO
{
    public class ContentBO : BusinessBase<Content>
    {
     
        public string GetHtml(Content content, ContentContent contentContent, bool hascontainer = true, Container DefaultContrainer = null)
        {

            if (content == null) return string.Empty;
            var st = new StringBuilder();
            if (!string.IsNullOrEmpty(content.UserScript))
                st.Append("<script type=\"text/javascript\">" + content.UserScript + "</script>");
            var contentContainer = content.Container;
            if (DefaultContrainer != null)
                contentContainer = DefaultContrainer;
            if (contentContainer == null || !hascontainer)
                st.Append(contentContent.Text);
            else
            {

                if (contentContainer.Html == null)
                    st.Append(contentContent.Text);
                else
                {
                    var htmltext = contentContainer.Html;
                    var html = htmltext.Replace("{Body}", contentContent.Text);
                    st.Append(html.Replace("{title}", contentContent.Title));
                }

            }
            return contentContent.Text.Length < 10 ? null : st.ToString();
        }

        public IEnumerable<Content> Search(IConnectionHandler connectionHandler, string qry)
        {
            var predicateBuilder=new PredicateBuilder<Content>();
            var b=new ContentContentBO().Select(connectionHandler,x=>x.Id,x=>x.Abstract.Contains(qry)||
            x.Text.Contains(qry)|| x.Title.Contains(qry)|| x.Subject.Contains(qry));
            if (b.Any())
                predicateBuilder.And(x => x.PageName.Contains(qry) || x.Link.Contains(qry) || x.Id.In(b));
            else
                predicateBuilder.And(x => x.PageName.Contains(qry) || x.Link.Contains(qry));
            return this.Where(connectionHandler, predicateBuilder.GetExpression());
           
        }
        public string GetHtml(IConnectionHandler connectionHandler, Content content, string culture, bool hascontainer = true, Container DefaultContrainer = null)
        {

            if (content == null) return string.Empty;
            var st = new StringBuilder();
            if (!string.IsNullOrEmpty(content.UserScript))
                st.Append("<script type=\"text/javascript\">" + content.UserScript + "</script>");

            var contentContent = new ContentContentBO().Get(connectionHandler, content.Id, culture) ??
                                 new ContentContent
                                 {
                                     Id = content.Id,
                                     Abstract = content.Abstract,
                                     Description = content.Description,
                                     Text = content.Text,
                                     Subject = content.Subject,
                                     Title = content.Title,
                                 };
            var contentContainer = content.Container;
            if (DefaultContrainer != null)
                contentContainer = DefaultContrainer;

            if (contentContainer == null || !hascontainer) st.Append(contentContent.Text);
            else
            {

                if (contentContainer.Html == null) st.Append(contentContent.Text);
                else
                {
                    var htmltext = contentContainer.Html;
                    var html = htmltext.Replace("{Body}", contentContent.Text);
                    st.Append(html.Replace("{title}", contentContent.Title));
                }

            }
            //اگر محتوای تگ خالی است خالی برگرداند تا تگ خالی در صفحه نمایش ندهد
            return contentContent.Text.Length < 10 ? null : st.ToString();
        }

        public async Task<string> GetHtmlAsync(IConnectionHandler connectionHandler, Content content, string culture, bool hascontainer = true, Container DefaultContrainer = null)
        {

            if (content == null) return string.Empty;
            var st = new StringBuilder();
            if (!string.IsNullOrEmpty(content.UserScript))
                st.Append("<script type=\"text/javascript\">" + content.UserScript + "</script>");

            var contentContent =await new ContentContentBO().GetAsync(connectionHandler, content.Id, culture) ??
                                 new ContentContent
                                 {
                                     Id = content.Id,
                                     Abstract = content.Abstract,
                                     Description = content.Description,
                                     Text = content.Text,
                                     Subject = content.Subject,
                                     Title = content.Title,
                                 };
            var contentContainer = content.Container;
            if (DefaultContrainer != null)
                contentContainer = DefaultContrainer;

            if (contentContainer == null || !hascontainer) st.Append(contentContent.Text);
            else
            {

                if (contentContainer.Html == null) st.Append(contentContent.Text);
                else
                {
                    var htmltext = contentContainer.Html;
                    var html = htmltext.Replace("{Body}", contentContent.Text);
                    st.Append(html.Replace("{title}", contentContent.Title));
                }

            }
            //اگر محتوای تگ خالی است خالی برگرداند تا تگ خالی در صفحه نمایش ندهد
            return contentContent.Text.Length < 10 ? null : st.ToString();
        }
        public override bool Insert(IConnectionHandler connectionHandler, Content obj)
        {
            if (!base.Insert(connectionHandler, obj)) return false;
            obj.Link = "/ContentManager/Content/View/" + obj.Id + "/" + (obj.Title != null ? obj.Title.FixUrlCatchall().Replace(' ', '-') : "");
            return base.Update(connectionHandler, obj);
        }
        protected override void CheckConstraint(IConnectionHandler connectionHandler, Content item)
        {
            if (item.MenuId.HasValue)
                item.IsMenu = true;
            if (item.IsMenu.Equals(false) || (item.IsMenu && item.MenuId == null))
            {
                item.IsMenu = false;
                item.MenuId = null;
            }
            if (item.IsSection.Equals(false) || (item.IsSection && item.SectionId == null))
            {
                item.IsSection = false;
                item.SectionId = null;
            }
            if (item.HasContainer.Equals(false) || (item.HasContainer && item.ContainerId == null))
            {
                item.HasContainer = false;
                item.ContainerId = null;
            }
            if (item.Abstract != null)
            {
                if (item.Abstract.Contains("'"))
                    item.Abstract = item.Abstract.Replace("'", "''");
            }
            if (item.Text != null)
            {
                if (item.Text.Contains("'"))
                    item.Text = item.Text.Replace("'", "''");
            }
            if (item.UserScript != null)
            {
                if (item.UserScript.Contains("'"))
                    item.UserScript = item.UserScript.Replace("'", "''");
            }

            item.Link = "/ContentManager/Content/View/" + item.Id + "/" + (item.Title != null ? item.Title.FixUrlCatchall().Replace(' ', '-') : "");
            if (item.MenuId != null)
            {
                var menuBo = new MenuBO();
                var menu = menuBo.Get(connectionHandler, item.MenuId);
                menu.Link = item.Link;
                if (!menuBo.Update(connectionHandler, menu))
                    throw new Exception("خطایی در ویرایش منوی محتوا وجود دارد");
            }
            base.CheckConstraint(connectionHandler, item);
        }
    }
}
