using System;
using Radyn.Framework;

namespace Radyn.Common.DataStructure
{
    [Serializable]
    [Schema("Common")]
    public sealed class Unit : DataStructureBase<Unit>
    {
        private Int32 _id;
        [Key(true)]
        [DbType("int")]
        public Int32 Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private string _title;
        [DbType("nvarchar(50)")]
        public string Title
        {
            get { return _title; }
            set { base.SetPropertyValue("Title", value); }
        }

        
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Title; }
        }
    }
}
