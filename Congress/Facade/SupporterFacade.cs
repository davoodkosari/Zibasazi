using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class SupporterFacade : CongressBaseFacade<Supporter>, ISupporterFacade
    {
        internal SupporterFacade()
        {
        }

        internal SupporterFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

       

        public bool Insert(Supporter supporter, HttpPostedFileBase file)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                if (file != null)
                {

                    supporter.Image =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Insert(file, new File() { MaxSize = 200 }).ToString();
                }
                if (!new SupporterBO().Insert(this.ConnectionHandler, supporter))
                    throw new Exception(Resources.Congress.ErrorInSaveSupporter);

                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();

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

        public bool Update(Supporter supporter, HttpPostedFileBase file)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (file != null)
                {
                    var fileTransactionalFacade =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                 var lanuageContent=  new SupporterBO().GetLanuageContent(base.ConnectionHandler,
                       supporter.CurrentUICultureName, supporter.Id);
                    if (!string.IsNullOrEmpty(lanuageContent.Image))
                    {
                        fileTransactionalFacade
                            .Update(file, lanuageContent.Image.ToGuid());
                    }
                    else
                    {
                        supporter.Image =
                        fileTransactionalFacade
                            .Insert(file, new File() { MaxSize = 200 }).ToString();
                    }
                }


                if (!new SupporterBO().Update(this.ConnectionHandler, supporter))
                    throw new Exception(Resources.Congress.ErrorInEditSupporter);
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
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

        public IEnumerable<ModelView.ReportChartModel> ChartKindOfSupport(Guid congressId)
        {
            try
            {
                return new SupporterBO().ChartNumberStandsWithReservationSeparation(this.ConnectionHandler, congressId);
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
