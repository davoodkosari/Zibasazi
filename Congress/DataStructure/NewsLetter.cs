using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class NewsLetter : DataStructureBase<NewsLetter>
    {


        private Guid _congressId;
        [DbType("uniqueidentifier")]
        [Key(false)]
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


        private string _email;
        [DbType("nvarchar(100)")]
        [Key(false)]
        public string Email
        {
            get { return _email; }
            set { base.SetPropertyValue("Email", value); }
        }



        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Email; }
        }

    }
}
