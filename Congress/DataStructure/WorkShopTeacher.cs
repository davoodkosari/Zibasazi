using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class WorkShopTeacher : DataStructureBase<WorkShopTeacher>
    {
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

        private Guid _teacherId;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid TeacherId
        {
            get
            {
                return this._teacherId;
            }
            set
            {
                base.SetPropertyValue("TeacherId", value);
                if (Teacher == null)
                    this.Teacher = new Teacher { Id = value };
            }
        }

        [Assosiation(PropName = "TeacherId")]
        public Teacher Teacher { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Teacher.DescriptionField; }
        }
    }
}
