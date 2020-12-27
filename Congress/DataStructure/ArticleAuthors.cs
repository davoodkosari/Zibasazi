using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ArticleAuthors : DataStructureBase<ArticleAuthors>
    {
        private Guid _id;
        [Key(false)]
        [DbType("uniqueidentifier")]
        public Guid Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private Guid _articleId;
        [DbType("uniqueidentifier")]
        public Guid ArticleId
        {
            get
            {
                return this._articleId;
            }
            set
            {
                base.SetPropertyValue("ArticleId", value);
                if (Article == null)
                    this.Article = new Article { Id = value };
            }
        }

        [Assosiation(PropName = "ArticleId")]
        public Article Article { get; set; }

        private string _name;
        [DbType("nvarchar(200)")]
        public string Name
        {
            get { return _name; }
            set { base.SetPropertyValue("Name", value); }
        }

        private string _address;
        [IsNullable]
        [DbType("nvarchar(1000)")]
        public string Address
        {
            get { return _address; }
            set { base.SetPropertyValue("Address", value); }
        }

        private bool _isDirector;
        [DbType("bit")]
        public bool IsDirector
        {
            get { return _isDirector; }
            set { base.SetPropertyValue("IsDirector", value); }
        }
        private int _order;
        [DbType("int")]
        public int Order
        {
            get { return _order; }
            set { base.SetPropertyValue("Order", value); }
        }

        private byte _percentage;
        [DbType("tinyint")]
        public byte Percentage
        {
            get { return _percentage; }
            set { base.SetPropertyValue("Percentage", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Name; }
        }
    }
}
