using System;
using System.Web;
using Radyn.EnterpriseNode.Tools;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class BoothOfficer : DataStructureBase<BoothOfficer>
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

            }
        }

        [Assosiation(PropName = "Id")]
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
                if (UserEnterpriseNode == null)
                    this.UserEnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode { Id = value };
            }
        }

        [Assosiation(PropName = "UserId")]
        public EnterpriseNode.DataStructure.EnterpriseNode UserEnterpriseNode { get; set; }

        private Int32 _order;
        [DbType("int")]
        public Int32 Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.EnterpriseNode.Title(); }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public HttpPostedFileBase AttachFile { get; set; }
    }
}
