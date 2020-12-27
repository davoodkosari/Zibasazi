using System;
using System.Collections.Generic;
using Radyn.Congress.Tools;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Homa : DataStructureBase<Homa>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private string _congressTitle;
        [DbType("nvarchar(500)")]
        [MultiLanguage]
        public string CongressTitle
        {
            get { return _congressTitle; }
            set { base.SetPropertyValue("CongressTitle", value); }
        }
        private string _description;
        [DbType("nvarchar(max)")]
        [MultiLanguage]
        public string Description
        {
            get { return _description; }
            set { base.SetPropertyValue("Description", value); }
        }
        private string _virtualDirectory;
        [DbType("nvarchar(50)")]
        [IsNullable]
        public string VirtualDirectory
        {
            get { return _virtualDirectory; }
            set { base.SetPropertyValue("VirtualDirectory", value); }
        }

        private Guid _ownerId;
        [DbType("uniqueidentifier")]
        public Guid OwnerId
        {
            get
            {
                return this._ownerId;
            }
            set
            {
                base.SetPropertyValue("OwnerId", value);
                if (Owner == null)
                    this.Owner = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Assosiation(PropName = "OwnerId")]
        public EnterpriseNode.DataStructure.EnterpriseNode Owner { get; set; }


        private bool _isDefaultForConfig;
        [DbType("bit")]
        public bool IsDefaultForConfig
        {
            get { return _isDefaultForConfig; }
            set { base.SetPropertyValue("IsDefaultForConfig", value); }
        }


        private int? _congressTypeId;
        [DbType("int")]
        [IsNullable]
        public int? CongressTypeId
        {
            get
            {
                return this._congressTypeId;
            }
            set
            {
                base.SetPropertyValue("CongressTypeId", value);
                if (CongressType == null)
                    this.CongressType =value.HasValue? new CongressType { Id = value.Value }:null;
            }
        }

        [Assosiation(PropName = "CongressTypeId")]
        public CongressType CongressType { get; set; }

        private Int32 _order;
        [DbType("int")]
        public Int32 Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }

        private int? _holdingDays;
        [DbType("int")]
        public int? HoldingDays
        {
            get { return _holdingDays; }
            set { base.SetPropertyValue("HoldingDays", value); }
        }

        private string _createDate;
        [DbType("char(10)")]
        public string CreateDate
        {
            get { return _createDate; }
            set { base.SetPropertyValue("CreateDate", value); }
        }

        private string _startDate;
        [IsNullable]
        [DbType("char(10)")]
        public string StartDate
        {
            get { return _startDate; }
            set { base.SetPropertyValue("StartDate", value); }
        }

        private string _endDate;
        [IsNullable]
        [DbType("char(10)")]
        public string EndDate
        {
            get { return _endDate; }
            set { base.SetPropertyValue("EndDate", value); }
        }

        private bool _enabled;
        [DbType("bit")]
        public bool Enabled
        {
            get { return _enabled; }
            set { base.SetPropertyValue("Enabled", value); }
        }

        private string _installPath;
        [IsNullable]
        [DbType("nvarchar(250)")]
        public string InstallPath
        {
            get { return _installPath; }
            set { base.SetPropertyValue("InstallPath", value); }
        }

        private bool? _isNotified;
        [IsNullable]
        [DbType("bit")]
        public bool? IsNotified
        {
            get { return _isNotified; }
            set { base.SetPropertyValue("IsNotified", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Enums.CongressStatus Status { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool AllowShow
        {
            get { return this.Status == Enums.CongressStatus.NoProblem || this.Status == Enums.CongressStatus.Ended; }
        }

        public Configuration _configuration;

        [Assosiation(PropName = "Id", JoinType = JoinType.Left)]
        public Configuration Configuration
        {
            get { return _configuration ?? (_configuration = new Configuration()); }
            set { _configuration = value; }
        }
        private Guid _configurationId;
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid ConfigurationId
        {
            get { return _configurationId == Guid.Empty ? Id : _configurationId; }
            set { _configurationId = Id; }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.CongressTitle; }
        }
       
    }
}
