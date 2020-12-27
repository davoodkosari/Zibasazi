using System;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressContent : DataStructureBase<CongressContent>
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
                if (this.Homa == null)
                    this.Homa = new Homa { Id = value };
            }
        }


        [Assosiation(PropName = "CongressId", FillData = false)]
        //[Assosiation]
        public Homa Homa { get; set; }

        private Int32 _contentId;
        [Key(false)]
        [DbType("int")]
        public Int32 ContentId
        {
            get
            {
                return this._contentId;
            }
            set
            {
                base.SetPropertyValue("ContentId", value);
                if (Content == null)
                    this.Content = new Content { Id = value };
            }
        }

        [Assosiation(PropName = "ContentId")]
        public Content Content { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Content.DescriptionField; }
        }
    }
}
