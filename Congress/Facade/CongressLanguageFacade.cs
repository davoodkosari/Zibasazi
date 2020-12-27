using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressLanguageFacade : CongressBaseFacade<CongressLanguage>, ICongressLanguageFacade
    {
        internal CongressLanguageFacade()
        {
        }

        internal CongressLanguageFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContentBO = new CongressLanguageBO();
                if (!congressContentBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressContent);
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, Language language, HttpPostedFileBase lanuagelogo)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                string langId;
                var haslanguage = CommonComponent.Instance.LanguageFacade.Get(language.Id);
                if (haslanguage == null)
                {
                    if (
                        !CommonComponent.Instance.LanguageTransactionalFacade(this.CommonConnection)
                            .Insert(language, lanuagelogo))
                        throw new Exception("خطایی در ذخیره زبان وجود دارد");
                    langId = language.Id;
                }
                else langId = haslanguage.Id;
                var congressLanguage = new CongressLanguage {CongressId = congressId, LanguageId = langId};
                if (!new CongressLanguageBO().Insert(this.ConnectionHandler, congressLanguage))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressLanguage);
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<Language> GetValidList(Guid congressId)
        {
            try
            {
              
               return 
                    new CongressLanguageBO().Select(this.ConnectionHandler,x=>x.Language, x => x.CongressId == congressId&&x.Language.Enabled);
                    

               
                

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
