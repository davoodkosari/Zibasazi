using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressHtml : DataStructureBase<CongressHtml>
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

        private Guid _htmlDesginId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid HtmlDesginId
        {
            get
            {
                return this._htmlDesginId;
            }
            set
            {
                base.SetPropertyValue("HtmlDesginId", value);
                if (HtmlDesgin == null)
                    this.HtmlDesgin = new HtmlDesgin { Id = value };
            }
        }

        [Assosiation(PropName = "HtmlDesginId")]
        public HtmlDesgin HtmlDesgin { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
