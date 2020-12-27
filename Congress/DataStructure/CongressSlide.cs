using System;
using Radyn.Framework;
using Radyn.Slider.DataStructure;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressSlide : DataStructureBase<CongressSlide>
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

        private short _slideId;
        [Key(false)]
        [DbType("smallint")]
        public short SlideId
        {
            get
            {
                return this._slideId;
            }
            set
            {
                base.SetPropertyValue("SlideId", value);
                if (Slide == null)
                    this.Slide = new Slide { Id = value };
            }
        }

        [Assosiation(PropName = "SlideId")]
        public Slide Slide { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Slide.DescriptionField; }
        }
    }
}
