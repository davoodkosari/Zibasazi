using System;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class UserBooth : DataStructureBase<UserBooth>
    {

        private Guid _userId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                base.SetPropertyValue("UserId", value);
                if (EnterpriseNode == null)
                    this.EnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public EnterpriseNode.DataStructure.EnterpriseNode EnterpriseNode { get; set; }




        private Guid _boothId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid BoothId
        {
            get
            {
                return this._boothId;
            }
            set
            {
                base.SetPropertyValue("BoothId", value);
                if (Booth == null)
                    this.Booth = new Booth { Id = value };
            }
        }

        [Assosiation(PropName = "BoothId")]
        public Booth Booth { get; set; }




        private string _registerDate;
        [DbType("char(10)")]
        public string RegisterDate
        {
            get { return _registerDate; }
            set { base.SetPropertyValue("RegisterDate", value); }
        }


        private byte _status;
        [DbType("tinyint")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private Guid? _transactionId;
        [DbType("uniqueidentifier")]
        public Guid? TransactionId
        {
            get { return _transactionId; }
            set
            {
                base.SetPropertyValue("TransactionId", value);
                if (Transaction == null)
                    this.Transaction = value.HasValue ? new Transaction() { Id = value.Value } : null;
            }
        }
        [Assosiation(PropName = "TransactionId")]
        public Transaction Transaction { get; set; }

        private Guid? _tempId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? TempId
        {
            get { return _tempId; }
            set { base.SetPropertyValue("TempId", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string StatusDescription { get { return ((Enums.RezervState)Status).GetDescriptionInLocalization(); } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string BoothOfficerNames { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.EnterpriseNode.DescriptionField; }
        }
    }
}
