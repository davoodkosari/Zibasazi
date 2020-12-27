using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongessGallery : DataStructureBase<CongessGallery>
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

        private Guid _galleryId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid GalleryId
        {
            get
            {
                return this._galleryId;
            }
            set
            {
                base.SetPropertyValue("GalleryId", value);
                if (Gallery == null)
                    this.Gallery = new Gallery.DataStructure.Gallery { Id = value };
            }
        }

        [Assosiation(PropName = "GalleryId")]
        public Gallery.DataStructure.Gallery Gallery { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Gallery.DescriptionField; }
        }
    }
}
