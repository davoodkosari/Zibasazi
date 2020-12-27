using System;
using System.Collections.Generic;
using Radyn.Congress.Facade;
using Radyn.Utility;

namespace Radyn.Congress.Tools
{
    public class CongressReport
    {
        public IEnumerable<ModelView.ReportChartModel> GetChart(Enums.ChartEnums type, Guid congressId, string year, string getmoth, string url)
        {
            var moth = "";
            if (!string.IsNullOrEmpty(getmoth))
            {
                var value = ((byte)getmoth.ToEnum<Common.Definition.Enums.PersianMonth>());
                if (value < 10) moth = "0" + value;
                else  moth = value.ToString();
            }
          
            switch (type)
            {
                case Enums.ChartEnums.ChartArticleByDay:
                       return new ArticleFacade().ChartArticleDayCount(congressId, moth, year);
                    break;
                case Enums.ChartEnums.ChartArticleByMoth:
                    return new ArticleFacade().ChartArticleMothCount(congressId, year);
                    break;
                case Enums.ChartEnums.ChartArticleByPivot:
                    return new ArticleFacade().ChartArticlePivotCount(congressId);
                    break;
                case Enums.ChartEnums.ChartPivotCategory:
                    return new ArticleFacade().ChartArticlePivotCategoryCount(congressId);
                    break;
                case Enums.ChartEnums.ChartArticleByType:
                    return new ArticleFacade().ChartArticleTypeCount(congressId);
                    break;
                case Enums.ChartEnums.ChartArticleByStatus:
                    return new ArticleFacade().ChartArticleStatusCount(congressId);
                    break;
                case Enums.ChartEnums.ChartHotelByStatus:
                    return new HotelUserFacade().CharHotelCountWithStatus(congressId);
                    break;
                case Enums.ChartEnums.ChartHotelUser:
                    return new HotelUserFacade().CharHotelCountWithReserv(congressId);
                    break;
                case Enums.ChartEnums.ChartUserRegisterByDay:
                    return new UserFacade().ChartUserDayCount(congressId, moth, year);
                    break;
                case Enums.ChartEnums.ChartUserRegisterByMoth:
                    return new UserFacade().ChartUserMothCount(congressId, year);
                    break;
                case Enums.ChartEnums.ChartUserRegisterByRegisterType:
                    return new UserRegisterPaymentTypeFacade().ChartRegisterTypeCount(congressId);
                    break;
                case Enums.ChartEnums.ChartUserRegisterByStatus:
                    return new UserFacade().ChartUserStatusCount(congressId);
                    break;
                case Enums.ChartEnums.ChartWorkShopByStatus:
                    return new WorkShopUserFacade().ChartWorkShopCountStatus(congressId);
                    break;
                case Enums.ChartEnums.ChartWorkShopUser:
                    return new WorkShopUserFacade().ChartWorkShopCountByReserv(congressId);
                case Enums.ChartEnums.ChartCash:
                    return new HomaFacade().ChartChash(congressId);
                    break;
                case Enums.ChartEnums.ChartCashBymoth:
                    return new HomaFacade().ChartChash(congressId, year);
                    break;
                case Enums.ChartEnums.ChartCashByDay:
                    return new HomaFacade().ChartChash(congressId, year, moth);
                    break;
                case Enums.ChartEnums.ChartViewsNumberByDay:
                    return new HomaFacade().ChartViewByDay(moth, year,url );
                    break;
                case Enums.ChartEnums.ChartViewsNumberByMonth:
                    return new HomaFacade().ChartViewByMonth(year,url);
                    break;
                case Enums.ChartEnums.NumberArticlesReferee:
                    return new HomaFacade().ChartNumberArticleByReferee(congressId);
                    break;
                case Enums.ChartEnums.NumberBoothsByDivision:
                    return new UserBoothFacade().ChartNumberBoothsByDivision(congressId);
                    break;
                case Enums.ChartEnums.NumberStandsWithReservationSeparation:
                    return new UserBoothFacade().ChartNumberStandsWithReservationSeparation(congressId);
                    break;
                case Enums.ChartEnums.KindOfSupport:
                    return new SupporterFacade().ChartKindOfSupport(congressId);
                    break;

            }
            return null;
        }
    }
}
