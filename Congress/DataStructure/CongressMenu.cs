using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressMenu : DataStructureBase<CongressMenu>
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

        private Guid _menuId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid MenuId
        {
            get
            {
                return this._menuId;
            }
            set
            {
                base.SetPropertyValue("MenuId", value);
                if (Menu == null)
                    this.Menu = new Menu() { Id = value };
            }
        }

        [Assosiation(PropName = "MenuId")]
        public Menu Menu { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Menu.DescriptionField; }
        }
    }
}
