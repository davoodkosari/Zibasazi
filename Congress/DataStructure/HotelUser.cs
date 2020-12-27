using System;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class HotelUser : DataStructureBase<HotelUser>
    {
        private Guid _hotelId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid HotelId
        {
            get
            {
                return this._hotelId;
            }
            set
            {
                base.SetPropertyValue("HotelId", value);
                if (Hotel == null)
                    this.Hotel = new Hotel { Id = value };
            }
        }

        [Assosiation(PropName = "HotelId")]
        public Hotel Hotel { get; set; }

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

        private string _registerDate;
        [DbType("char(10)")]
        public string RegisterDate
        {
            get { return _registerDate; }
            set { base.SetPropertyValue("RegisterDate", value); }
        }

        private Int32? _daysCount;
        [Layout(Caption = "تعداد روزهاي رزرو")]
        [IsNullable]
        [DbType("int")]
        public Int32? DaysCount
        {
            get { return _daysCount; }
            set { base.SetPropertyValue("DaysCount", value); }
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
        public override string DescriptionField
        {
            get { return this.User.DescriptionField; }
        }
    }
}
