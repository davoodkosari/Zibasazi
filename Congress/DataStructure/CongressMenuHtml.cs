using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressMenuHtml : DataStructureBase<CongressMenuHtml>
    {
        private Guid _congressId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid CongressId
        {
            get
            {
                return this._congressId;
            }
            set
            {
                base.SetPropertyValue("CongressId", value);
                if (Homa == null)
                    this.Homa = new Homa { Id = value };
            }
        }

        [Assosiation(PropName = "CongressId", FillData = false)]
        public Homa Homa { get; set; }

        private Guid _menuHtmlId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid MenuHtmlId
        {
            get
            {
                return this._menuHtmlId;
            }
            set
            {
                base.SetPropertyValue("MenuHtmlId", value);
                if (MenuHtml == null)
                    this.MenuHtml = new MenuHtml { Id = value };
            }
        }

        [Assosiation(PropName = "MenuHtmlId")]
        public MenuHtml MenuHtml { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
