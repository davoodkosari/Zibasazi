using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    [Track]
    [Description("کارتابل داوری")]
    public sealed class RefereeCartable : DataStructureBase<RefereeCartable>
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


        private Guid _refereeId;
        [DbType("uniqueidentifier")]
        [Layout(Caption = "داور")]
        public Guid RefereeId
        {
            get
            {
                return this._refereeId;
            }
            set
            {
                base.SetPropertyValue("RefereeId", value);
                if (Referee == null)
                    this.Referee = new Referee { Id = value };
            }
        }

        [Assosiation(PropName = "RefereeId")]
        public Referee Referee { get; set; }

        private Guid _articleId;
        [DbType("uniqueidentifier")]
        [TrackMaster(typeof(Article))]
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


        private bool _visited;
        [DbType("bit")]
        public bool Visited
        {
            get { return _visited; }
            set { base.SetPropertyValue("Visited", value); }
        }

        private bool _isActive;
        [DbType("bit")]
        [DisableAction(DisableTrack = true)]
        public bool IsActive
        {
            get { return _isActive; }
            set { base.SetPropertyValue("IsActive", value); }
        }

        private byte _status;
        [DbType("tinyint")]
        [Layout(Caption = "وضعیت")]
        public byte Status
        {
            get { return _status; }
            set { base.SetPropertyValue("Status", value); }
        }


        private double? _score;
        [IsNullable]
        [DbType("float")]
        [Layout(Caption = "امتیاز")]
        public double? Score
        {
            get { return _score; }
            set { base.SetPropertyValue("Score", value); }
        }

        private DateTime _insertDate;
        [DbType("smalldatetime")]
        [DisableAction(DisableTrack = true)]
        public DateTime InsertDate
        {
            get { return _insertDate; }
            set { base.SetPropertyValue("InsertDate", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return ""; }
        }
    }
}
