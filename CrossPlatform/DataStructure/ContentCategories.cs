using System;
using Radyn.Framework;

namespace Radyn.CrossPlatform.DataStructure
{
    [Serializable]
    [Schema("CrossPlatform")]
    public sealed class ContentCategories : DataStructureBase<ContentCategories>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _congressId;
        [DbType("uniqueidentifier")]
        public Guid CongressId
        {
            get { return _congressId; }
            set { base.SetPropertyValue("CongressId", value); }
        }

        private string _title;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        private Int32? _orderCategory;
        [IsNullable]
        [DbType("int")]
        public Int32? OrderCategory
        {
            get { return _orderCategory; }
            set { base.SetPropertyValue("OrderCategory", value); }
        }

        private Guid? _image;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? Image
        {
            get { return _image; }
            set { base.SetPropertyValue("Image", value); }
        }

        private string _description;
        [IsNullable]
        [DbType("nvarchar(150)")]
        public string Description
        {
            get { return _description; }
            set { base.SetPropertyValue("Description", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }
    }
}
