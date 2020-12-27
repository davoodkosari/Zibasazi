using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressDefinitionFacade : CongressBaseFacade<CongressDefinition>, ICongressDefinitionFacade
    {
        internal CongressDefinitionFacade()
        {
        }

        internal CongressDefinitionFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public CongressDefinition GetValidDefinition(Guid congressId)
        {
            try
            {


                return new CongressDefinitionBO().GetValidDefinition(this.ConnectionHandler, congressId);

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

        public List<Enums.CongressDefinitionReportTypes> CongressDefinitionGetReportList(Guid homaId)
        {
            try
            {
                var dictionary = new List<Enums.CongressDefinitionReportTypes>();
                var congressDefinition = new CongressDefinitionBO().GetValidDefinition(this.ConnectionHandler, homaId);
                if (congressDefinition.RptBoothOfficerId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptBoothOfficer);
                if (congressDefinition.RptHotelUserId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptHotelUser);
                if (congressDefinition.RptMiniUserCardId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptMiniUserCard);
                if (congressDefinition.RptUserBoothId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptUserBooth);
                if (congressDefinition.RptUserCardId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptUserCard);
                if (congressDefinition.RptAbstractArticleId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptAbstractArticle);
                if (congressDefinition.RptUserId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptUser);
                if (congressDefinition.RptWorkShopUserId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptWorkShopUser);
                if (congressDefinition.RptArticleCertificateId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptArticleCertificate);
                if (congressDefinition.RptArticleId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptArticle);
                if (congressDefinition.RptChipFoodId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptChipFood);
                if (congressDefinition.RptCongressCertificateId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptCongressCertificate);
                if (congressDefinition.RptAbstractArticleId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptAbstractArticle);
                if (congressDefinition.RptUserInfoCardId.HasValue)
                    dictionary.Add(Enums.CongressDefinitionReportTypes.RptUserInfoCard);
                return dictionary;

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

        public bool ResetFactoryList(Guid homaId, List<string> list)
        {
            try
            {
                var congressDefinition = new CongressDefinitionBO().GetValidDefinition(this.ConnectionHandler, homaId);
                foreach (var variable in list)
                {
                    var value = variable.ToEnum<Enums.CongressDefinitionReportTypes>();
                    switch (value)
                    {
                        case Enums.CongressDefinitionReportTypes.RptBoothOfficer:
                            congressDefinition.RptBoothOfficerId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptHotelUser:
                            congressDefinition.RptHotelUserId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptMiniUserCard:
                            congressDefinition.RptUserCardId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptUserBooth:
                            congressDefinition.RptUserBoothId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptUserCard:
                            congressDefinition.RptUserCardId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptUser:
                            congressDefinition.RptUserId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptWorkShopUser:
                            congressDefinition.RptWorkShopUserId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptArticleCertificate:
                            congressDefinition.RptArticleCertificateId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptArticle:
                            congressDefinition.RptArticleId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptChipFood:
                            congressDefinition.RptChipFoodId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptCongressCertificate:
                            congressDefinition.RptCongressCertificateId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptAbstractArticle:
                            congressDefinition.RptAbstractArticleId = null;
                            break;
                        case Enums.CongressDefinitionReportTypes.RptUserInfoCard:
                            congressDefinition.RptUserInfoCardId = null;
                            break;

                    }
                }
                if (!new CongressDefinitionBO().Update(this.ConnectionHandler, congressDefinition))
                    throw new Exception("خطایی در ویرایش تنظیمات وجود دارد");
                return true;
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

        public bool ModifyReports(CongressDefinition congressDefinition, Dictionary<string, HttpPostedFileBase> dictionary)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                var fileTransactionalFacade =
                     FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                foreach (var httpPostedFileBase in dictionary)
                {
                    if (httpPostedFileBase.Value == null) continue;
                    var property = congressDefinition.GetType().GetProperty(httpPostedFileBase.Key);
                    if (property != null)
                    {
                        var value = property.GetValue(congressDefinition, null);
                        if (value == null)
                            property.SetValue(congressDefinition, fileTransactionalFacade.Insert(httpPostedFileBase.Value), null);
                        else
                        {
                            if (!fileTransactionalFacade.Update(httpPostedFileBase.Value, (Guid)value))
                                throw new Exception(Resources.Congress.ErrorInEditCongress);
                        }
                    }
                }

                if (!new CongressDefinitionBO().Update(this.ConnectionHandler, congressDefinition))
                    throw new Exception(Resources.Congress.ErrorInEditConfuguration);

                this.FileManagerConnection.CommitTransaction();
                this.ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
