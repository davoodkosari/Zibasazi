using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class FAQ : DataStructureBase<FAQ>
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

        private Guid _fAQId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FAQId
        {
            get
            {
                return this._fAQId;
            }
            set
            {
                base.SetPropertyValue("FAQId", value);
                this.WebSiteFaq = new Radyn.FAQ.DataStructure.FAQ { Id = value };
            }
        }

        [Assosiation]
        public Radyn.FAQ.DataStructure.FAQ WebSiteFaq { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteFaq.Question; }
        }
    }
}
