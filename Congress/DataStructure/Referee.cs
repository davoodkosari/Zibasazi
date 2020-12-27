using System;
using System.Collections.Generic;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Referee : DataStructureBase<Referee>
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
        [DbType("nvarchar(30)")]
        public string Username
        {
            get { return _username; }
            set { base.SetPropertyValue("Username", value); }
        }

        private string _password;
        [DbType("varchar(200)")]
        public string Password
        {
            get { return _password; }
            set { base.SetPropertyValue("Password", value); }
        }

        private bool _enabled;
        [DbType("bit")]
        public bool Enabled
        {
            get { return _enabled; }
            set { base.SetPropertyValue("Enabled", value); }
        }

        private bool _isSpecial;
        [DbType("bit")]
        public bool IsSpecial
        {
            get { return _isSpecial; }
            set { base.SetPropertyValue("IsSpecial", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FullName
        {
            get
            {
                if (EnterpriseNode != null)
                {
                    return
                       EnterpriseNode.RealEnterpriseNode.FirstName + " " + EnterpriseNode.RealEnterpriseNode.LastName + " " + "(" + Username + ")";

                }
                else
                {
                    return this.FirstName + " " + this.LastName;
                }
            }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstNameAndLastName
        {
            get { return EnterpriseNode.RealEnterpriseNode.FirstName + " " + EnterpriseNode.RealEnterpriseNode.LastName; }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.EnterpriseNode.DescriptionField; }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool SendInform { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string PasswordWithoutHash { get; set; }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int AllArticleCount
        { get; set; }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public int WaitForAnswerArticleCount
        { get; set; }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public List<RefereeCartable> AllArticle
        { get; set; }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FirstName
        { get { return this.EnterpriseNode.RealEnterpriseNode.FirstName; } set { } }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string LastName
        { get { return this.EnterpriseNode.RealEnterpriseNode.LastName; } set { } }
    }
}
