using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Xml;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Settings;
using Radyn.Congress.Tools;
using Radyn.ContentManager;
using Radyn.EnterpriseNode;
using Radyn.FormGenerator;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.News;
using Radyn.Statistics;
using Radyn.Utility;
using Radyn.XmlModel;
using ModelView = Radyn.Message.Tools.ModelView;

namespace Radyn.Congress.BO
{
    internal class HomaBO : BusinessBase<Homa>
    {
        public void ContenetSearch(IConnectionHandler connectionHandler,Guid congressId, string txtvalue, List<Tools.ModelView.SerachResultvalue> resultvalues)
        {
            var content =
                ContentManagerComponent.Instance.ContentContentFacade.Where(
                    c => c.Text.Contains(txtvalue) || c.Title.Contains(txtvalue) || c.Abstract.Contains(txtvalue));
            if (!content.Any()) return;
            var ints = content.Select(x => x.Id);
            var contents =
                new CongressContentBO().Select(connectionHandler, x => x.Content,
                    c => c.CongressId == congressId && c.ContentId.In(ints));

            if (!contents.Any()) return;
            foreach (var VARIABLE in contents)
            {
                var firstOrDefault = content.FirstOrDefault(x => x.Id == VARIABLE.Id);
                if (firstOrDefault == null) continue;
                var resultvalue = new Tools.ModelView.SerachResultvalue();
                resultvalue.Key = VARIABLE.Id.ToString();
                resultvalue.ResultValue = firstOrDefault.Title;
                resultvalue.SearchType = Enums.SearchType.Content;
                resultvalues.Add(resultvalue);
            }
        }

        public void NewsSearch(IConnectionHandler connectionHandler, Guid congressId, string txtvalue, List<Tools.ModelView.SerachResultvalue> resultvalues)
        {
            var @select = NewsComponent.Instance.NewsContentFacade.Where(
                x => x.Body.Contains(txtvalue) || x.Lead.Contains(txtvalue) || x.Title1.Contains(txtvalue) ||
                     x.Title2.Contains(txtvalue));
            if (!@select.Any()) return;
            var enumerable = @select.Select(x => x.Id);

            var newses1 =
                new CongressNewsBO().Select(connectionHandler, x => x.News,
                    c => c.CongressId == congressId && c.NewsId.In(enumerable));
            if (!newses1.Any()) return;
            foreach (var VARIABLE in newses1)
            {
                var firstOrDefault = @select.FirstOrDefault(x => x.Id == VARIABLE.Id);
                if (firstOrDefault == null) continue;
                var resultvalue = new Tools.ModelView.SerachResultvalue();
                resultvalue.Key = VARIABLE.Id.ToString();
                resultvalue.ResultValue = firstOrDefault.Title1;
                resultvalue.SearchType = Enums.SearchType.News;
                resultvalues.Add(resultvalue);
            }
        }
        public override bool Insert(IConnectionHandler connectionHandler, Homa obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (string.IsNullOrEmpty(CongressConfiguration.Key))
                throw new Exception(Resources.Congress.YouCanNotAddNewCongress);
            if (!AllowAdd(connectionHandler, ObjectState.New, obj))
                throw new Exception(Resources.Congress.YouCanNotAddNewCongress);
            obj.CreateDate = DateTime.Now.ShamsiDate();
            return base.Insert(connectionHandler, obj);
        }
     
        public bool Delete(IConnectionHandler connectionHandler,
            IConnectionHandler enterpriseNodeConnectionconnectionHandler,
            IConnectionHandler statisticConnectionconnectionHandler, params object[] keys)
        {
            var obj = new HomaBO().Get(connectionHandler, keys);
            var homaAliasBo = new HomaAliasBO();
            var list = homaAliasBo.Where(connectionHandler, x => x.CongressId == obj.Id);
            foreach (var homaAliase in list)
            {
                if (!homaAliasBo.Delete(connectionHandler, homaAliase.Id))
                    throw new Exception(Resources.Congress.ErrorInSaveCongress);
            }
            if (!new HomaBO().Delete(connectionHandler, keys))
                throw new Exception(Resources.Congress.ErrorInDeleteCongress);
            var webSiteTransactionalFacade =
                StatisticComponents.Instance.WebSiteTransactionalFacade(statisticConnectionconnectionHandler);


            var byFilter = webSiteTransactionalFacade.Where(x => x.OwnerId == obj.OwnerId);
            if (byFilter != null)
            {
                foreach (var webSite in byFilter)
                {
                    if (!webSiteTransactionalFacade.Delete(webSite.Id))
                        throw new Exception("خطایی در حذف آمار سایت وجود دارد");
                }
            }
            else
            {
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(
                        enterpriseNodeConnectionconnectionHandler).Delete(obj.OwnerId))
                    return false;
            }
            return true;
        }

        public bool Update(IConnectionHandler connectionHandler,
            IConnectionHandler enterpriseNodeConnectionconnectionHandler,
            IConnectionHandler statisticConnectionconnectionHandler, Homa homa,
            
            HttpPostedFileBase file, List<HomaAlias> homaAliases)
        {

            if (
                !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(
                    enterpriseNodeConnectionconnectionHandler)
                    .Update(homa.Owner, file))
                return false;



            if (!new HomaBO().Update(connectionHandler, homa))
                throw new Exception(Resources.Congress.ErrorInEditCongress);
            var homaAliasBo = new HomaAliasBO();
            var list = homaAliasBo.Where(connectionHandler, x => x.CongressId == homa.Id);
            foreach (var homaAliase in homaAliases)
            {
                var homaAlias = homaAliasBo.Get(connectionHandler, homaAliase.Id);
                if (homaAlias == null)
                {
                    homaAliase.CongressId = homa.Id;
                    homaAliase.Homa = homa;
                    if (!homaAliasBo.Insert(connectionHandler, homaAliase))
                        throw new Exception(Resources.Congress.ErrorInSaveCongress);
                }
                else
                {
                    homaAliase.Homa = homa;
                    if (!homaAliasBo.Update(connectionHandler, homaAliase))
                        throw new Exception(Resources.Congress.ErrorInSaveCongress);
                }

            }
            foreach (var homaAliase in list)
            {
                if (homaAliases.Any(x => x.Id == homaAliase.Id)) continue;
                if (!homaAliasBo.Delete(connectionHandler, homaAliase.Id))
                    throw new Exception(Resources.Congress.ErrorInSaveCongress);
            }

            if (!StatisticComponents.Instance.WebSiteTransactionalFacade(statisticConnectionconnectionHandler)
                .Modify(StringUtils.Decrypt(homa.InstallPath), homa.CongressTitle, homa.OwnerId))
                throw new Exception(Resources.Congress.ErrorInEditCongress);
            return true;
        }

        public bool Insert(IConnectionHandler connectionHandler,
            IConnectionHandler enterpriseNodeConnectionconnectionHandler,
            IConnectionHandler statisticConnectionconnectionHandler, Homa homa,
            HttpPostedFileBase file, List<HomaAlias> homaAliases)
        {

            if (
                !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(
                    enterpriseNodeConnectionconnectionHandler)
                    .Insert(homa.Owner, file))
                throw new Exception("خطایی در ذخیره اطلاعات شخصی صاحب همایش وجود دارد");
            homa.OwnerId = homa.Owner.Id;
            if (!this.Insert(connectionHandler, homa))
                throw new Exception(Resources.Congress.ErrorInSaveCongress);
            var homaAliasBo = new HomaAliasBO();

            foreach (var homaAliase in homaAliases)
            {
                homaAliase.CongressId = homa.Id;
                homaAliase.Homa = homa;
                if (!homaAliasBo.Insert(connectionHandler, homaAliase))
                    throw new Exception(Resources.Congress.ErrorInSaveCongress);
            }

            if (
                !StatisticComponents.Instance.WebSiteTransactionalFacade(statisticConnectionconnectionHandler)
                    .Modify(
                        StringUtils.Decrypt(homa.InstallPath) +
                        (string.IsNullOrEmpty(homa.VirtualDirectory) ? "" : homa.VirtualDirectory),
                        homa.CongressTitle, homa.OwnerId))
                throw new Exception(Resources.Congress.ErrorInSaveCongress);
            return true;
        }

        private bool AllowAdd(IConnectionHandler connectionHandler, ObjectState state, Homa obj)
        {
            if (string.IsNullOrEmpty(CongressConfiguration.Key))
                throw new Exception(Resources.Congress.YouCanNotAddNewCongress);
            var decrypt = StringUtils.Decrypt(CongressConfiguration.Key);
            var strings = decrypt.Split(',');
            var allowed = strings[0].ToInt();
            var continual = strings[1] == "1";
            int count;
            if (continual)
            {
                count = this.Count(connectionHandler, x => x.Id, x => x.Enabled);
                if (allowed == 0) return true;

                if (state == ObjectState.New)
                    if (obj != null && obj.Enabled)
                        count++;
                if (state == ObjectState.Dirty)
                    if (obj != null)
                        if (obj.Enabled)
                        {
                            var oldValue = Get(connectionHandler, obj.Id);
                            if (!oldValue.Enabled)
                                count++;
                        }
            }
            else
            {
                count = this.Count(connectionHandler, x => x.Id);
                if (obj != null && state == ObjectState.New)
                    count++;
            }

            return allowed >= count;

        }

        private async Task<bool>  AllowAddAsync(IConnectionHandler connectionHandler, ObjectState state, Homa obj)
        {
            if (string.IsNullOrEmpty(CongressConfiguration.Key))
                throw new Exception(Resources.Congress.YouCanNotAddNewCongress);
            var decrypt = StringUtils.Decrypt(CongressConfiguration.Key);
            var strings = decrypt.Split(',');
            var allowed = strings[0].ToInt();
            var continual = strings[1] == "1";
            int count;
            if (continual)
            {
                count =await this.CountAsync(connectionHandler, x => x.Id, x => x.Enabled);
                if (allowed == 0) return true;

                if (state == ObjectState.New)
                    if (obj != null && obj.Enabled)
                        count++;
                if (state == ObjectState.Dirty)
                    if (obj != null)
                        if (obj.Enabled)
                        {
                            var oldValue =await GetAsync(connectionHandler, obj.Id);
                            if (!oldValue.Enabled)
                                count++;
                        }
            }
            else
            {
                count = await  this.CountAsync(connectionHandler, x => x.Id);
                if (obj != null && state == ObjectState.New)
                    count++;
            }

            return allowed >= count;

        }
        protected override void CheckConstraint(IConnectionHandler connectionHandler, Homa item)
        {
            if (item.IsDefaultForConfig)
            {
                var firstOrDefault = this.FirstOrDefault(connectionHandler, x => x.IsDefaultForConfig && x.Id != item.Id);
                if (firstOrDefault != null)
                    throw new KnownException(String.Format("تنظیمات پیش فرض قبلا پژوهش {0} انتخاب شده است", firstOrDefault.CongressTitle));

            }
            if (!string.IsNullOrEmpty(item.InstallPath))
            {
                var homaId = GetUrlHomaId(connectionHandler, item.InstallPath, item.VirtualDirectory);
                if (homaId != null && homaId.Status == Enums.CongressStatus.NoProblem && homaId.Id != Guid.Empty && homaId.Id != item.Id)
                    throw new Exception(Resources.Congress.ThereIsCongressThisUrl);
                item.InstallPath = StringUtils.Encrypt(item.InstallPath.ToLower());
            }
            if (!string.IsNullOrEmpty(item.VirtualDirectory))
            {
                var areaNames =
                    RouteTable.Routes.OfType<Route>()
                        .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                        .Select(r => r.DataTokens["area"]).ToList();
                if (areaNames.Any(
                    areaname =>
                        item.VirtualDirectory.ToLower().StartsWith(areaname.ToString().ToLower()) ||
                        item.VirtualDirectory.ToLower().StartsWith("/" + areaname.ToString().ToLower())))
                    throw new Exception(string.Format("کلید واژه {0} رزرو شده است", item.VirtualDirectory));
                item.VirtualDirectory = item.VirtualDirectory.ToLower();

            }
            if (!string.IsNullOrEmpty(item.Description))
            {
                if (item.Description.Contains("'"))
                    item.Description = item.Description.Replace("'", "''");
            }

            base.CheckConstraint(connectionHandler, item);
        }

        public override bool Update(IConnectionHandler connectionHandler, Homa obj)
        {
            if (!AllowAdd(connectionHandler, ObjectState.Dirty, obj))
                throw new Exception(Resources.Congress.YouCanNotAddNewCongress);
            return base.Update(connectionHandler, obj);
        }

        public override Homa Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var obj = base.Get(connectionHandler, keys);
            if (obj != null && !string.IsNullOrEmpty(obj.InstallPath))
                obj.InstallPath = StringUtils.Decrypt(obj.InstallPath).ToLower();
            return FillHoma(connectionHandler, obj);

        }

        public override Homa GetLanuageContent(IConnectionHandler connectionHandler, string culture,
            params object[] keys)
        {
            var obj = base.Get(connectionHandler, keys);
            GetLanuageContent(connectionHandler, culture, obj);
            return obj;
        }

        public override void GetLanuageContent(IConnectionHandler connectionHandler, string culture, Homa obj)
        {

            if (obj != null && !string.IsNullOrEmpty(obj.InstallPath))
            {
                obj.InstallPath = StringUtils.Decrypt(obj.InstallPath).ToLower();

            }
            FillHoma(connectionHandler, obj);
        }

        public Homa FillHoma(IConnectionHandler connectionHandler, Homa homa)
        {
            var newGuid = Guid.NewGuid();
            if (homa != null)
            {
                homa.ConfigurationId = homa.Id;
                if (!homa.Enabled&&!homa.IsDefaultForConfig)
                {
                   
                    homa.Status = Enums.CongressStatus.Disabled;
                    return homa;
                }

                if (homa.EndDate != null && String.Compare(homa.EndDate, DateTime.Now.ShamsiDate(), StringComparison.Ordinal) < 0)
                {
                    homa.Status = Enums.CongressStatus.Ended;
                    return homa;
                }

                if (homa.StartDate != null &&
                    String.Compare(homa.StartDate, DateTime.Now.ShamsiDate(), StringComparison.Ordinal) > 0)
                {
                    homa.Status = Enums.CongressStatus.NotStatrted;
                    return homa;
                }

                if (homa.Configuration == null || homa.Configuration.CongressId == Guid.Empty)
                {
                    homa.Status = Enums.CongressStatus.NotConfiged;
                    return homa;
                }
                homa.Status = Enums.CongressStatus.NoProblem;
                return homa;
            }

            homa = new Homa() { Configuration = new Configuration(), Id = newGuid, Status = Enums.CongressStatus.NotRegistered };
            homa.Configuration.ConfigurationContent = new Dictionary<string, ConfigurationContent>();
            return homa;


            


        }

        public Homa GetUrlHomaId(IConnectionHandler connectionHandler, string authority, string url)
        {
            if (url == "/") url = string.Empty;
            var encrypt = StringUtils.Encrypt(authority.ToLower());
            var predicateBuilder = new PredicateBuilder<Homa>();
            predicateBuilder.Or(x => x.InstallPath == encrypt);
            var homas = new HomaAliasBO().Select(connectionHandler, x => x.CongressId, x => x.Url == encrypt);
            if (homas.Any())
                predicateBuilder.Or(x => x.Id.In(homas));
            var urlHomaId = this.Where(connectionHandler, predicateBuilder.GetExpression());

            var areaNames =
                RouteTable.Routes.OfType<Route>()
                    .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                    .Select(r => r.DataTokens["area"]).ToList();
            Homa homa = null;
            if (string.IsNullOrEmpty(url) ||
                (areaNames.Any(areaname => url.ToLower().StartsWith("/" + areaname.ToString().ToLower()))))
                homa = urlHomaId.FirstOrDefault(x => string.IsNullOrEmpty(x.VirtualDirectory));
            else
            {
                foreach (var variable in urlHomaId.Where(x => !string.IsNullOrEmpty(x.VirtualDirectory)))
                {
                    if (!url.ToLower().Contains(variable.VirtualDirectory.ToLower())) continue;
                    homa = variable;
                    break;
                }
            }


            return FillHoma(connectionHandler, homa);
        }
        public async Task<Homa> GetUrlHomaIdAsync(IConnectionHandler connectionHandler, string authority, string url)
        {
            if (url == "/") url = string.Empty;
            var encrypt = StringUtils.Encrypt(authority.ToLower());
            var predicateBuilder = new PredicateBuilder<Homa>();
            predicateBuilder.Or(x => x.InstallPath == encrypt);
            var homas =await new HomaAliasBO().SelectAsync(connectionHandler, x => x.CongressId, x => x.Url == encrypt);
            if (homas.Any())
                predicateBuilder.Or(x => x.Id.In(homas));
            var urlHomaId =await this.WhereAsync(connectionHandler, predicateBuilder.GetExpression());

            var areaNames =
                RouteTable.Routes.OfType<Route>()
                    .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                    .Select(r => r.DataTokens["area"]).ToList();
            Homa homa = null;
            if (string.IsNullOrEmpty(url) ||
                (areaNames.Any(areaname => url.ToLower().StartsWith("/" + areaname.ToString().ToLower()))))
                homa = urlHomaId.FirstOrDefault(x => string.IsNullOrEmpty(x.VirtualDirectory));
            else
            {
                foreach (var variable in urlHomaId.Where(x => !string.IsNullOrEmpty(x.VirtualDirectory)))
                {
                    if (!url.ToLower().Contains(variable.VirtualDirectory.ToLower())) continue;
                    homa = variable;
                    break;
                }
            }


            return FillHoma(connectionHandler, homa);
        }

        public void SendInform(byte type, ModelView.MessageModel inform, Configuration configuration, string homaTitle)
        {
            switch (type)
            {

                case (byte)Enums.UserInformType.SMS:
                    MessageComponenet.Instance.SMSFacade.SendSms(configuration.SMSAccountId,
                        configuration.SMSAccountUserName, configuration.SMSAccountPassword, inform.Mobile,
                        inform.SMSBody);
                    break;
                case (byte)Enums.UserInformType.Email:
                    if (
                        !MessageComponenet.Instance.MailFacade.SendMail(configuration.MailHost,
                            configuration.MailPassword, configuration.MailUserName, configuration.MailFrom,
                            configuration.MailPort, homaTitle, configuration.EnableSSL, inform.Email, inform.EmailTitle,
                            inform.EmailBody))
                        throw new Exception("مشکلی در ارسال ایمیل وجود دارد");
                    break;
                case (byte)Enums.UserInformType.Both:
                    try
                    {
                        if (
                            !MessageComponenet.Instance.MailFacade.SendMail(configuration.MailHost,
                                configuration.MailPassword, configuration.MailUserName, configuration.MailFrom,
                                configuration.MailPort, homaTitle, configuration.EnableSSL, inform.Email,
                                inform.EmailTitle, inform.EmailBody))
                            throw new Exception("مشکلی در ارسال ایمیل وجود دارد");
                    }
                    catch
                    {

                    }
                    try
                    {
                        MessageComponenet.Instance.SMSFacade.SendSms(configuration.SMSAccountId,
                            configuration.SMSAccountUserName, configuration.SMSAccountPassword, inform.Mobile,
                            inform.SMSBody);
                    }
                    catch
                    {
                    }
                    break;

            }

        }


        public IEnumerable<Tools.ModelView.ReportChartModel> ChartChash(IConnectionHandler connectionHandler,
            Guid congressId, string year = "", string month = "")
        {
            var list = new List<Tools.ModelView.ReportChartModel>();
            var workShopUsers = new WorkShopUserBO().GetTransactionId(connectionHandler, congressId, year, month);
            var hotelUsers = new HotelUserBO().GetTransactionId(connectionHandler, congressId, year, month);
            var users = new UserBO().GetTransactionId(connectionHandler, congressId, year, month);
            var transactionId = new ArticleBO().GetTransactionId(connectionHandler, congressId, year, month);
            var enumerable = new UserBoothBO().GetTransactionId(connectionHandler, congressId, year, month);
            var reportChartModel = new Tools.ModelView.ReportChartModel
            {
                Count = (long)workShopUsers,
                Value = Resources.Congress.Workshop,
                StringFormat = "N0"
            };
            list.Add(reportChartModel);
            var chartModel = new Tools.ModelView.ReportChartModel
            {
                Count = (long)hotelUsers,
                Value = Resources.Congress.Hotel,
                StringFormat = "N0"
            };
            list.Add(chartModel);
            var model = new Tools.ModelView.ReportChartModel
            {
                Count = (long)enumerable,
                Value = Resources.Congress.booth,
                StringFormat = "N0"
            };
            list.Add(model);
            var chartModel1 = new Tools.ModelView.ReportChartModel
            {
                Count = (long)users,
                Value = Resources.Congress.User,
                StringFormat = "N0"
            };
            list.Add(chartModel1);
            var item = new Tools.ModelView.ReportChartModel
            {
                Count = (long)transactionId,
                Value = Resources.Congress.Article,
                StringFormat = "N0"
            };
            list.Add(item);
            return list;
        }

        public bool HasLicense(IConnectionHandler connectionHandler)
        {
            return AllowAdd(connectionHandler, ObjectState.New, null);
        }
        public async Task<bool>  HasLicenseAsync(IConnectionHandler connectionHandler)
        {
            return await AllowAddAsync(connectionHandler, ObjectState.New, null);
        }
        public List<Homa> GetLastCongress(IConnectionHandler connectionHandler)
        {
            return this.OrderBy(connectionHandler, x => x.Order,
                x => x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) < 0 &&
                     x.Enabled
                );


        }

        public List<Homa> GetCurrentCongress(IConnectionHandler connectionHandler)
        {

            return this.OrderBy(connectionHandler, x => x.Order,
                x =>
                    x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 &&
                    x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 &&
                    x.Enabled
                );



        }



        public List<Homa> GetNextCongress(IConnectionHandler connectionHandler)
        {
            return this.OrderBy(connectionHandler, x => x.Order,
                x => x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) > 0 &&
                     x.Enabled
                );

        }

        public bool DailyEvaulation(IConnectionHandler connectionHandler, Dictionary<Guid, Tools.ModelView.ModifyResult<UserBooth>> keyValuePairs, Dictionary<Guid,Tools.ModelView.ModifyResult<WorkShopUser>> workShopUsers, Dictionary<Guid,Tools.ModelView.ModifyResult<HotelUser>> hotelUsers)
        {
            var list = this.GetCurrentCongress(connectionHandler);
            foreach (var homa in list)
            {
                var keyValuePair=new Tools.ModelView.ModifyResult<UserBooth>();
                var workShopUser = new Tools.ModelView.ModifyResult<WorkShopUser>();
                var hotelUser = new Tools.ModelView.ModifyResult<HotelUser>();
               
                if (!this.DailyEvaulationForConfig(connectionHandler, homa.Configuration, keyValuePair, workShopUser, hotelUser))
                    return false;
                keyValuePairs.Add(homa.Id, keyValuePair);
                workShopUsers.Add(homa.Id, workShopUser);
                hotelUsers.Add(homa.Id, hotelUser);
            }
            return true;

        }

        private bool DailyEvaulationForConfig(IConnectionHandler connectionHandler, Configuration configuration,
            Tools.ModelView.ModifyResult<UserBooth> keyValuePairs, Tools.ModelView.ModifyResult<WorkShopUser> workShopUsers, Tools.ModelView.ModifyResult<HotelUser> hotelUsers)
        {
           
            bool result;

            var userBoothBo = new UserBoothBO();
            if (configuration.DayCountDeleteBoothReserveNotPay != null &&
                configuration.DayCountDeleteBoothReserveNotPay > 0)
            {

                var list = userBoothBo.Where(connectionHandler,
                    x => x.Booth.CongressId == configuration.CongressId && x.TransactionId == null);
                foreach (var userBooth in list)
                {
                    var dateTime =
                        DateTimeUtil.ShamsiDateToGregorianDate(userBooth.RegisterDate)
                            .AddDays((double)configuration.DayCountDeleteBoothReserveNotPay)
                            .ShamsiDate();
                    if (dateTime.CompareTo(DateTime.Now.ShamsiDate()) < 0)
                    {

                        if (!userBoothBo.Delete(connectionHandler, userBooth))
                            throw new Exception(Resources.Congress.ErrorInWorkDailyScheduler);
                        keyValuePairs.AddInform(
                        userBooth,Resources.Congress.BoothChangeStatusEmail , Resources.Congress.BoothChangeStatusSMS
                        );



                    }
                }
            }
            var hotelUserBo = new HotelUserBO();
            if (configuration.DayCountDeleteHotelReserveNotPay != null &&
                configuration.DayCountDeleteHotelReserveNotPay > 0)
            {
                var list = hotelUserBo.Where(connectionHandler,
                    x => x.Hotel.CongressId == configuration.CongressId && x.TransactionId == null);
                foreach (var hotelUser in list)
                {
                    var user = hotelUser;
                    var dateTime =
                        DateTimeUtil.ShamsiDateToGregorianDate(user.RegisterDate)
                            .AddDays((double)configuration.DayCountDeleteHotelReserveNotPay)
                            .ShamsiDate();
                    if (dateTime.CompareTo(DateTime.Now.ShamsiDate()) < 0)
                    {
                        if (!hotelUserBo.Delete(connectionHandler, hotelUser))
                            throw new Exception(Resources.Congress.ErrorInWorkDailyScheduler);
                        if (hotelUsers.InformList.All(x => x.obj.UserId != user.UserId))
                            hotelUsers.AddInform(
                            
                                user,
                               Resources.Congress.HotelChangeStatusEmail, Resources.Congress.HotelChangeStatusSMS
                            );

                        if (!user.User.ParentId.HasValue || hotelUsers.InformList.Any(x => x.obj.UserId == user.User.ParentId))
                            continue;
                        hotelUsers.AddInform(
                        
                            new HotelUser()
                            {
                                UserId = (Guid)user.User.ParentId,
                                HotelId = user.HotelId,
                                Status = user.Status
                            },
                            Resources.Congress.HotelChangeStatusEmail , Resources.Congress.HotelChangeStatusSMS
                        );


                    }
                }
            }
            var workShopUserBo = new WorkShopUserBO();
            if (configuration.DayCountDeleteWorkShopReserveNotPay != null &&
                configuration.DayCountDeleteWorkShopReserveNotPay > 0)
            {
                var list = workShopUserBo.Where(connectionHandler,
                    x =>
                        x.WorkShop.CongressId == configuration.CongressId && x.TransactionId == null);
                foreach (var shopUser in list)
                {
                    var workShopUser = shopUser;
                    var addDays =
                        DateTimeUtil.ShamsiDateToGregorianDate(workShopUser.RegisterDate)
                            .AddDays((double)configuration.DayCountDeleteWorkShopReserveNotPay);
                    var dateTime = addDays.ShamsiDate();
                    if (dateTime.CompareTo(DateTime.Now.ShamsiDate()) < 0)
                    {
                        if (!workShopUserBo.Delete(connectionHandler, shopUser))
                            throw new Exception(Resources.Congress.ErrorInWorkDailyScheduler);
                        if (workShopUsers.InformList.All(x => x.obj.UserId != workShopUser.UserId))
                            workShopUsers.AddInform(
                            
                                workShopUser,
                                Resources.Congress.WorkShopChangeStatusEmail,
                                 Resources.Congress.WorkShopChangeStatusSMS
                            );

                        if (!workShopUser.User.ParentId.HasValue ||
                            workShopUsers.InformList.Any(x => x.obj.UserId == workShopUser.User.ParentId))
                            continue;
                        workShopUsers.AddInform(
                           
                            
                                 new WorkShopUser()
                                {
                                    UserId = (Guid)workShopUser.User.ParentId,
                                    WorkShopId = workShopUser.WorkShopId,
                                    Status = workShopUser.Status
                                }
                                , Resources.Congress.WorkShopChangeStatusEmail 
                               , Resources.Congress.WorkShopChangeStatusSMS
                            );



                    }
                }
            }
            result = true;
          
            return result;
        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartViewByDay(IConnectionHandler connectionHandler,
            string moth,
            string year, string url)
        {
            var fromdate = DateTimeUtil.ShamsiDateToGregorianDate(year + "/" + moth + "/" + 01);
            var maxday = (moth.CompareTo("06") <= 0 ? 31 : ((moth == "12") ? 29 : 30));
            var todate = DateTimeUtil.ShamsiDateToGregorianDate(year + "/" + moth + "/" + maxday);


            var listout = new List<Tools.ModelView.ReportChartModel>();
            var list = StatisticComponents.Instance.LogFacade.GroupBy(
                new Expression<Func<Statistics.DataStructure.Log, object>>[] { x => x.Date },
                new GroupByModel<Statistics.DataStructure.Log>[]
                {
                    new GroupByModel<Statistics.DataStructure.Log>()
                    {
                        Expression = x => x.Date,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                },
                x =>
                    x.Date.ToShortDateString().CompareTo(fromdate.ToShortDateString()) >= 0 &&
                    x.Date.ToShortDateString().CompareTo(todate.ToShortDateString()) <= 0 && x.Url.ToLower().Equals(url.ToLower()));

            var models = new List<Tools.ModelView.ReportChartModel>();
            foreach (var value in list)
            {
                DateTime date = value.Date;
                models.Add(new Tools.ModelView.ReportChartModel()
                {
                    Value = date.ShamsiDate().Substring(8, 2),
                    Count = value.CountDate
                });
            }


            for (var x = 1; x <= maxday; x++)
            {
                var number = x > 10 ? x.ToString() : "0" + x;
                var first = models.Where(model => model.Value == number);
                listout.Add(new Tools.ModelView.ReportChartModel()
                {
                    Count = first.Sum(model => model.Count),
                    Value = number
                });
            }
            return listout;




        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartViewByMonth(IConnectionHandler connectionHandler,
            string year, string url)
        {

            var fromdate = DateTimeUtil.ShamsiDateToGregorianDate(year + "/" + 01 + "/" + 01);
            var todate = DateTimeUtil.ShamsiDateToGregorianDate(year + "/" + 12 + "/" + 29);
            var listout = new List<Tools.ModelView.ReportChartModel>();
            var list = StatisticComponents.Instance.LogFacade.GroupBy(
                new Expression<Func<Statistics.DataStructure.Log, object>>[] { x => x.Date },
                new GroupByModel<Statistics.DataStructure.Log>[]
                {
                    new GroupByModel<Statistics.DataStructure.Log>()
                    {
                        Expression = x => x.Date,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                },
                x =>
                    x.Date.ToShortDateString().CompareTo(fromdate.ToShortDateString()) >= 0 &&
                    x.Date.ToShortDateString().CompareTo(todate.ToShortDateString()) <= 0 && x.Url.ToLower().Equals(url.ToLower()));
            var models = new List<Tools.ModelView.ReportChartModel>();
            foreach (var value in list)
            {
                DateTime date = value.Date;
                models.Add(new Tools.ModelView.ReportChartModel()
                {
                    Value = date.ShamsiDate().Substring(5, 2),
                    Count = value.CountDate
                });
            }

            var months = EnumUtils.ConvertEnumToIEnumerableInLocalization<Common.Definition.Enums.PersianMonth>()
                .Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>(
                            (byte)keyValuePair.Key.ToEnum<Common.Definition.Enums.PersianMonth>(),
                            keyValuePair.Value));
            foreach (var item in months)
            {
                var first = models.Where(x => x.Value.ToByte() == item.Key);
                listout.Add(new Tools.ModelView.ReportChartModel()
                {
                    Count = first.Sum(x => x.Count),
                    Value = ((Common.Definition.Enums.PersianMonth)item.Key).GetDescriptionInLocalization()
                });
            }



            return listout;




        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartNumberArticleByReferee(
            IConnectionHandler connectionHandler, Guid congressId)
        {
            var models = new List<Tools.ModelView.ReportChartModel>();
            var list = new RefereeCartableBO().GroupBy(connectionHandler,
                new Expression<Func<RefereeCartable, object>>[] { c => c.RefereeId },
                new GroupByModel<RefereeCartable>[]
                {
                    new GroupByModel<RefereeCartable>()
                    {
                        Expression = c=>c.ArticleId,
                        AggrigateFuntionType = AggrigateFuntionType.Count

                    },
                }, c => c.Referee.CongressId == congressId);

            var refList = new RefereeBO().Select(connectionHandler,
                new Expression<Func<Referee, object>>[]
                {
                    x => x.Id,
                    x =>
                        x.EnterpriseNode.RealEnterpriseNode.FirstName + " " +
                        x.EnterpriseNode.RealEnterpriseNode.LastName
                }, x => x.CongressId == congressId);

            foreach (var o in refList)
            {
                if (!(o.FirstNameAndLastName is string)) continue;
                var model = new Tools.ModelView.ReportChartModel { Value = o.FirstNameAndLastName };
                var firstOrDefault = list.FirstOrDefault(x => x.RefereeId == o.Id);
                if (firstOrDefault != null && firstOrDefault.CountArticleId is int)
                    model.Count = firstOrDefault.CountArticleId;
                else
                    model.Count = 0;
                models.Add(model);
            }

            return models;

        }

        public CongressXml GetXmlData(IConnectionHandler connectionHandler)
        {

            var homaXml = new CongressXml
            {

                AttendanceType = new List<KeyValueXml>()
                {
                    new KeyValueXml() {Key = "Congress", Value = "همایش"},
                    new KeyValueXml() {Key = "WorkShop", Value = "کارگاه"},
                }
            };
            var currentCongress = new HomaBO().GetCurrentCongress(connectionHandler);
            foreach (var homa in currentCongress)
            {
                var congressModelXml = new CongressModelXml() { CongressId = homa.Id, Title = homa.CongressTitle };
                var users = new UserBO().SelectKeyValuePair(connectionHandler, x => x.Number, x => x.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.EnterpriseNode.RealEnterpriseNode.LastName + "(" + x.Username + ")", x => x.CongressId == homa.Id);
                foreach (var user in users)
                {
                    congressModelXml.UserList.Add(new KeyValueXml() { Value = user.Value, Key = user.Key });
                }
                var @where = new WorkShopBO().Where(connectionHandler, x => x.CongressId == homa.Id);
                foreach (var shop in @where)
                {
                    var shopModelXml = new WorkShopModelXml() { Key = shop.Id.ToString(), Value = shop.Subject };
                    var shopUsers = new WorkShopUserBO().SelectKeyValuePair(connectionHandler, x => x.User.Number, x => x.User.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.User.EnterpriseNode.RealEnterpriseNode.LastName + "(" + x.User.Username + ")", x => x.WorkShopId == shop.Id);
                    foreach (var user in shopUsers)
                    {
                        shopModelXml.UserList.Add(new KeyValueXml() { Value = user.Value, Key = user.Key });
                    }
                    congressModelXml.WorkShopModelList.Add(shopModelXml);

                }
                homaXml.CongressModelXml.Add(congressModelXml);

            }
            return homaXml;


        }

        public bool SetXmlData(IConnectionHandler connectionHandler, string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            var deserialize = XmlModel.Serialize.JsonDeserialize<AttendanceXml>(data);
            var userBo = new UserBO();
            if (deserialize != null)
            {
                if (deserialize.CongressAbsentList == null) return true;
                foreach (var congressAbsentXml in deserialize.CongressAbsentList)
                {
                    foreach (var absentItem in congressAbsentXml.AbsentItem)
                    {

                        var firstOrDefault = userBo.FirstOrDefault(connectionHandler, x => x.Number == absentItem.Value.ToLong() & x.CongressId == congressAbsentXml.CongressId);
                        if (firstOrDefault == null)
                            continue;

                        firstOrDefault.Status = (byte)Enums.UserStatus.ConfirmPresentInHoma;
                        userBo.Update(connectionHandler, firstOrDefault);

                    }




                }
            }

            return true;
        }
    }
}
