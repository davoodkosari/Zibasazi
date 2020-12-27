using System;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Payment.DataStructure;
using Radyn.Reservation.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class User : DataStructureBase<User>
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
                if (EnterpriseNode == null)
                    this.EnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Assosiation(PropName = "Id")]
        public EnterpriseNode.DataStructure.EnterpriseNode EnterpriseNode { get; set; }

        private Guid _congressId;
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

        private string _username;
        [DbType("nvarchar(100)")]
        public string Username
        {
            get { return _username; }
            set { base.SetPropertyValue("Username", value); }
        }

        private string _fbTokenId;
        [IsNullable]
        [DbType("nvarchar(500)")]
        public string FbTokenId
        {
            get { return _fbTokenId; }
            set { base.SetPropertyValue("FbTokenId", value); }
        }

        private Int64 _number;
        [DbType("bigint")]
        [Identity(true)]
        public Int64 Number
        {
            get { return _number; }
            set { base.SetPropertyValue("Number", value); }
        }

        private string _password;
        [IsNullable]
        [DbType("varchar(200)")]
        public string Password
        {
            get { return _password; }
            set { base.SetPropertyValue("Password", value); }
        }

        private string _registerDate;
        [DbType("char(10)")]
        public string RegisterDate
        {
            get { return _registerDate; }
            set { base.SetPropertyValue("RegisterDate", value); }
        }

        private string _comment;
        [IsNullable]
        [DbType("nvarchar(1000)")]
        public string Comment
        {
            get { return _comment; }
            set { base.SetPropertyValue("Comment", value); }
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

        private Guid? _paymentTypeId;
        [DbType("uniqueidentifier")]
        public Guid? PaymentTypeId
        {
            get
            {
                return this._paymentTypeId;
            }
            set
            {
                base.SetPropertyValue("PaymentTypeId", value);
                if (PaymentType == null)
                    this.PaymentType = value.HasValue ? new UserRegisterPaymentType { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "PaymentTypeId")]
        public UserRegisterPaymentType PaymentType { get; set; }

        private byte _status;
        [DbType("tinyint")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private Guid? _parentId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ParentId
        {
            get
            {
                return this._parentId;
            }
            set
            {
                base.SetPropertyValue("ParentId", value);
                if (Parent == null)
                    this.Parent = value.HasValue ? new User() { Id = value.Value } : null;

            }
        }
        [Assosiation(PropName = "ParentId")]
        public User Parent { get; set; }


        private string _paymentTypeDaysInfo;
        [IsNullable]
        [DbType("varchar(50)")]
        public string PaymentTypeDaysInfo
        {
            get
            {
                return this._paymentTypeDaysInfo;
            }
            set
            {
                base.SetPropertyValue("PaymentTypeDaysInfo", value);


            }
        }


        private Guid? _groupRegisterDiscountId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? GroupRegisterDiscountId
        {
            get
            {
                return this._groupRegisterDiscountId;
            }
            set
            {
                base.SetPropertyValue("GroupRegisterDiscountId", value);
                if (GroupRegisterDiscount == null)
                    this.GroupRegisterDiscount = value.HasValue ? new GroupRegisterDiscount { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "گروه")]
        [Assosiation(PropName = "GroupRegisterDiscountId")]
        public GroupRegisterDiscount GroupRegisterDiscount { get; set; }


        private Guid? _chairId;
        [DbType("uniqueidentifier")]
        public Guid? ChairId
        {
            get
            {
                return this._chairId;
            }
            set
            {
                base.SetPropertyValue("ChairId", value);
                if (Chair == null)
                    this.Chair = value.HasValue ? new Chair() { Id = value.Value } : null;

            }
        }
        [Assosiation(PropName = "ChairId")]
        public Chair Chair { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public byte? StatusNullable { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FullName
        {
            get { return EnterpriseNode.RealEnterpriseNode.FirstName + " " + EnterpriseNode.RealEnterpriseNode.LastName + " " + "(" + Username + ")"; }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstNameAndLastName
        {
            get { return EnterpriseNode.RealEnterpriseNode.FirstName + " " + EnterpriseNode.RealEnterpriseNode.LastName; }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string StatusTostring
        {
            get { return ((Enums.UserStatus)Status).GetDescriptionInLocalization(); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get
            {
                return
                  this.EnterpriseNode != null ? this.EnterpriseNode.DescriptionField : "";
            }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string Email
        {
            get
            {
                return
                  this.EnterpriseNode != null ? this.EnterpriseNode.Email : "";
            }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool ReservedItem { get; set; }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HasChild { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HasSuccedPayment { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Enums.UserChairStatus UserChairStatus { get; set; }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool RegisterInNewsLetter { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstName { get { return this.EnterpriseNode.RealEnterpriseNode.FirstName; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string LastName { get { return this.EnterpriseNode.RealEnterpriseNode.LastName; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string TitlePaymentType { get { return this.PaymentType != null ? this.PaymentType.Title : ""; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Int32? ChairRow { get { return this.Chair != null ? this.Chair.Row : (int?)null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Int32? ChairColumn { get { return this.Chair != null ? this.Chair.Column : (int?)null; } set { } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid? ChairTypeId { get { return this.Chair != null ? this.Chair.ChairTypeId : (Guid?)null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid? ChairHallId { get { return this.Chair != null ? this.Chair.HallId : (Guid?)null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public byte? ChairStatus { get { return this.Chair != null ? this.Chair.Status : (byte?)null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Int32? ChairNumber { get { return this.Chair != null ? this.Chair.Number : (int?)null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid? HasChildUser { get; set; }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid? PayerId { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string HallName { get { return this.Chair != null ? this.Chair.Hall.Name : null; } set { } }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string ChairTypeTitle { get { return (this.Chair != null && this.Chair.ChairType != null) ? this.Chair.ChairType.Title : null; } set { } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool Done { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Enums.HasValue HasOtherPayState { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool HasOtherPay { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string UserEmail
        {
            get { return this.EnterpriseNode.Email; }
            set { }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string UserPhone
        {
            get { return this.EnterpriseNode.Cellphone; }
            set { }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string UserGender
        {
            get
            {
                if (this.EnterpriseNode.RealEnterpriseNode.Gender != null)
                {
                    return bool.Parse(this.EnterpriseNode.RealEnterpriseNode.Gender.ToString())
                        ? Enums.Gender.Male.GetDescriptionInLocalization()
                        : Enums.Gender.Female.GetDescriptionInLocalization();
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        private string _culture;
        [IsNullable]
        [DbType("varchar(10)")]
        public string Culture
        {
            get { return _culture; }
            set { base.SetPropertyValue("Culture", value); }
        }



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Guid FormId { get; set; }

    }
}
