using System;
using Radyn.Framework;
namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class ArticleUserComment : DataStructureBase<ArticleUserComment>
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
        [DbType("nvarchar(150)")]

        [IsNullable]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                base.SetPropertyValue("Name", value);
            }
        }

      


        private string _description;
        [DbType("nvarchar(Max)")]
        [IsNullable]
        public string Description
        {
            get { return _description; }
            set { base.SetPropertyValue("Description", value); }
        }

        private string _saveDate;
        [DbType("char(10)")]
        public string SaveDate
        {
            get { return _saveDate; }
            set { base.SetPropertyValue("SaveDate", value); }
        }

        private string _saveTime;
        [DbType("char(5)")]
        public string SaveTime
        {
            get { return _saveDate; }
            set { base.SetPropertyValue("SaveTime", value); }
        }

        [Layout(Caption = "تایید مدیر")]
        [DbType("bit")]
        public bool ConfirmAdmin { get; set; }


        [Layout(Caption = "پسند")]
        [DbType("bit")]
        public bool IsLike { get; set; }

        private string _iP;
        [DbType("varchar(15)")]
        public string IP
        {
            get { return _iP; }
            set { base.SetPropertyValue("IP", value); }
        }


        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField { get; }
    }
}
