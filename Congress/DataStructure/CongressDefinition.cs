using System;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class CongressDefinition : DataStructureBase<CongressDefinition>
    {
        private Guid _congressId;
        [Key(false)]
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

        [Assosiation(PropName = "CongressId")]
        public Homa Homa { get; set; }

        private Guid? _rptUserCardId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptUserCardId
        {
            get { return _rptUserCardId; }
            set { base.SetPropertyValue("RptUserCardId", value); }
        }

        private Guid? _rptCongressCertificateId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptCongressCertificateId
        {
            get { return _rptCongressCertificateId; }
            set { base.SetPropertyValue("RptCongressCertificateId", value); }
        }

        private Guid? _rptArticleCertificateId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptArticleCertificateId
        {
            get { return _rptArticleCertificateId; }
            set { base.SetPropertyValue("RptArticleCertificateId", value); }
        }
        private Guid? _rptBoothOfficerId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptBoothOfficerId
        {
            get { return _rptBoothOfficerId; }
            set { base.SetPropertyValue("RptBoothOfficerId", value); }
        }

        private Guid? _rptArticleId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptArticleId
        {
            get { return _rptArticleId; }
            set { base.SetPropertyValue("RptArticleId", value); }
        }

        private Guid? _rptUserId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptUserId
        {
            get { return _rptUserId; }
            set { base.SetPropertyValue("RptUserId", value); }
        }

        private Guid? _rptWorkShopUserId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptWorkShopUserId
        {
            get { return _rptWorkShopUserId; }
            set { base.SetPropertyValue("RptWorkShopUserId", value); }
        }

        private Guid? _rptHotelUserId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptHotelUserId
        {
            get { return _rptHotelUserId; }
            set { base.SetPropertyValue("RptHotelUserId", value); }
        }

        private Guid? _rptUserBoothId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptUserBoothId
        {
            get { return _rptUserBoothId; }
            set { base.SetPropertyValue("RptUserBoothId", value); }
        }
        private Guid? _rptChipFoodId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptChipFoodId
        {
            get { return _rptChipFoodId; }
            set { base.SetPropertyValue("RptChipFoodId", value); }
        }



        private Guid? _rptMiniUserCardId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptMiniUserCardId
        {
            get { return _rptMiniUserCardId; }
            set { base.SetPropertyValue("RptMiniUserCardId", value); }
        }

        private Guid? _rptAbstractArticleId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptAbstractArticleId
        {
            get { return _rptAbstractArticleId; }
            set { base.SetPropertyValue("RptAbstractArticleId", value); }
        }

        private Guid? _rptUserInfoCardId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? RptUserInfoCardId
        {
            get { return _rptUserInfoCardId; }
            set { base.SetPropertyValue("RptUserInfoCardId", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Homa.CongressTitle; }
        }
    }
}
