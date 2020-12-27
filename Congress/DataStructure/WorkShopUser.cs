using System;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class WorkShopUser : DataStructureBase<WorkShopUser>
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

        private Guid _workShopId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid WorkShopId
        {
            get
            {
                return this._workShopId;
            }
            set
            {
                base.SetPropertyValue("WorkShopId", value);
                if (WorkShop == null)
                    this.WorkShop = new WorkShop { Id = value };
            }
        }

        [Assosiation(PropName = "WorkShopId")]
        public WorkShop WorkShop { get; set; }





        private byte _status;
        [DbType("tinyint")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private string _registerDate;
        [Layout(Caption = "تاريخ ثبت نام")]
        [DbType("char(10)")]
        public string RegisterDate
        {
            get { return _registerDate; }
            set { base.SetPropertyValue("RegisterDate", value); }
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
        [DbType("uniqueidentifier")]
        public Guid? TempId
        {
            get { return _tempId; }
            set { base.SetPropertyValue("TempId", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string StatusDescription { get { return ((Enums.WorkShopRezervState)Status).GetDescriptionInLocalization(); } }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.User.DescriptionField; }
        }
    }
}
