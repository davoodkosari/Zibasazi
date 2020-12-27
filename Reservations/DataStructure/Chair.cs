using System;
using Radyn.Framework;

namespace Radyn.Reservation.DataStructure
{
    [Serializable]
    [Schema("Reservation")]
    public sealed class Chair : DataStructureBase<Chair>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _hallId;
        [DbType("uniqueidentifier")]
        public Guid HallId
        {
            get
            {
                return this._hallId;
            }
            set
            {
                base.SetPropertyValue("HallId", value);
                if (Hall == null)
                    this.Hall = new Hall() { Id = value };
            }
        }
        [Assosiation(PropName = "HallId")]
        public Hall Hall { get; set; }

        private Int32 _number;
        [IsNullable]
        [DbType("int")]
        public Int32 Number
        {
            get { return _number; }
            set { base.SetPropertyValue("Number", value); }
        }

        private Guid? _chairTypeId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? ChairTypeId
        {
            get { return _chairTypeId; }
            set
            {
                base.SetPropertyValue("ChairTypeId", value);
                if (ChairType == null)
                    this.ChairType = value.HasValue ? new ChairType() { Id = value.Value } : null;
            }
        }
        [Assosiation(PropName = "ChairTypeId")]
        public ChairType ChairType { get; set; }

        private Int32 _row;
        [DbType("int")]
        public Int32 Row
        {
            get { return _row; }
            set { base.SetPropertyValue("Row", value); }
        }

        private Int32 _column;
        [DbType("int")]
        public Int32 Column
        {
            get { return _column; }
            set { base.SetPropertyValue("Column", value); }
        }

        private byte _status;
        [DbType("tinyint")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }

        private Guid? _ownerId;
        [DbType("uniqueidentifier")]
        public Guid? OwnerId
        {
            get
            {
                return this._ownerId;
            }
            set
            {
                base.SetPropertyValue("OwnerId", value);
                if(Owner == null) { 
                this.Owner = value.HasValue
                    ? new EnterpriseNode.DataStructure.EnterpriseNode() { Id = value.Value }
                    : null;
                }
            }
        }
        [Assosiation(PropName = "OwnerId")]
        public EnterpriseNode.DataStructure.EnterpriseNode Owner { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string CellValue { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string ColumValue { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.ChairType.Title; }
        }



    }
}
