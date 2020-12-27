using Radyn.Framework;

namespace Radyn.Congress.Tools
{
    public class Enums
    {
        public enum ResourceType
        {
            [Description("JSFile", Type = typeof(Resources.Congress))]
            JSFile,
            [Description("CssFile", Type = typeof(Resources.Congress))]
            CssFile,
            [Description("JSFunction", Type = typeof(Resources.Congress))]
            JSFunction,
            [Description("Style", Type = typeof(Resources.Congress))]
            Style,
        }

        public enum UseLayout : byte
        {
            None = 0,
            [Description("Layout", Type = typeof(Resources.Congress))]
            Layout = 1,
            [Description("UserLayout", Type = typeof(Resources.Congress))]
            UserLayout = 2,
            [Description("CongressRefereeLayout", Type = typeof(Resources.Congress))]
            CongressRefereeLayout = 3,
            [Description("CongressUserLayout", Type = typeof(Resources.Congress))]
            CongressUserLayout = 4,
            [Description("WebDesignUserLayout", Type = typeof(Resources.Congress))]
            WebDesignUserLayout = 5,

        }
        public enum HasValue : byte
        {
            [Description("All", Type = typeof(Resources.Congress))]
            None = 0,
            [Description("Has", Type = typeof(Resources.Congress))]
            Has = 1,
            [Description("NoHas", Type = typeof(Resources.Congress))]
            NotHas = 2,

        }
        public enum PaymentSection : byte
        {
            [Description("PaymentUserRegister", Type = typeof(Resources.Congress))]
            UserRegister = 1,
            [Description("PaymentWorkShopReserv", Type = typeof(Resources.Congress))]
            WorkShop = 2,
            [Description("PaymentHotelReserv", Type = typeof(Resources.Congress))]
            Hotel = 3,
            [Description("PaymentBoothReserv", Type = typeof(Resources.Congress))]
            Booth = 4,
            [Description("PaymentArticlePayment", Type = typeof(Resources.Congress))]
            Article = 5,
        }
        public enum SortAccordingToUser : byte
        {
            [Description("RegisterDate", Type = typeof(Resources.Congress))]
            RegisterDate = 0,
            [Description("LName", Type = typeof(Resources.Congress))]
            LName = 1,
            [Description("PayDate", Type = typeof(Resources.Congress))]
            PayDate = 2,
        }
        public enum CongressDefinitionReportTypes : byte
        {
            [Description("RptBoothOfficer", Type = typeof(Resources.Congress))]
            RptBoothOfficer = 1,
            [Description("RptHotelUser", Type = typeof(Resources.Congress))]
            RptHotelUser = 2,
            [Description("RptMiniUserCard", Type = typeof(Resources.Congress))]
            RptMiniUserCard = 3,
            [Description("RptUserBooth", Type = typeof(Resources.Congress))]
            RptUserBooth = 4,
            [Description("RptUserCard", Type = typeof(Resources.Congress))]
            RptUserCard = 5,
            [Description("RptUser", Type = typeof(Resources.Congress))]
            RptUser = 6,
            [Description("RptWorkShopUser", Type = typeof(Resources.Congress))]
            RptWorkShopUser = 7,
            [Description("RptArticleCertificate", Type = typeof(Resources.Congress))]
            RptArticleCertificate = 8,
            [Description("RptArticle", Type = typeof(Resources.Congress))]
            RptArticle = 9,
            [Description("RptChipFood", Type = typeof(Resources.Congress))]
            RptChipFood = 10,
            [Description("RptCongressCertificate", Type = typeof(Resources.Congress))]
            RptCongressCertificate = 11,
            [Description("RptAbstractArticle", Type = typeof(Resources.Congress))]
            RptAbstractArticle = 12,
            [Description("RptUserInfoCard", Type = typeof(Resources.Congress))]
            RptUserInfoCard = 13,
        }
        public enum UserChairStatus : byte
        {
            None = 0,
            [Description("HasChair", Type = typeof(Resources.Congress))]
            HasChair = 1,
            [Description("NotHasChair", Type = typeof(Resources.Congress))]
            NoHasChair = 2,

        }
        public enum ArticleCertificateType : byte
        {
            [Description("ForeachCertificate", Type = typeof(Resources.Congress))]
            ForeachCertificate = 1,
            [Description("AllsingleCertificate", Type = typeof(Resources.Congress))]
            AllsingleCertificate = 2,

        }
        public enum CardType
        {
            RegisterCard,
            ArticleTypeCard,
            BoothOfficerCard,
            ChipFood
        }
        public enum InformActions
        {
            Insert,
            Update,
            Payment,
        }
        public enum SubscribeStatus : byte
        {
            None = 0,
            MailSent = 1,
            NotConfirmed = 2,
            Confirmed = 3,
            Registered = 4,
            Deactived = 5,
        }
        public enum SearchType : byte
        {
            [Description("All", Type = typeof(Resources.Congress))]
            All = 0,
            [Description("Content",Type = typeof(Resources.Congress))]
            Content = 1,
            [Description("News",Type = typeof(Resources.Congress))]
            News = 2,

        }
        public enum UserStatus : byte
        {
            [Description("UserPreRegisterStatus", Type = typeof(Resources.Congress))]
            PreRegister = 0,
            [Description("UserRegisterStatus", Type = typeof(Resources.Congress))]
            Register = 1,
            [Description("UserRegisterPayStatus", Type = typeof(Resources.Congress))]
            RegisterPay = 2,
            [Description("UserRegisterPayConfirmStatus", Type = typeof(Resources.Congress))]
            PayConfirm = 3,
            [Description("UserRegisterPayDenialStatus", Type = typeof(Resources.Congress))]
            PayDenial = 4,
            [Description("UserRegisterConfirmPresentInHomaStatus", Type = typeof(Resources.Congress))]
            ConfirmPresentInHoma = 5,
            [Description("UserRegisterDenialPresentInHomaStatus", Type = typeof(Resources.Congress))]
            DenialPresentInHoma = 6,
            [Description("Guest", Type = typeof(Resources.Congress))]
            Guest = 7,

        }

        public enum Gender : byte
        {
            [Description("Female", Type = typeof(Resources.Congress))]
            Female = 0,
            [Description("Male", Type = typeof(Resources.Congress))]
            Male = 1,
        }
        public enum CongressStatus
        {
            NotRegistered,
            NotConfiged,
            Disabled,
            NotStatrted,
            Ended,
            NoProblem
        }
        public enum SupporterShowType : byte
        {
            [Description("Image", Type = typeof(Resources.Congress))]
            Image = 1,
            [Description("ImageAndTitle", Type = typeof(Resources.Congress))]
            ImageAndTitle = 2,
            [Description("OnlyTitle", Type = typeof(Resources.Congress))]
            OnlyTitle = 3,

        }
        public enum FinalStateSpecialRefree : byte
        {
            [Description("WaitForAnswer", Type = typeof(Resources.Congress))]
            WaitForAnswer = 0,
            [Description("AbtractConfirm", Type = typeof(Resources.Congress))]
            AbstractConfirm = 1,
            [Description("Confirm", Type = typeof(Resources.Congress))]
            Confirm = 2,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 3,
            [Description("DenialAbstarct", Type = typeof(Resources.Congress))]
            DenialAbstarct = 4,
            [Description("ConfirmandEdit", Type = typeof(Resources.Congress))]
            ConfirmandEdit = 5,
            [Description("ConFirmReferee", Type = typeof(Resources.Congress))]
            ConFirmReferee = 6
        }

        public enum FinalState : byte
        {
            [Description("WaitForAnswer", Type = typeof(Resources.Congress))]
            WaitForAnswer = 0,
            [Description("AbtractConfirm", Type = typeof(Resources.Congress))]
            AbstractConfirm = 1,
            [Description("Confirm", Type = typeof(Resources.Congress))]
            Confirm = 2,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 3,
            [Description("DenialAbstarct", Type = typeof(Resources.Congress))]
            DenialAbstarct = 4,
            [Description("ConfirmandEdit", Type = typeof(Resources.Congress))]
            ConfirmandEdit = 5,
            [Description("ConFirmReferee", Type = typeof(Resources.Congress))]
            ConFirmReferee = 6,
            [Description("SendToReferee", Type = typeof(Resources.Congress))]
            SendToReferee = 7,
            [Description("RefereeAnswered", Type = typeof(Resources.Congress))]
            RefereeAnswered = 8,
        }
        public enum FinalStateReferee : byte
        {
            [Description("WaitForAnswer", Type = typeof(Resources.Congress))]
            WaitForAnswer = 0,
            [Description("AbtractConfirm", Type = typeof(Resources.Congress))]
            AbstractConfirm = 1,
            [Description("Confirm", Type = typeof(Resources.Congress))]
            Confirm = 2,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 3,
            [Description("DenialAbstarct", Type = typeof(Resources.Congress))]
            DenialAbstarct = 4,
            [Description("ConfirmandEdit", Type = typeof(Resources.Congress))]
            ConfirmandEdit = 5,
        }

        public enum SortAccordingToArticle : byte
        {
            [Description("TitleArticle", Type = typeof(Resources.Congress))]
            TitleArticle = 0,
            [Description("DateOfSendingArticle", Type = typeof(Resources.Congress))]
            DateOfSendingArticle = 1,
            [Description("DateOfSendingAbstract", Type = typeof(Resources.Congress))]
            DateOfSendingAbstract = 2,
            [Description("DateOfLastFlowChanges", Type = typeof(Resources.Congress))]
            DateOfLastFlowChanges = 3,
            [Description("ArticleCode", Type = typeof(Resources.Congress))]
            ArticleCode = 4,
            //[Description("DateOfOkArticle", Type = typeof(Resources.Congress))]
            //DateOfOkArticle = 3,
            //[Description("DateOfOkAbstract", Type = typeof(Resources.Congress))]
            //DateOfOkAbstract = 4,
            //[Description("EditDate", Type = typeof(Resources.Congress))]
            //EditDate = 5,
            //[Description("EditTime", Type = typeof(Resources.Congress))]
            //EditTime = 6
        }

        public enum AscendingDescending : byte
        {
            [Description("Ascending", Type = typeof(Resources.Congress))]
            Ascending = 0,
            [Description("Descending", Type = typeof(Resources.Congress))]
            Descending = 1,
        }

        public enum RezervState : byte
        {
            [Description("RegisterRequest", Type = typeof(Resources.Congress))]
            RegisterRequest = 0,
            [Description("Pay", Type = typeof(Resources.Congress))]
            Pay = 1,
            [Description("PayConfirm", Type = typeof(Resources.Congress))]
            PayConfirm = 2,
            [Description("Finalconfirm", Type = typeof(Resources.Congress))]
            Finalconfirm = 3,
            [Description("DenialPay", Type = typeof(Resources.Congress))]
            DenialPay = 4,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 5,

        }
        public enum WorkShopRezervState : byte
        {
            [Description("RegisterRequest", Type = typeof(Resources.Congress))]
            RegisterRequest = 0,
            [Description("Pay", Type = typeof(Resources.Congress))]
            Pay = 1,
            [Description("PayConfirm", Type = typeof(Resources.Congress))]
            PayConfirm = 2,
            [Description("Finalconfirm", Type = typeof(Resources.Congress))]
            Finalconfirm = 3,
            [Description("PresentConfirm", Type = typeof(Resources.Congress))]
            PresentConfirm = 4,
            [Description("DenialPay", Type = typeof(Resources.Congress))]
            DenialPay = 5,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 6,
            [Description("PresentDenial", Type = typeof(Resources.Congress))]
            PresentDenial = 7
        }





        public enum ArticleState : byte
        {
            [Description("AbstractSended", Type = typeof(Resources.Congress))]
            AbstractSended = 0,
            [Description("OrginalSended", Type = typeof(Resources.Congress))]
            OrginalSended = 1,
            [Description("WaitForRefereeOpinion", Type = typeof(Resources.Congress))]
            WaitForRefereeOpinion = 2,
            [Description("RefereesAnswered", Type = typeof(Resources.Congress))]
            WaitForScientificTeacher = 3,
            [Description("AbtractConfirm", Type = typeof(Resources.Congress))]
            AbstractConfirm = 4,
            [Description("Finalconfirm", Type = typeof(Resources.Congress))]
            Confirm = 5,
            [Description("DenialAbstarct", Type = typeof(Resources.Congress))]
            DenialAbstarct = 6,
            [Description("Denial", Type = typeof(Resources.Congress))]
            Denial = 7,
            [Description("ConfirmandEdit", Type = typeof(Resources.Congress))]
            ConfirmandEdit = 8,
            [Description("SentToSpecialReferee", Type = typeof(Resources.Congress))]
            SentToSpecialReferee = 9,
            [Description("WaitforSpecialRefereeOpinion", Type = typeof(Resources.Congress))]
            WaitforSpecialRefereeOpinion = 10,
            [Description("ArticleSended", Type = typeof(Resources.Congress))]
            ArticleSended = 11,
        }


        public enum ArticlepayState : byte
        {
            [Description("PayWait", Type = typeof(Resources.Congress))]
            PayWait = 1,
            [Description("Pay", Type = typeof(Resources.Congress))]
            Pay = 2,
            [Description("PayConfirm", Type = typeof(Resources.Congress))]
            PayConfirm = 3,
            [Description("DenialPay", Type = typeof(Resources.Congress))]
            PayDenial = 4,



        }
        public enum SentUserTypes
        {
            [Description("SentForUserSendAbstractArticle", Type = typeof(Resources.Congress))]
            SentForUserSendAbstractArticle,
            [Description("SendToMembersOfTheAbstractsWillBeConfirmedAfterCorrection", Type = typeof(Resources.Congress))]
            SendToMembersOfTheAbstractsWillBeConfirmedAfterCorrection,
            [Description("SentForUserSendOrginalArticle", Type = typeof(Resources.Congress))]
            SentForUserSendOrginalArticle,
            [Description("SendForUserAbstarctConfirmAndNotSendOrginal", Type = typeof(Resources.Congress))]
            SendForUserAbstarctConfirmAndNotSendOrginal,
            [Description("SendForUserAbstarctConfirm", Type = typeof(Resources.Congress))]
            SendForUserAbstarctConfirm,
            [Description("SendForUserOrginalConfirm", Type = typeof(Resources.Congress))]
            SendForUserOrginalConfirm,
            [Description("SendForUserArticleDenial", Type = typeof(Resources.Congress))]
            SendForUserArticleDenial,
            [Description("SendForUserWhoReservedHotelButNotPay", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedHotelButNotPay,
            [Description("SendForUserWhoReservedHotelConfirm", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedHotelConfirm,
            [Description("SendForUserWhoReservedHotelDenial", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedHotelDenial,
            [Description("SendForUserWhoReservedWorkShopButNotPay", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedWorkShopButNotPay,
            [Description("SendForUserWhoReservedWorkShopConfirm", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedWorkShopConfirm,
            [Description("SendForUserWhoReservedWorkShopDenial", Type = typeof(Resources.Congress))]
            SendForUserWhoReservedWorkShopDenial,
            [Description("SendForUserWhoRegisterPayConfirm", Type = typeof(Resources.Congress))]
            SendForUserWhoRegisterPayConfirm,
            [Description("SendForUserWhoRegisterPayDenial", Type = typeof(Resources.Congress))]
            SendForUserWhoRegisterPayDenial,
            [Description("SendForUserWhoRegisterAndnotPay", Type = typeof(Resources.Congress))]
            SendForUserWhoRegisterAndnotPay,
            [Description("SendForUserWhoConfirmInHoma", Type = typeof(Resources.Congress))]
            SendForUserWhoConfirmInHoma,
            [Description("SendForUserWhoDenialInHoma", Type = typeof(Resources.Congress))]
            SendForUserWhoDenialInHoma,
            [Description("SendForRefrereeHasWaitForAnswerArticle", Type = typeof(Resources.Congress))]
            SendForRefrereeHasWaitForAnswerArticle,
            [Description("SendForRefrereeNotHasWaitForAnswerArticle", Type = typeof(Resources.Congress))]
            SendForRefrereeNotHasWaitForAnswerArticle,
            [Description("SendForAllUserInPreRegisterStatus", Type = typeof(Resources.Congress))]
            SendForAllUserInPreRegisterStatus,
            [Description("SendForAllUserPayConfirmAndNotHaveChair", Type = typeof(Resources.Congress))]
            SendForAllUserPayConfirmAndNotHaveChair,
            [Description("SendForAllUserHaveSaledChair", Type = typeof(Resources.Congress))]
            SendForAllUserHaveSaledChair,
            [Description("SendForAllUserHaveReserveChair", Type = typeof(Resources.Congress))]
            SendForAllUserHaveReserveChair,
            [Description("SendForAllUser", Type = typeof(Resources.Congress))]
            SendForAllUser,
            [Description("SendForAllMan", Type = typeof(Resources.Congress))]
            SendForAllMan,
            [Description("SendForAllWomen", Type = typeof(Resources.Congress))]
            SendForAllWomen,



        }
        public enum ArticlePaymentSteps : byte
        {
            [Description("BeforSendArticle", Type = typeof(Resources.Congress))]
            BeforSendArticle = 1,
            [Description("AfterFinalState", Type = typeof(Resources.Congress))]
            AfterFinalState = 2,
        }
        public enum UserInformType : byte
        {
            [Description("Email", Type = typeof(Resources.Congress))]
            Email = 1,
            [Description("SMS", Type = typeof(Resources.Congress))]
            SMS = 2,
            [Description("Both", Type = typeof(Resources.Congress))]
            Both = 3,
        }
        public enum ChartEnums
        {
            [Description("ChartUserRegisterByDay", Type = typeof(Resources.Congress))]
            ChartUserRegisterByDay,
            [Description("ChartUserRegisterByMoth", Type = typeof(Resources.Congress))]
            ChartUserRegisterByMoth,
            [Description("ChartUserRegisterByStatus", Type = typeof(Resources.Congress))]
            ChartUserRegisterByStatus,
            [Description("ChartUserRegisterByRegisterType", Type = typeof(Resources.Congress))]
            ChartUserRegisterByRegisterType,
            [Description("ChartArticleByDay", Type = typeof(Resources.Congress))]
            ChartArticleByDay,
            [Description("ChartArticleByMoth", Type = typeof(Resources.Congress))]
            ChartArticleByMoth,
            [Description("ChartArticleByType", Type = typeof(Resources.Congress))]
            ChartArticleByType,
            [Description("ChartArticleByPivot", Type = typeof(Resources.Congress))]
            ChartArticleByPivot,
            [Description("ChartPivotCategory", Type = typeof(Resources.Congress))]
            ChartPivotCategory,
            [Description("ChartArticleByStatus", Type = typeof(Resources.Congress))]
            ChartArticleByStatus,
            [Description("ChartHotelByStatus", Type = typeof(Resources.Congress))]
            ChartHotelByStatus,
            [Description("ChartHotelUser", Type = typeof(Resources.Congress))]
            ChartHotelUser,
            [Description("ChartWorkShopByStatus", Type = typeof(Resources.Congress))]
            ChartWorkShopByStatus,
            [Description("ChartWorkShopUser", Type = typeof(Resources.Congress))]
            ChartWorkShopUser,
            [Description("ChartCashBymoth", Type = typeof(Resources.Congress))]
            ChartCashBymoth,
            [Description("ChartCashByDay", Type = typeof(Resources.Congress))]
            ChartCashByDay,
            [Description("ChartCash", Type = typeof(Resources.Congress))]
            ChartCash,
            [Description("ChartViewsNumberByDay", Type = typeof(Resources.Congress))]
            ChartViewsNumberByDay,
            [Description("ChartViewsNumberByMonth", Type = typeof(Resources.Congress))]
            ChartViewsNumberByMonth,
            [Description("NumberArticlesReferee", Type = typeof(Resources.Congress))]
            NumberArticlesReferee,
            [Description("NumberBoothsByDivision", Type = typeof(Resources.Congress))]
            NumberBoothsByDivision,
            [Description("NumberStandsWithReservationSeparation", Type = typeof(Resources.Congress))]
            NumberStandsWithReservationSeparation,
            [Description("KindOfSupport", Type = typeof(Resources.Congress))]
            KindOfSupport,
        }

        public enum MessageInformType : byte
        {
            [Description("UserRegister", Type = typeof(Resources.Congress))]
            User = 1,
            [Description("Workshop", Type = typeof(Resources.Congress))]
            Workshop = 2,
            [Description("Hotel", Type = typeof(Resources.Congress))]
            Hotel = 3,
            [Description("booth", Type = typeof(Resources.Congress))]
            Booth = 4,
            [Description("Article", Type = typeof(Resources.Congress))]
            Article = 5,
            [Description("Referee", Type = typeof(Resources.Congress))]
            Referee = 6,
            [Description("RefereeArticle", Type = typeof(Resources.Congress))]
            RefereeArticle = 7
        }

        public enum UserMessageKey : byte
        {
            [Description("Username", Type = typeof(Resources.Congress))]
            Username = 1,
            [Description("FullName", Type = typeof(Resources.Congress))]
            FullName = 2,
            [Description("Email", Type = typeof(Resources.Congress))]
            Email = 3,
            [Description("Status", Type = typeof(Resources.Congress))]
            Status = 4,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 5,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 6
        }
        public enum RefereeMessageKey : byte
        {
            [Description("Username", Type = typeof(Resources.Congress))]
            Username = 1,
            [Description("FullName", Type = typeof(Resources.Congress))]
            FullName = 2,
            [Description("Password", Type = typeof(Resources.Congress))]
            Password = 3,
            [Description("Email", Type = typeof(Resources.Congress))]
            Email = 4,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 5,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 6
        }
        public enum RefereeArticleMessageKey : byte
        {
            [Description("Username", Type = typeof(Resources.Congress))]
            Username = 1,
            [Description("FullName", Type = typeof(Resources.Congress))]
            FullName = 2,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 3,
            [Description("ArticleTitle", Type = typeof(Resources.Congress))]
            ArticleTitle = 4,
            [Description("ArticleCode", Type = typeof(Resources.Congress))]
            ArticleCode = 5,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 6
        }

        public enum WorkshopMessageKey : byte
        {
            [Description("WorkshopName", Type = typeof(Resources.Congress))]
            WorkshopName = 1,
            [Description("UsersName", Type = typeof(Resources.Congress))]
            UsersName = 2,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 3,
            [Description("Status", Type = typeof(Resources.Congress))]
            Status = 4,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 5

        }

        public enum HotelMessageKey : byte
        {
            [Description("HotelName", Type = typeof(Resources.Congress))]
            HotelName = 1,
            [Description("UsersName", Type = typeof(Resources.Congress))]
            UsersName = 2,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 3,
            [Description("Status", Type = typeof(Resources.Congress))]
            Status = 4,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 5
        }

        public enum BoothMessageKey : byte
        {
            [Description("BoothCode", Type = typeof(Resources.Congress))]
            BoothCode = 1,
            [Description("UsersName", Type = typeof(Resources.Congress))]
            UsersName = 2,
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 3,
            [Description("Status", Type = typeof(Resources.Congress))]
            Status = 4,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 5
        }

        public enum ArticleMessageKey : byte
        {
            [Description("CongressTitle", Type = typeof(Resources.Congress))]
            CongressTitle = 1,
            [Description("ArticleTitle", Type = typeof(Resources.Congress))]
            ArticleTitle = 2,
            [Description("UsersName", Type = typeof(Resources.Congress))]
            UsersName = 3,
            [Description("ArticleCode", Type = typeof(Resources.Congress))]
            ArticleCode = 4,
            [Description("ArticleStatus", Type = typeof(Resources.Congress))]
            ArticleStatus = 5,
            [Description("CongressAddress", Type = typeof(Resources.Congress))]
            CongressAddress = 6
        }



    }
}
