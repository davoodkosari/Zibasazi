using System;
using Radyn.Framework;
using Radyn.Reservation.DataStructure;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class UserChair : DataStructureBase<UserChair>
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
                if (User == null)
                    this.User = new User { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public User User { get; set; }

        private Guid _chairId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid ChairId
        {
            get
            {
                return this._chairId;
            }
            set
            {
                base.SetPropertyValue("ChairId", value);
                if (Chair == null)
                    this.Chair = new Chair { Id = value };
            }
        }

        [Assosiation(PropName = "ChairId")]
        public Chair Chair { get; set; }

        private byte _status;
        [DbType("tinyint")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private Guid? _transactionId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? TransactionId
        {
            get { return _transactionId; }
            set { base.SetPropertyValue("TransactionId", value); }
        }

        private string _registerDate;
        [DbType("char(10)")]
        public string RegisterDate
        {
            get { return _registerDate; }
            set { base.SetPropertyValue("RegisterDate", value); }
        }

        private Guid? _tempId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? TempId
        {
            get { return _tempId; }
            set { base.SetPropertyValue("TempId", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.User.DescriptionField; }
        }
    }
}
