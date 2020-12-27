using System;
using Radyn.Framework;
using Radyn.News.Tools;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class News : DataStructureBase<News>
    {
        private Guid _webId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid WebId
        {
            get
            {
                return this._webId;
            }
            set
            {
                base.SetPropertyValue("WebId", value);
                this.WebSite = new WebSite { Id = value };
            }
        }

        [Assosiation]
        public WebSite WebSite { get; set; }

        private Int32 _newsId;
        [Key(false)]
        [DbType("int")]
        public Int32 NewsId
        {
            get
            {
                return this._newsId;
            }
            set
            {
                base.SetPropertyValue("NewsId", value);
                this.WebSiteNews = new Radyn.News.DataStructure.News { Id = value };
            }
        }

        [Assosiation(PropName = "NewsId")]
        public Radyn.News.DataStructure.News WebSiteNews { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteNews.PublishDate; }
        }
    }
}
