using System;
using System.Data;
using System.Web;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class ConfigurationFacade : WebDesignBaseFacade<Configuration>, IConfigurationFacade
    {
        internal ConfigurationFacade() { }

        internal ConfigurationFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public bool Insert(Configuration configuration, HttpPostedFileBase favIcon)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                var fileTransactionalFacade =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (favIcon != null)
                    configuration.FavIcon = fileTransactionalFacade.Insert(favIcon);

                if (!new ConfigurationBO().Insert(this.ConnectionHandler, configuration))
                    throw new Exception("خطای در ذخیره اطلاعات رخ داده است");



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

        public bool Update(Configuration configuration, HttpPostedFileBase favIcon)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var fileTransactionalFacade =
                     FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                var configurationBo = new ConfigurationBO();
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
                if (!configurationBo.Update(this.ConnectionHandler, configuration))
                    throw new Exception("خطای در ذخیره اطلاعات رخ داده است");

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


    }
}
