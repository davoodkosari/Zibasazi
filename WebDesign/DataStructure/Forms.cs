using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Forms : DataStructureBase<Forms>
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

        private Guid _formId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid FormId
        {
            get
            {
                return this._formId;
            }
            set
            {
                base.SetPropertyValue("FormId", value);
                this.WebSiteForm = new Radyn.FormGenerator.DataStructure.FormStructure { Id = value };
            }
        }

        [Assosiation(PropName = "FormId")]
        public Radyn.FormGenerator.DataStructure.FormStructure WebSiteForm { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteForm.Name; }
        }
    }
}
