using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Gallery : DataStructureBase<Gallery>
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

        [Assosiation(PropName = "WebId")]
        public WebSite WebSite { get; set; }

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
                this.WebSiteGallery = new Radyn.Gallery.DataStructure.Gallery { Id = value };
            }
        }

        [Assosiation(PropName = "GalleryId")]
        public Radyn.Gallery.DataStructure.Gallery WebSiteGallery { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteGallery.Title; }
        }
    }
}
