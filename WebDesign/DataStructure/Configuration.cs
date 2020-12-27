using System;
using Radyn.Framework;

namespace Radyn.WebDesign.DataStructure
{
    [Serializable]
    [Schema("WebDesign")]
    public sealed class Configuration : DataStructureBase<Configuration>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                base.SetPropertyValue("Id", value);
                this.WebSite = new WebSite { Id = value };
            }
        }

        [Assosiation]
        public WebSite WebSite { get; set; }

        private Int32? _maxNewsShow;
        [IsNullable]
        [DbType("int")]
        public Int32? MaxNewsShow
        {
            get { return _maxNewsShow; }
            set { base.SetPropertyValue("MaxNewsShow", value); }
        }

        private bool _enabled;
        [DbType("bit")]
        public bool Enabled
        {
            get { return _enabled; }
            set { base.SetPropertyValue("Enabled", value); }
        }

        private Int32? _headerId;
        [IsNullable]
        [DbType("int")]
        public Int32? HeaderId
        {
            get { return _headerId; }
            set { base.SetPropertyValue("HeaderId", value); }
        }

        private Int32? _footerId;
        [IsNullable]
        [DbType("int")]
        public Int32? FooterId
        {
            get { return _footerId; }
            set { base.SetPropertyValue("FooterId", value); }
        }

        private short? _bigSlideId;

        [DbType("smallint")]
        [IsNullable]
        public short? BigSlideId
        {
            get { return this._bigSlideId; }
            set { base.SetPropertyValue("BigSlideId", value); }
        }

        private short? _miniSlideId;
        [DbType("smallint")]
        [IsNullable]
        public short? MiniSlideId
        {
            get { return this._miniSlideId; }
            set { base.SetPropertyValue("MiniSlideId", value); }
        }

        private short? _averageSlideId;
        [DbType("smallint")]
        [IsNullable]
        public short? AverageSlideId
        {
            get { return this._averageSlideId; }
            set { base.SetPropertyValue("AverageSlideId", value); }
        }
        private int? _introPageId;
        [IsNullable]
        [DbType("int")]
        public int? IntroPageId
        {
            get { return _introPageId; }
            set { base.SetPropertyValue("IntroPageId", value); }
        }



        private Int16? _certificateSlideId;
        [IsNullable]
        [DbType("smallint")]
        public Int16? CertificateSlideId
        {
            get { return _certificateSlideId; }
            set { base.SetPropertyValue("CertificateSlideId", value); }
        }

        private Int16? _eventsSlideId;
        [IsNullable]
        [DbType("smallint")]
        public Int16? EventsSlideId
        {
            get { return _eventsSlideId; }
            set { base.SetPropertyValue("EventsSlideId", value); }
        }


        private Guid? _defaultContainerID;

        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? DefaultContainerID
        {
            get { return _defaultContainerID; }
            set { base.SetPropertyValue("DefaultContainerID", value); }
        }

        private Guid? _defaultHTMLID;

        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? DefaultHTMLID
        {
            get { return _defaultHTMLID; }
            set { base.SetPropertyValue("DefaultHTMLID", value); }
        }

        private Guid? _defaultMenuHtmlId;
        [DbType("uniqueidentifier")]
        public Guid? DefaultMenuHtmlId
        {
            get
            {
                return this._defaultMenuHtmlId;
            }
            set
            {
                base.SetPropertyValue("DefaultMenuHtmlId", value);
                if (MenuHtml == null)
                    this.MenuHtml = value.HasValue ? new ContentManager.DataStructure.MenuHtml { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "DefaultMenuHtmlId",FillAnyway = true)]
        public ContentManager.DataStructure.MenuHtml MenuHtml { get; set; }

        private Guid? _favIcon;

        [DbType("uniqueidentifier")]
        [IsNullable]
        public Guid? FavIcon
        {
            get { return _favIcon; }
            set
            {
                base.SetPropertyValue("FavIcon", value);
            }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.WebSite.Title; }
        }
    }
}
