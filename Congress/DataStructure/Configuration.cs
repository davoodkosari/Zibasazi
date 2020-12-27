using System;
using System.Collections.Generic;
using Radyn.Congress.Tools;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.DataStructure
{
    [Serializable]
    [Schema("Congress")]
    public sealed class Configuration : DataStructureBase<Configuration>
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

            }
        }

        [Assosiation(PropName = "CongressId", FillData = false)]
        public Homa Homa { get; set; }

        private bool _multiLanguageArticle;
        [Layout(Caption = "مقاله/ایده/اثر ها چند زبانه باشند؟")]
        [DbType("bit")]
        public bool MultiLanguageArticle
        {
            get { return _multiLanguageArticle; }
            set { base.SetPropertyValue("MultiLanguageArticle", value); }
        }

        private bool _getAbsrtact;
        [Layout(Caption = "چکیده دریافت می شود؟")]
        [DbType("bit")]
        public bool GetAbsrtact
        {
            get { return _getAbsrtact; }
            set { base.SetPropertyValue("GetAbsrtact", value); }
        }

        private bool _getOrginal;
        [Layout(Caption = "اصل دریافت می شود؟")]
        [DbType("bit")]
        public bool GetOrginal
        {
            get { return _getOrginal; }
            set { base.SetPropertyValue("GetOrginal", value); }
        }

        private bool _getAbstractFile;
        [Layout(Caption = "چکیده به صورت فایل دریافت می شود یا متن؟")]
        [DbType("bit")]
        public bool GetAbstractFile
        {
            get { return _getAbstractFile; }
            set { base.SetPropertyValue("GetAbstractFile", value); }
        }

        private bool _getArticleOrginalFile;
        [Layout(Caption = "اصل  به صورت فایل دریافت می شود یا متن؟")]
        [DbType("bit")]
        public bool GetArticleOrginalFile
        {
            get { return _getArticleOrginalFile; }
            set { base.SetPropertyValue("GetArticleOrginalFile", value); }
        }

        private bool _enableArticlePercentage;
        [DbType("bit")]
        public bool EnableArticlePercentage
        {
            get { return _enableArticlePercentage; }
            set { base.SetPropertyValue("EnableArticlePercentage", value); }
        }

        private bool _isScrollSupporter;
        [DbType("bit")]
        public bool IsScrollSupporter
        {
            get { return _isScrollSupporter; }
            set { base.SetPropertyValue("IsScrollSupporter", value); }
        }

        private bool _registerForReservBooth;
        [DbType("bit")]
        public bool RegisterForReservBooth
        {
            get { return _registerForReservBooth; }
            set { base.SetPropertyValue("RegisterForReservBooth", value); }
        }

        private bool _isScrollVIP;
        [DbType("bit")]
        public bool IsScrollVIP
        {
            get { return _isScrollVIP; }
            set { base.SetPropertyValue("IsScrollVIP", value); }
        }
        private bool _hasBooth;
        [DbType("bit")]
        public bool HasBooth
        {
            get { return _hasBooth; }
            set { base.SetPropertyValue("HasBooth", value); }
        }

        private bool _canUserSelectChairs;
        [DbType("bit")]
        public bool CanUserSelectChairs
        {
            get { return _canUserSelectChairs; }
            set { base.SetPropertyValue("CanUserSelectChairs", value); }
        }

        private bool _hasHotel;
        [DbType("bit")]
        public bool HasHotel
        {
            get { return _hasHotel; }
            set { base.SetPropertyValue("HasHotel", value); }
        }

        private bool _hasWorkShop;
        [DbType("bit")]
        public bool HasWorkShop
        {
            get { return _hasWorkShop; }
            set { base.SetPropertyValue("HasWorkShop", value); }
        }

        private bool _hasArticle;
        [DbType("bit")]
        public bool HasArticle
        {
            get { return _hasArticle; }
            set { base.SetPropertyValue("HasArticle", value); }
        }
        private bool _hasArticlePayment;
        [DbType("bit")]
        public bool HasArticlePayment
        {
            get { return _hasArticlePayment; }
            set { base.SetPropertyValue("HasArticlePayment", value); }
        }
        private bool _hasUserPayment;
        [DbType("bit")]
        public bool HasUserPayment
        {
            get { return _hasUserPayment; }
            set { base.SetPropertyValue("HasUserPayment", value); }
        }
        private Int16? _abstractFileSize;
        [Layout(Caption = "حداکثر اندازه قایل چکیده چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? AbstractFileSize
        {
            get { return _abstractFileSize; }
            set { base.SetPropertyValue("AbstractFileSize", value); }
        }

        private Int16? _articleOrginalFileSize;
        [Layout(Caption = "حداکثر اندازه قایل اصل  چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? ArticleOrginalFileSize
        {
            get { return _articleOrginalFileSize; }
            set { base.SetPropertyValue("ArticleOrginalFileSize", value); }
        }


        private Int16? _abstractWordCount;
        [Layout(Caption = "حداکثر تعداد کلمات چکیده چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? AbstractWordCount
        {
            get { return _abstractWordCount; }
            set { base.SetPropertyValue("AbstractWordCount", value); }
        }

        private Int16? _minAbstractWordCount;
        [Layout(Caption = "حداقل تعداد کلمات چکیده چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? MinAbstractWordCount
        {
            get { return _minAbstractWordCount; }
            set { base.SetPropertyValue("MinAbstractWordCount", value); }
        }



        private Int16? _articleOrginalWordCount;
        [Layout(Caption = "حداکثر تعداد کلمات اصل  چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? ArticleOrginalWordCount
        {
            get { return _articleOrginalWordCount; }
            set { base.SetPropertyValue("ArticleOrginalWordCount", value); }
        }

        private Int16? _minArticleOrginalWordCount;
        [Layout(Caption = "حداقل تعداد کلمات اصل  چقدر باشد؟")]
        [IsNullable]
        [DbType("smallint")]
        public Int16? MinArticleOrginalWordCount
        {
            get { return _minArticleOrginalWordCount; }
            set { base.SetPropertyValue("MinArticleOrginalWordCount", value); }
        }




        private Int16? _maxArticleCountShow;
        [IsNullable]
        [DbType("smallint")]
        public Int16? MaxArticleCountShow
        {
            get { return _maxArticleCountShow; }
            set { base.SetPropertyValue("MaxArticleCountShow", value); }
        }

        private Int16? _maxNewsCountShow;
        [IsNullable]
        [DbType("smallint")]
        public Int16? MaxNewsCountShow
        {
            get { return _maxNewsCountShow; }
            set { base.SetPropertyValue("MaxNewsCountShow", value); }
        }


        private string _abstractStartDate;
        [Layout(Caption = "تاریخ شروع دریافت چکیده")]
        [IsNullable]
        [DbType("char(10)")]
        public string AbstractStartDate
        {
            get { return _abstractStartDate; }
            set { base.SetPropertyValue("AbstractStartDate", value); }
        }

        private string _abstractFinishDate;
        [Layout(Caption = "تاریخ پایان دریافت چکیده")]
        [IsNullable]
        [DbType("char(10)")]
        public string AbstractFinishDate
        {
            get { return _abstractFinishDate; }
            set { base.SetPropertyValue("AbstractFinishDate", value); }
        }






        private string _orginalStartDate;
        [Layout(Caption = "تاریخ شروع دریافت اصل ")]
        [IsNullable]
        [DbType("char(10)")]
        public string OrginalStartDate
        {
            get { return _orginalStartDate; }
            set { base.SetPropertyValue("OrginalStartDate", value); }
        }

        private string _orginalFinishDate;
        [Layout(Caption = "تاریخ پایان دریافت اصل ")]
        [IsNullable]
        [DbType("char(10)")]
        public string OrginalFinishDate
        {
            get { return _orginalFinishDate; }
            set { base.SetPropertyValue("OrginalFinishDate", value); }
        }

        private bool _canUserDeleteArticle;
        [Layout(Caption = "کاربر مجاز به حذف  می باشد؟")]
        [DbType("bit")]
        public bool CanUserDeleteArticle
        {
            get { return _canUserDeleteArticle; }
            set { base.SetPropertyValue("CanUserDeleteArticle", value); }
        }
        private string _merchantId;
        [IsNullable]
        [DbType("varchar(100)")]
        public string MerchantId
        {
            get { return _merchantId; }
            set { base.SetPropertyValue("MerchantId", value); }
        }
        private int _terminalId;
        [IsNullable]
        [DbType("int")]
        public int TerminalId
        {
            get { return _terminalId; }
            set { base.SetPropertyValue("TerminalId", value); }
        }
        private string _merchantPublicKey;
        [IsNullable]
        [DbType("varchar(MAX)")]
        public string MerchantPublicKey
        {
            get { return _merchantPublicKey; }
            set { base.SetPropertyValue("MerchantPublicKey", value); }
        }
        private string _merchantPrivateKey;
        [IsNullable]
        [DbType("varchar(MAX)")]
        public string MerchantPrivateKey
        {
            get { return _merchantPrivateKey; }
            set { base.SetPropertyValue("MerchantPrivateKey", value); }
        }
        private string _terminalUserName;
        [IsNullable]
        [DbType("varchar(20)")]
        public string TerminalUserName
        {
            get { return _terminalUserName; }
            set { base.SetPropertyValue("TerminalUserName", value); }
        }
        private string _terminalPassword;
        [IsNullable]
        [DbType("varchar(4000)")]
        public string TerminalPassword
        {
            get { return _terminalPassword; }
            set { base.SetPropertyValue("TerminalPassword", value); }
        }
        private string _certificatePath;
        [IsNullable]
        [DbType("varchar(200)")]
        public string CertificatePath
        {
            get { return _certificatePath; }
            set { base.SetPropertyValue("CertificatePath", value); }
        }
        private string _certificatePassword;
        [IsNullable]
        [DbType("varchar(200)")]
        public string CertificatePassword
        {
            get { return _certificatePassword; }
            set { base.SetPropertyValue("CertificatePassword", value); }
        }
        private byte? _bankId;
        [DbType("tinyint")]
        [IsNullable]
        public byte? BankId
        {
            get { return _bankId; }
            set { base.SetPropertyValue("BankId", value); }
        }

        private int _sMSAccountId;
        [IsNullable]
        [DbType("int")]
        public int SMSAccountId
        {
            get { return _sMSAccountId; }
            set { base.SetPropertyValue("SMSAccountId", value); }
        }
        private string _sMSAccountUserName;
        [IsNullable]
        [DbType("varchar(20)")]
        public string SMSAccountUserName
        {
            get { return _sMSAccountUserName; }
            set { base.SetPropertyValue("SMSAccountUserName", value); }
        }
        private string _sMSAccountPassword;
        [IsNullable]
        [DbType("varchar(150)")]
        public string SMSAccountPassword
        {
            get { return _sMSAccountPassword; }
            set { base.SetPropertyValue("SMSAccountPassword", value); }
        }

        private bool _attachReferee;
        [Layout(Caption = "فرم داوری پیوست شود؟")]
        [DbType("bit")]
        public bool AttachReferee
        {
            get { return _attachReferee; }
            set { base.SetPropertyValue("AttachReferee", value); }
        }

        private Int16 _disscountCount;
        [Layout(Caption = "چند تخفیف داشته باشد")]
        [DbType("smallint")]
        public Int16 DisscountCount
        {
            get { return _disscountCount; }
            set { base.SetPropertyValue("DisscountCount", value); }
        }

        private string _workShopRezervStartDate;
        [Layout(Caption = "تاریخ شروع رزرو کارگاه")]
        [IsNullable]
        [DbType("char(10)")]
        public string WorkShopRezervStartDate
        {
            get { return _workShopRezervStartDate; }
            set { base.SetPropertyValue("WorkShopRezervStartDate", value); }
        }

        private string _workShopRezervEndDate;
        [Layout(Caption = "تاریخ پایان رزرو کارگاه")]
        [IsNullable]
        [DbType("char(10)")]
        public string WorkShopRezervEndDate
        {
            get { return _workShopRezervEndDate; }
            set { base.SetPropertyValue("WorkShopRezervEndDate", value); }
        }

        private Int16 _maxHotelPerUser;
        [Layout(Caption = "تعداد مجاز رزرو اسکان برای هر کاربر")]
        [DbType("smallint")]
        public Int16 MaxHotelPerUser
        {
            get { return _maxHotelPerUser; }
            set { base.SetPropertyValue("MaxHotelPerUser", value); }
        }

        private string _hotelRezervStartDate;
        [Layout(Caption = "تاریخ شروع رزرو هتل")]
        [IsNullable]
        [DbType("char(10)")]
        public string HotelRezervStartDate
        {
            get { return _hotelRezervStartDate; }
            set { base.SetPropertyValue("HotelRezervStartDate", value); }
        }

        private string _hotelRezervEndDate;
        [Layout(Caption = "تاریخ پایان رزرو غرفه")]
        [IsNullable]
        [DbType("char(10)")]
        public string HotelRezervEndDate
        {
            get { return _hotelRezervEndDate; }
            set { base.SetPropertyValue("HotelRezervEndDate", value); }
        }

        private string _boothRezervStartDate;
        [Layout(Caption = "تاریخ شروع رزرو غرفه")]
        [IsNullable]
        [DbType("char(10)")]
        public string BoothRezervStartDate
        {
            get { return _boothRezervStartDate; }
            set { base.SetPropertyValue("BoothRezervStartDate", value); }
        }

        private string _boothRezervEndDate;
        [Layout(Caption = "تاریخ پایان رزرو غرفه")]
        [IsNullable]
        [DbType("char(10)")]
        public string BoothRezervEndDate
        {
            get { return _boothRezervEndDate; }
            set { base.SetPropertyValue("BoothRezervEndDate", value); }
        }

        private Int16 _maxArticlePerUser;
        [Layout(Caption = "تعداد مقالات مجاز هر کاربر")]
        [DbType("smallint")]
        public Int16 MaxArticlePerUser
        {
            get { return _maxArticlePerUser; }
            set { base.SetPropertyValue("MaxArticlePerUser", value); }
        }

        private Int16 _maxBoothPerUser;
        [DbType("smallint")]
        public Int16 MaxBoothPerUser
        {
            get { return _maxBoothPerUser; }
            set { base.SetPropertyValue("MaxBoothPerUser", value); }
        }

        private Int16 _maxWorkShopPerUser;
        [Layout(Caption = "تعداد مجاز ثبت نام هر کاربر در کارگاه")]
        [DbType("smallint")]
        public Int16 MaxWorkShopPerUser
        {
            get { return _maxWorkShopPerUser; }
            set { base.SetPropertyValue("MaxWorkShopPerUser", value); }
        }
        private Int16 _hotelReservDailyCount;
        [DbType("smallint")]
        public Int16 HotelReservDailyCount
        {
            get { return _hotelReservDailyCount; }
            set { base.SetPropertyValue("HotelReservDailyCount", value); }
        }

        private string _paymentType;
        [DbType("varchar(20)")]
        [IsNullable]
        public string PaymentType
        {
            get { return _paymentType; }
            set { base.SetPropertyValue("PaymentType", value); }
        }

        private byte? _userWorkReserveShopInformType;
        [DbType("tinyint")]
        [IsNullable]
        public byte? UserWorkReserveShopInformType
        {
            get { return _userWorkReserveShopInformType; }
            set { base.SetPropertyValue("UserWorkReserveShopInformType", value); }
        }
        private byte? _userHotelReserveInformType;
        [DbType("tinyint")]
        [IsNullable]
        public byte? UserHotelReserveInformType
        {
            get { return _userHotelReserveInformType; }
            set { base.SetPropertyValue("UserHotelReserveInformType", value); }
        }
        private byte? _userRegisterInformType;
        [DbType("tinyint")]
        public byte? UserRegisterInformType
        {
            get { return _userRegisterInformType; }
            set { base.SetPropertyValue("UserRegisterInformType", value); }
        }
        private byte? _articleInformType;
        [DbType("tinyint")]
        public byte? ArticleInformType
        {
            get { return _articleInformType; }
            set { base.SetPropertyValue("ArticleInformType", value); }
        }


        private byte? _boothReserveInformType;
        [DbType("tinyint")]
        [IsNullable]
        public byte? BoothReserveInformType
        {
            get { return _boothReserveInformType; }
            set { base.SetPropertyValue("BoothReserveInformType", value); }
        }
        private byte? _articlePaymentStep;
        [DbType("tinyint")]
        [IsNullable]
        public byte? ArticlePaymentStep
        {
            get { return _articlePaymentStep; }
            set { base.SetPropertyValue("ArticlePaymentStep", value); }
        }
        private string _theme;
        [Layout(Caption = "تاریخ شروع دریافت اصل مقالات")]
        [IsNullable]
        [DbType("nvarchar(100)")]
        public string Theme
        {
            get { return _theme; }
            set { base.SetPropertyValue("Theme", value); }
        }
        private string _cardLanguageId;
        [DbType("char(5)")]
        public string CardLanguageId
        {
            get { return _cardLanguageId; }
            set { base.SetPropertyValue("CardLanguageId", value); }
        }
        private bool _allowUserPrintCard;
        [DbType("bit")]
        public bool AllowUserPrintCard
        {
            get { return _allowUserPrintCard; }
            set { base.SetPropertyValue("AllowUserPrintCard", value); }
        }

        private bool _allowUserPrintCertification;
        [DbType("bit")]
        public bool AllowUserPrintCertification
        {
            get { return _allowUserPrintCertification; }
            set { base.SetPropertyValue("AllowUserPrintCertification", value); }
        }
        
        
        private bool _allowUserAddAuthor;
        [DbType("bit")]
        public bool AllowUserAddAuthor
        {
            get { return _allowUserAddAuthor; }
            set { base.SetPropertyValue("AllowUserAddAuthor", value); }
        }

        private bool _registerEmailConfirm;
        [DbType("bit")]
        public bool RegisterEmailConfirm
        {
            get { return _registerEmailConfirm; }
            set { base.SetPropertyValue("RegisterEmailConfirm", value); }
        }
        private byte _articleCertificateTypeId;
        [DbType("tinyint")]
        public byte ArticleCertificateTypeId
        {
            get { return _articleCertificateTypeId; }
            set { base.SetPropertyValue("ArticleCertificateTypeId", value); }
        }

        private string _congressCode;
        [DbType("nvarchar(50)")]
        public string CongressCode
        {
            get { return _congressCode; }
            set { base.SetPropertyValue("CongressCode", value); }
        }

        private short? _bigSlideId;

        [DbType("smallint")]
        [IsNullable]
        public short? BigSlideId
        {
            get { return this._bigSlideId; }
            set { base.SetPropertyValue("BigSlideId", value); }
        }

        private short? _miniSlideId;
        [DbType("smallint")]
        [IsNullable]
        public short? MiniSlideId
        {
            get { return this._miniSlideId; }
            set { base.SetPropertyValue("MiniSlideId", value); }
        }

        private short? _averageSlideId;
        [DbType("smallint")]
        [IsNullable]
        public short? AverageSlideId
        {
            get { return this._averageSlideId; }
            set { base.SetPropertyValue("AverageSlideId", value); }
        }

        private short _groupEmailInterval;
        [DbType("smallint")]
        [IsNullable]
        public short GroupEmailInterval
        {
            get { return this._groupEmailInterval; }
            set { base.SetPropertyValue("GroupEmailInterval", value); }
        }

        private string _mailHost;
        [DbType("varchar(50)")]
        public string MailHost
        {
            get { return _mailHost; }
            set { base.SetPropertyValue("MailHost", value); }
        }
        private string _mailPassword;
        [DbType("varchar(100)")]
        public string MailPassword
        {
            get { return _mailPassword; }
            set { base.SetPropertyValue("MailPassword", value); }
        }

        private string _mailUserName;
        [DbType("varchar(50)")]
        public string MailUserName
        {
            get { return _mailUserName; }
            set { base.SetPropertyValue("MailUserName", value); }
        }

        private string _mailFrom;
        [DbType("varchar(50)")]
        public string MailFrom
        {
            get { return _mailFrom; }
            set { base.SetPropertyValue("MailFrom", value); }
        }

        private short _mailPort;
        [DbType("smallint")]
        public short MailPort
        {
            get { return _mailPort; }
            set { base.SetPropertyValue("MailPort", value); }
        }
        private bool _enableSSL;
        [DbType("bit")]
        public bool EnableSSL
        {
            get { return _enableSSL; }
            set { base.SetPropertyValue("EnableSSL", value); }
        }
        private int? _introPageId;
        [IsNullable]
        [DbType("int")]
        public int? IntroPageId
        {
            get { return _introPageId; }
            set { base.SetPropertyValue("IntroPageId", value); }
        }
        private bool _autoAssigneArticleToReferee;
        [DbType("bit")]
        public bool AutoAssigneArticleToReferee
        {
            get { return _autoAssigneArticleToReferee; }
            set { base.SetPropertyValue("AutoAssigneArticleToReferee", value); }
        }

        private byte? _refereeInformType;
        [DbType("tinyint")]
        public byte? RefereeInformType
        {
            get { return _refereeInformType; }
            set { base.SetPropertyValue("RefereeInformType", value); }
        }





        private bool _registerGroupEnable;
        [DbType("bit")]
        public bool RegisterGroupEnable
        {
            get { return _registerGroupEnable; }
            set { base.SetPropertyValue("RegisterGroupEnable", value); }
        }
        private bool _allowSentOrginalWhileAbstractDeny;
        [DbType("bit")]
        public bool AllowSentOrginalWhileAbstractDeny
        {
            get { return _allowSentOrginalWhileAbstractDeny; }
            set { base.SetPropertyValue("AllowSentOrginalWhileAbstractDeny", value); }
        }
        private int? _dayCountDeleteWorkShopReserveNotPay;
        [IsNullable]
        [DbType("int")]
        public int? DayCountDeleteWorkShopReserveNotPay
        {
            get { return _dayCountDeleteWorkShopReserveNotPay; }
            set { base.SetPropertyValue("DayCountDeleteWorkShopReserveNotPay", value); }
        }
        private int? _dayCountDeleteHotelReserveNotPay;
        [IsNullable]
        [DbType("int")]
        public int? DayCountDeleteHotelReserveNotPay
        {
            get { return _dayCountDeleteHotelReserveNotPay; }
            set { base.SetPropertyValue("DayCountDeleteHotelReserveNotPay", value); }
        }
        private int? _dayCountDeleteBoothReserveNotPay;
        [IsNullable]
        [DbType("int")]
        public int? DayCountDeleteBoothReserveNotPay
        {
            get { return _dayCountDeleteBoothReserveNotPay; }
            set { base.SetPropertyValue("DayCountDeleteBoothReserveNotPay", value); }
        }

        private bool _canUserReserveHotelWithoutPaymentType;
        [DbType("bit")]
        public bool CanUserReserveHotelWithoutPaymentType
        {
            get { return _canUserReserveHotelWithoutPaymentType; }
            set { base.SetPropertyValue("CanUserReserveHotelWithoutPaymentType", value); }
        }

        private bool _canUserReserveWorkShopWithoutPaymentType;
        [DbType("bit")]
        public bool CanUserReserveWorkShopWithoutPaymentType
        {
            get { return _canUserReserveWorkShopWithoutPaymentType; }
            set { base.SetPropertyValue("CanUserReserveWorkShopWithoutPaymentType", value); }
        }

        private bool _canUserReserveBoothWithoutPaymentType;
        [DbType("bit")]
        public bool CanUserReserveBoothWithoutPaymentType
        {
            get { return _canUserReserveBoothWithoutPaymentType; }
            set { base.SetPropertyValue("CanUserReserveBoothWithoutPaymentType", value); }
        }

        private bool _canUserSendArticleWithoutPaymentType;
        [DbType("bit")]
        public bool CanUserSendArticleWithoutPaymentType
        {
            get { return _canUserSendArticleWithoutPaymentType; }
            set { base.SetPropertyValue("CanUserSendArticleWithoutPaymentType", value); }
        }


        private string _backgroundColor;
        [DbType("varchar(6)")]
        [IsNullable]
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { base.SetPropertyValue("BackgroundColor", value); }
        }

        private Guid? _backgroundImage;
        [DbType("uniqueidentifier")]
        [IsNullable]
        public Guid? BackgroundImage
        {
            get { return _backgroundImage; }
            set { base.SetPropertyValue("BackgroundImage", value); }
        }


        private bool _sentArticleSpecialReferee;
        [DbType("bit")]
        public bool SentArticleSpecialReferee
        {
            get { return _sentArticleSpecialReferee; }
            set { base.SetPropertyValue("SentArticleSpecialReferee", value); }
        }

        private bool? _shoudBeUserPaymentApprovedForUserCardPrint;
        [IsNullable]
        [DbType("bit")]
        public bool? ShoudBeUserPaymentApprovedForUserCardPrint
        {
            get { return _shoudBeUserPaymentApprovedForUserCardPrint; }
            set { base.SetPropertyValue("ShoudBeUserPaymentApprovedForUserCardPrint", value); }
        }

        private string _webStatistics;
        [DbType("nvarchar(max)")]
        public string WebStatistics
        {
            get { return _webStatistics; }
            set { base.SetPropertyValue("WebStatistics", value); }
        }

        private bool _backgroundIsImage;

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public bool BackgroundIsImage
        {
            get
            {
                if (BackgroundImage.HasValue && string.IsNullOrEmpty(BackgroundColor)) return true;
                return _backgroundIsImage;
            }
            set { _backgroundIsImage = value; }
        }


        [MultiLanguage]
        public string CartTypeEmptyValue { get; set; }

        private string _articleTitle;
        [MultiLanguage]
        public string ArticleTitle
        {
            get { return string.IsNullOrEmpty(_articleTitle) ? Resources.Congress.Article : _articleTitle; }
            set { _articleTitle = value; }
        }

 private string _articleOrginalTitle;
        [MultiLanguage]
        public string ArticleOrginalTitle
        {
            get { return string.IsNullOrEmpty(_articleOrginalTitle) ? Resources.Congress.ArticleText : _articleOrginalTitle; }
            set { _articleOrginalTitle = value; }
        }

        public Dictionary<string, ConfigurationContent> _configurationContent;

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public Dictionary<string, ConfigurationContent> ConfigurationContent
        {
            get
            {
                return _configurationContent ?? (_configurationContent = new Dictionary<string, ConfigurationContent>());
            }
            set { _configurationContent = value; }
        }


        private Guid? _defaultContrainerId;
        [IsNullable]
        [DbType("uniqueidentifier")]
        public Guid? DefaultContrainerId
        {
            get
            {
                return this._defaultContrainerId;
            }
            set
            {
                base.SetPropertyValue("DefaultContrainerId", value);
                if (Container == null)
                    this.Container = value.HasValue ? new Container { Id = value.Value } : null;
            }
        }

        [Layout(Caption = "قالب پیش فرض")]
        [Assosiation(PropName = "DefaultContrainerId", FillAnyway = true)]
        public Container Container { get; set; }




        private Guid? _defaultHtmlId;
        [DbType("uniqueidentifier")]
        public Guid? DefaultHtmlId
        {
            get
            {
                return this._defaultHtmlId;
            }
            set
            {
                base.SetPropertyValue("DefaultHtmlId", value);
                if (HtmlDesgin == null)
                    this.HtmlDesgin = value.HasValue ? new HtmlDesgin { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "DefaultHtmlId",FillAnyway = true)]
        public HtmlDesgin HtmlDesgin { get; set; }


        private Guid? _defaultMenuHtmlId;
        [DbType("uniqueidentifier")]
        public Guid? DefaultMenuHtmlId
        {
            get
            {
                return this._defaultMenuHtmlId;
            }
            set
            {
                base.SetPropertyValue("DefaultMenuHtmlId", value);
                if (MenuHtml == null)
                    this.MenuHtml = value.HasValue ? new MenuHtml { Id = value.Value } : null;
            }
        }

        [Assosiation(PropName = "DefaultMenuHtmlId", FillAnyway = true)]
        public MenuHtml MenuHtml { get; set; }

        private bool _accessToUserImportFromExcel;
        [DbType("bit")]
        public bool AccessToUserImportFromExcel
        {
            get { return _accessToUserImportFromExcel; }
            set { base.SetPropertyValue("AccessToUserImportFromExcel", value); }
        }


        private bool _hasFinancialOperation;
        [DbType("bit")]
        public bool HasFinancialOperation
        {
            get { return _hasFinancialOperation; }
            set { base.SetPropertyValue("HasFinancialOperation", value); }
        }


        private bool _hasUserForms;
        [DbType("bit")]
        public bool HasUserForms
        {
            get { return _hasUserForms; }
            set { base.SetPropertyValue("HasUserForms", value); }
        }


        private Guid? _favIcon;

        [DbType("uniqueidentifier")]
        [IsNullable]
        public Guid? FavIcon
        {
            get { return _favIcon; }
            set
            {
                base.SetPropertyValue("FavIcon", value);
            }
        }

        private string _themeColorURL;

        [DbType("nvarchar(200)")]
        [IsNullable]
        public string ThemeColorURL
        {
            get { return _themeColorURL; }
            set
            {
                base.SetPropertyValue("ThemeColorURL", value);
            }
        }

        private bool _requireArticleTypeForCertificate;
        [DbType("bit")]
        public bool RequireArticleTypeForCertificate
        {
            get { return _requireArticleTypeForCertificate; }
            set { base.SetPropertyValue("RequireArticleTypeForCertificate", value); }
        }

        private byte? _articleCertificateState;
        [DbType("tinyint")]
        public byte? ArticleCertificateState
        {
            get { return _articleCertificateState; }
            set { base.SetPropertyValue("ArticleCertificateState", value); }
        }

        private bool _enableArticleComment;
        [DbType("bit")]
        public bool EnableArticleComment
        {
            get { return _enableArticleComment; }
            set { base.SetPropertyValue("EnableArticleComment", value); }
        }


        private bool _enabledArticleKeyword;
        [DbType("bit")]
        public bool EnabledArticleKeyword
        {
            get { return _enabledArticleKeyword; }
            set { base.SetPropertyValue("EnabledArticleKeyword", value); }
        }
        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public override string DescriptionField
        {
            get { return this.Homa.DescriptionField; }
        }
    }
}
