using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.ContentManager;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News;
using Radyn.Utility;
using Radyn.XmlModel;

namespace Radyn.Congress.Facade
{
    internal sealed class HomaFacade : CongressBaseFacade<Homa>, IHomaFacade
    {
        internal HomaFacade()
        {
        }

        internal HomaFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public bool ConfigByDefaulToHoma(Guid homaId)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                var defaulthoma = new HomaBO().FirstOrDefault(ConnectionHandler, x => x.IsDefaultForConfig);
                var configurationTranscationFacade = new ConfigurationBO();
                var configurationContentTranscationFacade = new ConfigurationContentBO();
                var fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (defaulthoma != null)
                {
                    var homa = new HomaBO().FirstOrDefault(ConnectionHandler, x => x.Id == homaId);
                    if (defaulthoma.Id == homa.Id) return true;
                    if (homa.Configuration == null || homa.Configuration.CongressId == Guid.Empty)
                    {
                        var configuration1 = new ConfigurationBO().Get(ConnectionHandler, defaulthoma.Id);
                        if (configuration1 != null)
                        {
                            configuration1.CongressId = homa.Id;
                            var configurationContents = new ConfigurationContentBO().Where(ConnectionHandler, x => x.ConfigurationId == defaulthoma.Id);
                            configurationTranscationFacade.Insert(ConnectionHandler, configuration1);
                            foreach (var configurationContent in configurationContents)
                            {
                                configurationContent.ConfigurationId = homa.Id;
                                if (configurationContent != null)
                                {

                                    if (configurationContent.AttachRefereeFile != null)
                                    {
                                        var AttachRefereeFile = Guid.NewGuid();
                                        configurationContent.AttachRefereeFile.Id = AttachRefereeFile;
                                        configurationContent.AttachRefereeFileId = AttachRefereeFile;
                                        fileTransactionalFacade.Insert(configurationContent.AttachRefereeFile);

                                    }
                                    if (configurationContent.BoothMapAttachment != null)
                                    {
                                        var BoothMapAttachment = Guid.NewGuid();
                                        configurationContent.BoothMapAttachment.Id = BoothMapAttachment;
                                        configurationContent.BoothMapAttachmentId = BoothMapAttachment;
                                        fileTransactionalFacade.Insert(configurationContent.BoothMapAttachment);

                                    }
                                    if (configurationContent.OrginalPoster != null)
                                    {
                                        var OrginalPoster = Guid.NewGuid();
                                        configurationContent.OrginalPoster.Id = OrginalPoster;
                                        configurationContent.OrginalPosterId = OrginalPoster;
                                        fileTransactionalFacade.Insert(configurationContent.OrginalPoster);

                                    }
                                    if (configurationContent.MiniPoster != null)
                                    {

                                        var MiniPoster = Guid.NewGuid();
                                        configurationContent.MiniPoster.Id = MiniPoster;
                                        configurationContent.MiniPosterId = MiniPoster;
                                        fileTransactionalFacade.Insert(configurationContent.MiniPoster);

                                    }
                                    if (configurationContent.Logo != null)
                                    {
                                        var logoId = Guid.NewGuid();
                                        configurationContent.Logo.Id = logoId;
                                        configurationContent.LogoId = logoId;
                                        fileTransactionalFacade.Insert(configurationContent.Logo);

                                    }
                                    if (configurationContent.Header != null)
                                    {
                                        var headerId = Guid.NewGuid();
                                        configurationContent.HeaderId = headerId;
                                        configurationContent.Header.Id = headerId;
                                        fileTransactionalFacade.Insert(configurationContent.Header);

                                    }
                                    if (configurationContent.Footer != null)
                                    {
                                        var logoId = Guid.NewGuid();
                                        configurationContent.FooterId = logoId;
                                        configurationContent.Footer.Id = logoId;
                                        fileTransactionalFacade.Insert(configurationContent.Footer);

                                    }
                                    if (configurationContent.HallMap != null)
                                    {
                                        var HallMap = Guid.NewGuid();
                                        configurationContent.HallMapId = HallMap;
                                        configurationContent.HallMap.Id = HallMap;
                                        fileTransactionalFacade.Insert(configurationContent.HallMap);

                                    }


                                }
                                configurationContentTranscationFacade.Insert(ConnectionHandler, configurationContent);
                            }


                        }
                    }
                    var congressLanguageBo = new CongressLanguageBO();
                    var congressLanguages = congressLanguageBo.Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressLanguages != null)
                    {
                        var congressMenuTranscationFacade = congressLanguageBo;
                        foreach (var congressLanguage in congressLanguages)
                        {
                            congressLanguage.CongressId = homa.Id;
                            congressMenuTranscationFacade.Insert(ConnectionHandler, congressLanguage);
                        }
                    }

                    var congressContentBo = new CongressContentBO();
                    var congressContents = congressContentBo.Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressContents != null)
                    {

                        var contentContentFacade = ContentManagerComponent.Instance.ContentTransactionalFacade(ConnectionHandler);
                        var contentFacade = ContentManagerComponent.Instance.ContentContentTransactionalFacade(ConnectionHandler);
                        foreach (var congressContent in congressContents)
                        {
                            var oldid = congressContent.ContentId;
                            var contents = contentFacade.Where(x => x.Id == oldid);
                            contentContentFacade.Insert(congressContent.Content);
                            foreach (var contentContent in contents)
                            {
                                contentContent.Id = congressContent.Content.Id;
                                contentFacade.Insert(contentContent);
                            }
                            congressContent.CongressId = homa.Id;
                            congressContent.ContentId = congressContent.Content.Id;
                            congressContentBo.Insert(ConnectionHandler, congressContent);
                        }
                    }

                    var congressContainers = new CongressContainerBO().Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressContainers != null)
                    {
                        var congressMenuTranscationFacade = new CongressContainerFacade(ConnectionHandler);
                        foreach (var congressMenu in congressContainers)
                        {

                            var newGuid = Guid.NewGuid();
                            congressMenu.Container.Id = newGuid;
                            congressMenu.ContainerId = newGuid;
                            congressMenu.CongressId = homa.Id;
                            congressMenuTranscationFacade.Insert(homa.Id, congressMenu.Container);

                        }
                    }
                    var congressMenus = new CongressMenuHtmlBO().Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressMenus != null)
                    {
                        var congressMenuTranscationFacade = new CongressMenuHtmlFacade(ConnectionHandler);
                        foreach (var congressMenu in congressMenus)
                        {

                            var newGuid = Guid.NewGuid();
                            congressMenu.MenuHtml.Id = newGuid;
                            congressMenu.MenuHtmlId = newGuid;
                            congressMenu.CongressId = homa.Id;
                            congressMenuTranscationFacade.Insert(homa.Id, congressMenu.MenuHtml);

                        }
                    }
                    var @where = new CongressHtmlBO().Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (@where != null)
                    {
                        var congressMenuTranscationFacade = new CongressHtmlFacade(ConnectionHandler);
                        foreach (var congressMenu in @where)
                        {
                            var oldId = congressMenu.HtmlDesginId;
                            var newGuid = Guid.NewGuid();
                            congressMenu.HtmlDesgin.Id = newGuid;
                            congressMenu.HtmlDesginId = newGuid;
                            congressMenu.CongressId = homa.Id;
                            congressMenuTranscationFacade.Insert(homa.Id, congressMenu.HtmlDesgin);
                            var partialLoadTransactionalFacade = ContentManagerComponent.Instance.PartialLoadTransactionalFacade(ConnectionHandler);
                            var list = partialLoadTransactionalFacade.Where(x => x.HtmlDesginId == oldId);
                            foreach (var partialLoad in list)
                            {
                                partialLoad.HtmlDesginId = newGuid;
                                partialLoadTransactionalFacade.Insert(partialLoad);
                            }
                        }
                    }

                    var congressId = new CongressMenuBO().Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressId != null)
                    {
                        var congressMenuTranscationFacade = new CongressMenuFacade(ConnectionHandler);
                        foreach (var congressMenu in congressId)
                        {

                            var newGuid = Guid.NewGuid();
                            congressMenu.Menu.Id = newGuid;
                            congressMenu.MenuId = newGuid;
                            congressMenu.CongressId = homa.Id;
                            congressMenuTranscationFacade.Insert(homa.Id, congressMenu.Menu, null);
                        }
                    }
                    var formAssigmentTransactionalFacade = FormGeneratorComponent.Instance.FormAssigmentTransactionalFacade(ConnectionHandler);
                    var congressFormses = new CongressFormsBO().Where(ConnectionHandler, x => x.CongressId == defaulthoma.Id);
                    if (congressFormses != null)
                    {
                        var congressFormsFacade = new CongressFormsFacade(ConnectionHandler);
                        foreach (var congressForms in congressFormses)
                        {
                            var oldId = congressForms.FomId;
                            var formAssigments = formAssigmentTransactionalFacade.Where(x => x.FormStructureId == oldId);
                            var newGuid = Guid.NewGuid();
                            var newGuidStructureFileId = Guid.NewGuid();
                            congressForms.FormStructure.Id = newGuid;
                            congressForms.FomId = newGuid;
                            congressForms.CongressId = homa.Id;
                            if (!string.IsNullOrEmpty(congressForms.FormStructure.StructureFileId))
                            {
                                var file=fileTransactionalFacade.Get(congressForms.FormStructure.StructureFileId);
                                if (file != null)
                                {
                                    file.Id = newGuidStructureFileId;
                                    fileTransactionalFacade.Insert(file);
                                    congressForms.FormStructure.StructureFileId = newGuidStructureFileId.ToString();
                                }
                            }
                            congressFormsFacade.Insert(homa.Id, congressForms.FormStructure);
                            foreach (var formAssigment in formAssigments)
                            {
                                formAssigment.FormStructureId = newGuid;
                                formAssigmentTransactionalFacade.Insert(formAssigment);
                            }
                        }
                    }

                }
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();

            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }

            return true;



        }
        public List<ModelView.SerachResultvalue> SearchInHoma(Guid congressId, Enums.SearchType SerachType, string txtvalue)
        {
            var resultvalues = new List<ModelView.SerachResultvalue>();
            if (string.IsNullOrEmpty(txtvalue)) return resultvalues;
            var homaBo = new HomaBO();
            switch (SerachType)
            {
                case Enums.SearchType.All:
                    homaBo.NewsSearch(ConnectionHandler, congressId, txtvalue, resultvalues);
                    homaBo.ContenetSearch(ConnectionHandler, congressId, txtvalue, resultvalues);
                    return resultvalues;
                    break;
                case Enums.SearchType.Content:
                    homaBo.ContenetSearch(ConnectionHandler, congressId, txtvalue, resultvalues);
                    return resultvalues;
                case Enums.SearchType.News:
                    homaBo.NewsSearch(ConnectionHandler, congressId, txtvalue, resultvalues);
                    return resultvalues;
            }
            return resultvalues;
        }



        public override bool Delete(params object[] keys)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.StatisticConnection.StartTransaction(IsolationLevel.ReadUncommitted);


                if (!new HomaBO().Delete(this.ConnectionHandler, this.EnterpriseNodeConnection, this.StatisticConnection, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongress);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.StatisticConnection.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.StatisticConnection.RollBack();

                throw new KnownException(ex.Message, ex);
            }

        }

        public void SendInform(Homa homa, byte type, Message.Tools.ModelView.MessageModel inform)
        {
            try
            {

                new HomaBO().SendInform(type, inform, homa.Configuration, homa.CongressTitle);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Homa homa,

            HttpPostedFileBase file, List<HomaAlias> homaAliases)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.StatisticConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                if (!new HomaBO().Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, this.StatisticConnection, homa, file, homaAliases))
                    throw new Exception(Resources.Congress.ErrorInSaveCongress);

                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.StatisticConnection.CommitTransaction();

            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.StatisticConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }


            return true;
        }

        internal IEnumerable<ModelView.ReportChartModel> ChartNumberArticleByReferee(Guid congressId)
        {
            try
            {
                return new HomaBO().ChartNumberArticleByReferee(this.ConnectionHandler, congressId);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Homa homa,
             HttpPostedFileBase file, List<HomaAlias> homaAliases)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.StatisticConnection.StartTransaction(IsolationLevel.ReadUncommitted);


                if (!new HomaBO().Update(this.ConnectionHandler, this.EnterpriseNodeConnection, this.StatisticConnection, homa, file, homaAliases))
                    throw new Exception(Resources.Congress.ErrorInSaveCongress);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.StatisticConnection.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.StatisticConnection.RollBack();

                throw new KnownException(ex.Message, ex);
            }

        }

        public Homa GetUrlHomaId(string authority, string url)
        {
            try
            {

                return new HomaBO().GetUrlHomaId(this.ConnectionHandler, authority, url);



            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }
        public async Task<Homa> GetUrlHomaIdAsync(string authority, string url)
        {
            try
            {

                return await new HomaBO().GetUrlHomaIdAsync(this.ConnectionHandler, authority, url);



            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool HasLicense()
        {
            try
            {


                return new HomaBO().HasLicense(this.ConnectionHandler);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }

        }
        public async Task<bool> HasLicenseAsync()
        {
            try
            {


                return await new HomaBO().HasLicenseAsync(this.ConnectionHandler);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }

        }

        public List<Homa> GetLastCongress()
        {
            try
            {

                return new HomaBO().GetLastCongress(this.ConnectionHandler);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<Homa> GetCurrentCongress()
        {
            try
            {

                return new HomaBO().GetCurrentCongress(this.ConnectionHandler);

            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }



        public List<Homa> GetNextCongress()
        {
            try
            {
                return new HomaBO().GetNextCongress(this.ConnectionHandler);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }


        public IEnumerable<ModelView.ReportChartModel> ChartChash(Guid congressId, string year = "", string month = "")
        {
            try
            {
                return new HomaBO().ChartChash(this.ConnectionHandler, congressId, year, month);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }
        }


        public void DailyEvaulation()
        {
            var workShopUserBo = new WorkShopUserBO();
            var hotelUserBo = new HotelUserBO();
            var userBoothBo = new UserBoothBO();
            var keyValuePairs = new Dictionary<Guid, ModelView.ModifyResult<UserBooth>>();
            var workShopUsers = new Dictionary<Guid, ModelView.ModifyResult<WorkShopUser>>();
            var hotelUsers = new Dictionary<Guid, ModelView.ModifyResult<HotelUser>>();
            bool result = false;
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new HomaBO().DailyEvaulation(this.ConnectionHandler, keyValuePairs, workShopUsers, hotelUsers))
                    throw new Exception(Resources.Congress.ErrorInWorkDailyScheduler);
                this.ConnectionHandler.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                {
                    foreach (var modifyResult in hotelUsers)
                    {

                        hotelUserBo.InformHotelReserv(ConnectionHandler, modifyResult.Key, modifyResult.Value.InformList);
                    }
                    foreach (var modifyResult in workShopUsers)
                    {

                        workShopUserBo.InformWorkShopReserv(ConnectionHandler, modifyResult.Key, modifyResult.Value.InformList);
                    }
                    foreach (var modifyResult in keyValuePairs)
                    {

                        userBoothBo.InformUserboothReserv(ConnectionHandler, modifyResult.Key, modifyResult.Value.InformList);
                    }

                }
            }
            catch (Exception)
            {


            }
        }

        public CongressXml GetXmlData()
        {

            try
            {
                return new HomaBO().GetXmlData(this.ConnectionHandler);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool SetXmlData(string data)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new HomaBO().SetXmlData(this.ConnectionHandler, data))
                    throw new Exception(Resources.Congress.ErrorInWorkDailyScheduler);
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<Tools.ModelView.ReportChartModel> ChartViewByDay(string moth,
     string year, string url)
        {
            try
            {
                return new HomaBO().ChartViewByDay(this.ConnectionHandler, moth, year, url);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<Tools.ModelView.ReportChartModel> ChartViewByMonth(string year, string url)
        {
            try
            {
                return new HomaBO().ChartViewByMonth(this.ConnectionHandler, year, url);
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

    }
}
