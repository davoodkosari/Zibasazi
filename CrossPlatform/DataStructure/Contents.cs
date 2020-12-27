using System;
using Radyn.Framework;

namespace Radyn.CrossPlatform.DataStructure
{
    [Serializable]
    [Schema("CrossPlatform")]
    public sealed class Contents : DataStructureBase<Contents>
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

        private Guid _categoryId;
        [DbType("uniqueidentifier")]
        public Guid? CategoryId
        {
            get { return _categoryId; }
            set { base.SetPropertyValue("CategoryId", value); }
        }

        private string _subject;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Subject
        {
            get { return _subject; }
            set { base.SetPropertyValue("Subject", value); }
        }

        private string _body;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Body
        {
            get { return _body; }
            set { base.SetPropertyValue("Body", value); }
        }

        private string _summary;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Summary
        {
            get { return _summary; }
            set { base.SetPropertyValue("Summary", value); }
        }

        private Int32? _observerCount;
        [IsNullable]
        [DbType("int")]
        public Int32? ObserverCount
        {
            get { return _observerCount; }
            set { base.SetPropertyValue("ObserverCount", value); }
        }

        private string _recordDate;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string RecordDate
        {
            get { return _recordDate; }
            set { base.SetPropertyValue("RecordDate", value); }
        }

        private string _recordTime;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string RecordTime
        {
            get { return _recordTime; }
            set { base.SetPropertyValue("RecordTime", value); }
        }

        private Guid? _image;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? Image
        {
            get { return _image; }
            set { base.SetPropertyValue("Image", value); }
        }

        [Assosiation]
        public ContentCategories ContentCategory { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Subject; }
        }
    }
}
