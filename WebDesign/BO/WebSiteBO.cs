using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Definition;

namespace Radyn.WebDesign.BO
{
    internal class WebSiteBO : BusinessBase<WebSite>
    {
        public bool Update(IConnectionHandler connectionHandler, WebSite webSite, List<WebSiteAlias> webSiteAliases)
        {

            if (!this.Update(connectionHandler, webSite))
                throw new Exception("خطا در ثبت اطلاعات");
            var homaAliasBo = new WebSiteAliasBO();
            var list = homaAliasBo.Where(connectionHandler, x => x.WebSiteId == webSite.Id);
            foreach (var homaAliase in webSiteAliases)
            {
                var homaAlias = homaAliasBo.Get(connectionHandler, homaAliase.Id);
                if (homaAlias == null)
                {
                    homaAliase.WebSiteId = webSite.Id;
                    homaAliase.WebSite = webSite;
                    if (!homaAliasBo.Insert(connectionHandler, homaAliase))
                        throw new Exception("خطا در ثبت اطلاعات");
                }
                else
                {
                    homaAliase.WebSite = webSite;
                    if (!homaAliasBo.Update(connectionHandler, homaAliase))
                        throw new Exception("خطا در ثبت اطلاعات");
                }

            }
            foreach (var homaAliase in list)
            {
                if (webSiteAliases.Any(x => x.Id == homaAliase.Id)) continue;
                if (!homaAliasBo.Delete(connectionHandler, homaAliase.Id))
                    throw new Exception("خطا در ثبت اطلاعات");
            }

            return true;
        }
        public override bool Delete(IConnectionHandler connectionHandler,
            params object[] keys)
        {
            var obj = this.Get(connectionHandler, keys);
            var siteAliasBo = new WebSiteAliasBO();
            var list = siteAliasBo.Where(connectionHandler, x => x.WebSiteId == obj.Id);
            foreach (var homaAliase in list)
            {
                if (!siteAliasBo.Delete(connectionHandler, homaAliase.Id))
                    throw new Exception("خطا در حذف اطلاعات");
            }
            if (!base.Delete(connectionHandler, keys))
                throw new Exception("خطا در حذف اطلاعات");
            return true;
        }
        public bool Insert(IConnectionHandler connectionHandler,
            WebSite webSite, List<WebSiteAlias> webSiteAliases)
        {

            if (!this.Insert(connectionHandler, webSite))
                throw new Exception("خطا در ثبت اطلاعات");
            var homaAliasBo = new WebSiteAliasBO();

            foreach (var homaAliase in webSiteAliases)
            {
                homaAliase.WebSiteId = webSite.Id;
                homaAliase.WebSite = webSite;
                if (!homaAliasBo.Insert(connectionHandler, homaAliase))
                    throw new Exception("خطا در ثبت اطلاعات");
            }
            return true;
        }

        public override bool Insert(IConnectionHandler connectionHandler, WebSite obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public WebSite GetWebSiteByUrl(IConnectionHandler connectionHandler, string authority)
        {
            var newGuid = Guid.NewGuid();
            var encrypt = StringUtils.Encrypt(authority.ToLower());
            var predicateBuilder = new PredicateBuilder<WebSite>();
            predicateBuilder.Or(x => x.InstallPath == encrypt);
            var homas = new WebSiteAliasBO().Select(connectionHandler, x => x.WebSiteId, x => x.Url == encrypt);
            if (homas.Any())
                predicateBuilder.Or(x => x.Id.In(homas));
            var webSite = this.FirstOrDefault(connectionHandler, predicateBuilder.GetExpression());
            if (webSite != null)
            {
                webSite.ConfigurationId = webSite.Id;
                if (!webSite.Enabled)
                {
                    webSite.Status = Enums.WebSiteStatus.Disabled;
                    webSite.Id = newGuid;
                    return webSite;

                }
                if (webSite.Configuration == null ||
                    (webSite.Configuration != null && webSite.Configuration.Id == Guid.Empty))
                {
                    webSite.Status = Enums.WebSiteStatus.NotConfiged;
                    webSite.Id = newGuid;
                    return webSite;

                }
                webSite.Status = Enums.WebSiteStatus.NoProblem;
                return webSite;
            }

            webSite = new WebSite
            {
                Configuration = new Configuration(),
                Id = newGuid,
                Status = Enums.WebSiteStatus.NotRegistered
            };
            return webSite;

        }
        public override WebSite Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var obj = base.Get(connectionHandler, keys);
            if (obj != null&&!string.IsNullOrEmpty(obj.InstallPath))
                obj.InstallPath = StringUtils.Decrypt(obj.InstallPath).ToLower();
            return obj;
        }

        protected override void CheckConstraint(IConnectionHandler connectionHandler, WebSite item)
        {
            if (!string.IsNullOrEmpty(item.InstallPath))
            {
               
                var encrypt = StringUtils.Encrypt(item.InstallPath.ToLower());
                var byUrl = this.Any(connectionHandler, c => c.InstallPath == encrypt&&c.Id!=item.Id);
                if (byUrl)
                    throw new Exception("وب سایتی قبلا در این مسیر ثبت شده است");
                item.InstallPath = encrypt;
            }
            else
                throw new Exception("مسیر وب سایت را وارد کنید");

        }
    }
}
