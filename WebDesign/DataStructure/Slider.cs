using System;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Slider : DataStructureBase<Slider>
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

        private Int16 _slideId;
        [Key(false)]
        [DbType("smallint")]
        public Int16 SlideId
        {
            get
            {
                return this._slideId;
            }
            set
            {
                base.SetPropertyValue("SlideId", value);
                this.WebSiteSlide = new Radyn.Slider.DataStructure.Slide { Id = value };
            }
        }

        [Assosiation(PropName = "SlideId")]
        public Radyn.Slider.DataStructure.Slide WebSiteSlide { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSiteSlide.Title; }
        }
    }
}
