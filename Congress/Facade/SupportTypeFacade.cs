using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class SupportTypeFacade : CongressBaseFacade<SupportType>, ISupportTypeFacade
    {
        internal SupportTypeFacade()
        {
        }

        internal SupportTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public override bool Delete(params object[] keys)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               
                var supportTypeBo = new SupportTypeBO();
                var obj = supportTypeBo.Get(this.ConnectionHandler, keys);
                var url = obj.PartialUrl();
                var list = new SupporterBO().Any(ConnectionHandler,
                    supporter => supporter.SupportTypeId == obj.Id);
                if (list)
                    throw new Exception(Resources.Congress.ErrorInDeleteSupportTypeBecuaseThereAreSupportWithThisType);
                var configurationSupportTypes = new ConfigurationSupportTypeBO().Any(ConnectionHandler,
                    supporter => supporter.SupportTypeId == obj.Id);
                if (configurationSupportTypes)
                    throw new Exception(Resources.Congress.ErrorInEditConfigurationBecauseThereErrorInDeleteSupportType);
                if (
                    !ContentManager.ContentManagerComponent.Instance.PartialsTransactionalFacade(
                        this.ContentManagerConnection).DeletePartialWithUrl(url))
                    return false;
                if (!supportTypeBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteSupportType);
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                
                return true;

            }

            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
               
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
               
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

      

        public bool Update(Guid congressId,SupportType supportType)
        {
            try
            {
                var supportTypeBo = new SupportTypeBO();
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!supportTypeBo.Update(this.ConnectionHandler, supportType))
                    throw new Exception(Resources.Congress.ErrorInEditSupportType);
              
                if (!supportTypeBo.ModifySupportTypes(this.ConnectionHandler, this.ContentManagerConnection, congressId,supportType))
                    throw new Exception(Resources.Congress.ErrorInSaveSupportType);
                
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, SupportType supportType)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
             
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var supportTypeBo = new SupportTypeBO();
                if (!supportTypeBo.Insert(this.ConnectionHandler, supportType))
                    throw new Exception(Resources.Congress.ErrorInSaveSupportType);
                if (!supportTypeBo.ModifySupportTypes(this.ConnectionHandler, this.ContentManagerConnection, congressId,supportType))
                    throw new Exception(Resources.Congress.ErrorInSaveSupportType);
                this.ConnectionHandler.CommitTransaction();
              
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<KeyValuePair<SupportType, bool>> GetSupportTypeModel(Guid congressId)
        {
            try
            {
                var list = new SupportTypeBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                var getSupportTypeModel = new List<KeyValuePair<SupportType, bool>>();
                var configurationSupportTypeBo = new ConfigurationSupportTypeBO();
                var @select = configurationSupportTypeBo.Select(ConnectionHandler, x => x.SupportTypeId,
                    x => x.CongressId == congressId);
                foreach (var supportType in list)
                {
                    bool added = @select.Any(x=>x.Equals(supportType.Id));
                    getSupportTypeModel.Add(new KeyValuePair<SupportType, bool>(supportType, added));
                }
                return getSupportTypeModel;
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

        public SupportType GetSupportWithSupporters(short id)
        {
            try
            {

                var supportType = new SupportTypeBO().Get(this.ConnectionHandler, id);
                if (supportType == null) return null;
                var list = new SupporterBO().OrderBy(this.ConnectionHandler, x=>x.Sort,x => x.SupportTypeId == id);
                switch (supportType.ShowType)
                {
                    case (byte) Enums.SupporterShowType.Image:
                        supportType.Supporters = list.Where(x => x.Image != null).ToList();
                        break;
                    case (byte) Enums.SupporterShowType.ImageAndTitle:
                        supportType.Supporters = list;
                        break;
                    case (byte) Enums.SupporterShowType.OnlyTitle:
                        supportType.Supporters = list.Where(x => !string.IsNullOrEmpty(x.Title)).ToList();
                        break;
                }
                return supportType;
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
        }public async Task<SupportType> GetSupportWithSupportersAsync(short id)
        {
            try
            {

                var supportType =await new SupportTypeBO().GetAsync(this.ConnectionHandler, id);
                if (supportType == null) return null;
                var list =await new SupporterBO().OrderByAsync(this.ConnectionHandler, x=>x.Sort,x => x.SupportTypeId == id);
                switch (supportType.ShowType)
                {
                    case (byte) Enums.SupporterShowType.Image:
                        supportType.Supporters = list.Where(x => x.Image != null).ToList();
                        break;
                    case (byte) Enums.SupporterShowType.ImageAndTitle:
                        supportType.Supporters = list;
                        break;
                    case (byte) Enums.SupporterShowType.OnlyTitle:
                        supportType.Supporters = list.Where(x => !string.IsNullOrEmpty(x.Title)).ToList();
                        break;
                }
                return supportType;
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
