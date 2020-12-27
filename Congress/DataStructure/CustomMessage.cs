using Radyn.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CustomMessage : DataStructureBase<CustomMessage>
    {
        private Int32 _id;
        [Key(true)]
        [DbType("int")]
        public Int32 Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private string _emailText;
        [DbType("nvarchar(Max)")]
        [MultiLanguage]
        public string EmailText
        {
            get { return _emailText; }
            set { base.SetPropertyValue("EmailText", value); }
        }
        private string _smsText;
        [DbType("nvarchar(Max)")]
        [MultiLanguage]
        public string SmsText
        {
            get { return _smsText; }
            set { base.SetPropertyValue("SmsText", value); }
        }

      


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


        private MessageInformType _type;
        [DbType("tinyint")]
        public MessageInformType Type
        {
            get { return _type; }
            set { base.SetPropertyValue("Type", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField { get; }
    }
}
