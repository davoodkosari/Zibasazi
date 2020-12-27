using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class ConfigurationFacade : CongressBaseFacade<Configuration>, IConfigurationFacade
    {
        internal ConfigurationFacade()
        {
        }

        internal ConfigurationFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }





        public bool Insert(Configuration configuration, ConfigurationContent configurationContent,

             HttpPostedFileBase refreeAttachment, HttpPostedFileBase boothMapAttachmentId,
            HttpPostedFileBase orginalPoster, HttpPostedFileBase miniPoster, HttpPostedFileBase logo,
            HttpPostedFileBase header,
            HttpPostedFileBase footer, HttpPostedFileBase hallMapId, HttpPostedFileBase backgroundImageId, HttpPostedFileBase favIcon, List<DiscountTypeSection> sectiontypes)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                var fileTransactionalFacade =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (favIcon != null)
                    configuration.FavIcon = fileTransactionalFacade.Insert(favIcon);
                if (backgroundImageId != null)
                {
                    configuration.BackgroundImage = fileTransactionalFacade.Insert(backgroundImageId);
                    configuration.BackgroundColor = null;
                }
                if (!string.IsNullOrEmpty(configuration.BackgroundColor))
                    configuration.BackgroundImage = null;
                if (!new ConfigurationBO().Insert(this.ConnectionHandler, configuration))
                    throw new Exception(Resources.Congress.ErrorInSaveConfiguartion);
                if (configurationContent != null)
                {

                    if (refreeAttachment != null)
                        configurationContent.AttachRefereeFileId = fileTransactionalFacade.Insert(refreeAttachment);
                    if (boothMapAttachmentId != null)
                        configurationContent.BoothMapAttachmentId = fileTransactionalFacade.Insert(boothMapAttachmentId);
                    if (orginalPoster != null)
                        configurationContent.OrginalPosterId = fileTransactionalFacade.Insert(orginalPoster);
                    if (miniPoster != null)
                        configurationContent.MiniPosterId = fileTransactionalFacade.Insert(miniPoster);
                    if (logo != null)
                        configurationContent.LogoId = fileTransactionalFacade.Insert(logo);
                    if (header != null)
                        configurationContent.HeaderId = fileTransactionalFacade.Insert(header);
                    if (footer != null)
                        configurationContent.FooterId = fileTransactionalFacade.Insert(footer);
                    if (hallMapId != null)
                        configurationContent.HallMapId = fileTransactionalFacade.Insert(hallMapId);
                    configurationContent.ConfigurationId = configuration.CongressId;
                    if (!new ConfigurationContentBO().Insert(this.ConnectionHandler, configurationContent))
                        throw new Exception(Resources.Congress.ErrorInSaveConfiguartion);
                }

                if (
                    !PaymentComponenets.Instance.DiscountTypeSectionTransactionalFacade(this.ContentManagerConnection)
                        .Insert(sectiontypes))
                    return false;


                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                this.PaymentConnection.CommitTransaction();

                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                this.FileManagerConnection.RollBack();
                this.PaymentConnection.RollBack();

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                this.FileManagerConnection.RollBack();
                this.PaymentConnection.RollBack();

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Configuration configuration, ConfigurationContent configurationContent,
           HttpPostedFileBase refreeAttachment,
            HttpPostedFileBase boothMapAttachmentId, HttpPostedFileBase orginalPoster, HttpPostedFileBase miniPoster,
            HttpPostedFileBase logo, HttpPostedFileBase header, HttpPostedFileBase footer, HttpPostedFileBase hallMapId, HttpPostedFileBase backgroundImageId, HttpPostedFileBase favIcon, string modelName,
            List<DiscountTypeSection> sectiontypes)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var fileTransactionalFacade =
                     FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                var configurationBo = new ConfigurationBO();
                var oldconfig = configurationBo.Get(this.ConnectionHandler, configuration.CongressId);
                configuration.TerminalPassword = !String.IsNullOrEmpty(configuration.TerminalPassword)
                    ? StringUtils.Encrypt(configuration.TerminalPassword)
                    : oldconfig.TerminalPassword;
                configuration.SMSAccountPassword = !String.IsNullOrEmpty(configuration.SMSAccountPassword)
                    ? StringUtils.Encrypt(configuration.SMSAccountPassword)
                    : oldconfig.SMSAccountPassword;
                configuration.MailPassword = !String.IsNullOrEmpty(configuration.MailPassword)
                    ? StringUtils.Encrypt(configuration.MailPassword)
                    : oldconfig.MailPassword;
                configuration.CertificatePassword = !String.IsNullOrEmpty(configuration.CertificatePassword)
                    ? StringUtils.Encrypt(configuration.CertificatePassword)
                    : oldconfig.CertificatePassword;


                //---------------------
                configuration.MerchantPublicKey = !String.IsNullOrEmpty(configuration.MerchantPublicKey)
                ? StringUtils.Encrypt(configuration.MerchantPublicKey)
                : oldconfig.MerchantPublicKey;


                configuration.MerchantPrivateKey = !String.IsNullOrEmpty(configuration.MerchantPrivateKey)
                ? StringUtils.Encrypt(configuration.MerchantPrivateKey)
                : oldconfig.MerchantPrivateKey;
                //------------------------




                if (
                    !PaymentComponenets.Instance.DiscountTypeSectionTransactionalFacade(this.ContentManagerConnection)
                        .Update(modelName, sectiontypes))
                    throw new Exception(Resources.Congress.ErrorInEditConfuguration);
                if (favIcon != null)
                {
                    if (configuration.FavIcon == null)
                        configuration.FavIcon = fileTransactionalFacade.Insert(favIcon);
                    else
                    {
                        if (!fileTransactionalFacade.Update(favIcon, (Guid)configuration.FavIcon))
                            throw new Exception("خطا در ذخیره fav icon وجود دارد");
                    }
                }
                if (backgroundImageId != null)
                {
                    if (configuration.BackgroundImage == null)
                        configuration.BackgroundImage = fileTransactionalFacade.Insert(backgroundImageId);
                    else
                    {
                        if (!fileTransactionalFacade.Update(backgroundImageId, (Guid)configuration.BackgroundImage))
                            throw new Exception("خطایی در ذخیره عکس پشت زمینه وجود دارد");
                    }
                    configuration.BackgroundColor = null;
                }
                if (!string.IsNullOrEmpty(configuration.BackgroundColor))
                    configuration.BackgroundImage = null;
                if (!configurationBo.Update(this.ConnectionHandler, configuration))
                    throw new Exception(Resources.Congress.ErrorInEditConfuguration);

                if (configurationContent != null)
                {

                    if (refreeAttachment != null)
                    {
                        if (configurationContent.AttachRefereeFileId == null)
                            configurationContent.AttachRefereeFileId = fileTransactionalFacade.Insert(refreeAttachment);
                        else
                        {
                            if (
                                !fileTransactionalFacade.Update(refreeAttachment,
                                    (Guid)configurationContent.AttachRefereeFileId))
                                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleOrginalFile, configuration.ArticleTitle));
                        }

                    }
                    if (boothMapAttachmentId != null)
                    {
                        if (configurationContent.BoothMapAttachmentId == null)
                            configurationContent.BoothMapAttachmentId =
                                fileTransactionalFacade.Insert(boothMapAttachmentId);
                        else
                        {
                            if (
                                !fileTransactionalFacade.Update(boothMapAttachmentId,
                                    (Guid)configurationContent.BoothMapAttachmentId))
                                throw new Exception(Resources.Congress.ErrorInEditBoothMapFile);
                        }

                    }
                    if (orginalPoster != null)
                    {
                        if (configurationContent.OrginalPosterId == null)
                            configurationContent.OrginalPosterId = fileTransactionalFacade.Insert(orginalPoster);
                        else
                        {
                            if (
                                !fileTransactionalFacade.Update(orginalPoster,
                                    (Guid)configurationContent.OrginalPosterId))
                                throw new Exception(string.Format(Resources.Congress.ErrorInEditArticleOrginalFile, configuration.ArticleTitle));
                        }
                    }
                    if (miniPoster != null)
                    {
                        if (configurationContent.MiniPosterId == null)
                            configurationContent.MiniPosterId = fileTransactionalFacade.Insert(miniPoster);
                        else
                        {
                            if (!fileTransactionalFacade.Update(miniPoster, (Guid)configurationContent.MiniPosterId))
                                throw new Exception(Resources.Congress.ErrorInEditMiniPosterFile);
                        }
                    }
                    if (logo != null)
                    {

                        if (configurationContent.LogoId == null)
                            configurationContent.LogoId = fileTransactionalFacade.Insert(logo);
                        else
                        {
                            if (!fileTransactionalFacade.Update(logo, (Guid)configurationContent.LogoId))
                                throw new Exception(Resources.Congress.ErrorInEditLogoFile);
                        }
                    }
                    if (header != null)
                    {
                        if (configurationContent.HeaderId == null)
                            configurationContent.HeaderId = fileTransactionalFacade.Insert(header);
                        else
                        {
                            if (!fileTransactionalFacade.Update(header, (Guid)configurationContent.HeaderId))
                                throw new Exception(Resources.Congress.ErrorInEditHeaderFile);
                        }
                    }
                    if (footer != null)
                    {
                        if (configurationContent.FooterId == null)
                            configurationContent.FooterId = fileTransactionalFacade.Insert(footer);
                        else
                        {
                            if (!fileTransactionalFacade.Update(footer, (Guid)configurationContent.FooterId))
                                throw new Exception(Resources.Congress.ErrorInEditFooterFile);
                        }
                    }
                    if (hallMapId != null)
                    {
                        if (configurationContent.HallMapId == null)
                            configurationContent.HallMapId = fileTransactionalFacade.Insert(hallMapId);
                        else
                        {
                            if (!fileTransactionalFacade.Update(footer, (Guid)configurationContent.HallMapId))
                                throw new Exception(Resources.Congress.ErrorInEditHallmapFile);
                        }
                    }

                    if (configurationContent.ConfigurationId == Guid.Empty)
                    {
                        configurationContent.ConfigurationId = configuration.CongressId;
                        if (!new ConfigurationContentBO().Insert(this.ConnectionHandler, configurationContent))
                            throw new Exception(Resources.Congress.ErrorInSaveConfiguartion);
                    }
                    else
                    {
                        var oldobj = new ConfigurationContentBO().Get(this.ConnectionHandler,configurationContent.ConfigurationId, configurationContent.LanguageId);
                        if (oldobj != null)
                        {
                            if (oldobj.HeaderId.HasValue && configurationContent.HeaderId == null)
                                fileTransactionalFacade.Delete(oldobj.HeaderId);
                            if (oldobj.FooterId.HasValue && configurationContent.FooterId == null)
                                fileTransactionalFacade.Delete(oldobj.FooterId);
                            if (oldobj.LogoId.HasValue && configurationContent.LogoId == null)
                                fileTransactionalFacade.Delete(oldobj.LogoId);
                            if (oldobj.OrginalPosterId.HasValue && configurationContent.OrginalPosterId == null)
                                fileTransactionalFacade.Delete(oldobj.OrginalPosterId);
                            if (oldobj.MiniPosterId.HasValue && configurationContent.MiniPosterId == null)
                                fileTransactionalFacade.Delete(oldobj.MiniPosterId);
                            if (oldobj.AttachRefereeFileId.HasValue && configurationContent.AttachRefereeFileId == null)
                                fileTransactionalFacade.Delete(oldobj.AttachRefereeFileId);
                            if (oldobj.BoothMapAttachmentId.HasValue && configurationContent.BoothMapAttachmentId == null)
                                fileTransactionalFacade.Delete(oldobj.BoothMapAttachmentId);
                        }
                        if (!new ConfigurationContentBO().Update(this.ConnectionHandler, configurationContent))
                            throw new Exception(Resources.Congress.ErrorInEditConfuguration);
                    }
                }
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.ContentManagerConnection.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.ContentManagerConnection.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public Configuration ValidConfig(Guid congressId)
        {
            try
            {
                return new ConfigurationBO().ValidConfig(this.ConnectionHandler, congressId);
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
