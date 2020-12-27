using System;
using System.Security.Policy;
using Radyn.Framework;

namespace Radyn.CrossPlatform.DataStructure
{
    [Serializable]
    [Schema("CrossPlatform")]
    public sealed class SyncAdapter : DataStructureBase<SyncAdapter>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private string _sourceId;
        [IsNullable]
        [DbType("varchar(100)")]
        public string SourceId
        {
            get { return _sourceId; }
            set { base.SetPropertyValue("SourceId", value); }
        }

        private Guid _congressId;
        [DbType("uniqueidentifier")]
        public Guid CongressId
        {
            get { return _congressId; }
            set { base.SetPropertyValue("CongressId", value); }
        }

        private string _tableName;
        [IsNullable]
        [DbType("varchar(100)")]
        public string TableName
        {
            get { return _tableName; }
            set { base.SetPropertyValue("TableName", value); }
        }

        private Int64 _versionId;
        [DbType("bigint")]
        [DisableAction(DiableSelect = false, DisableInsert = true, DisableUpdate = true)]
        public Int64 VersionId
        {
            get { return _versionId; }
            set { base.SetPropertyValue("VersionId", value); }
        }

        private DateTime _recordDate;
        [IsNullable]
        [DbType("date")]
        public DateTime RecordDate
        {
            get { return _recordDate; }
            set
            {
                base.SetPropertyValue("RecordDate", value);
            }
        }

        private string _script;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Script
        {
            get { return _script; }
            set { base.SetPropertyValue("Script", value); }
        }

        private Int32? _type;
        [IsNullable]
        [DbType("int")]
        public Int32? Type
        {
            get { return _type; }
            set { base.SetPropertyValue("Type", value); }
        }

        private bool? _deprecated;
        [IsNullable]
        [DbType("bit")]
        public bool? Deprecated
        {
            get { return _deprecated; }
            set { base.SetPropertyValue("Deprecated", value); }
        }

        private Guid? _userId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? UserId
        {
            get { return _userId; }
            set { base.SetPropertyValue("UserId", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.TableName; }
        }
    }
}
