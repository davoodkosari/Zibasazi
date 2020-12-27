using System;
using System.Collections.Generic;
using System.Data;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Reservation.DataStructure
{
    [Serializable]
    [Schema("Reservation")]
    public sealed class Hall : DataStructureBase<Hall>
    {

        private Guid _id;
        [Key(false)]
        [Framework.DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }



        private int _length;
        [Framework.DbType("int")]
        public int Length
        {
            get { return _length; }
            set { base.SetPropertyValue("Length", value); }
        }

        private int _width;
        [Framework.DbType("int")]
        public int Width
        {
            get { return _width; }
            set { base.SetPropertyValue("Width", value); }
        }

        private Guid? _parentId;
        [Framework.DbType("uniqueidentifier")]
        public Guid? ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                base.SetPropertyValue("ParentId", value);
                if (ParentHall == null)
                    ParentHall = value.HasValue ? new Hall { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "ParentId")]
        public Hall ParentHall { get; set; }

        private bool _enabled;
        [Framework.DbType("bit")]
        public bool Enabled
        {
            get { return _enabled; }
            set { base.SetPropertyValue("Enabled", value); }
        }

        private bool _isExternal;
        [Framework.DbType("bit")]
        public bool IsExternal
        {
            get { return _isExternal; }
            set { base.SetPropertyValue("IsExternal", value); }
        }

        private Guid? _photoId;
        [IsNullable]
        [Framework.DbType("uniqueidentifier")]
        public Guid? PhotoId
        {
            get
            {
                return this._photoId;
            }
            set
            {
                base.SetPropertyValue("PhotoId", value);

            }
        }



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        [MultiLanguage]
        public string Name { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return Name; }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public List<Chair> Chairs { get; set; }
    }
}
