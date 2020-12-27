using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Common;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class LanguageFacade : WebDesignBaseFacade<Language>, ILanguageFacade
    {
        internal LanguageFacade() { }

        internal LanguageFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContentBo = new LanguageBO();
                if (!congressContentBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف زبان وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<Common.DataStructure.Language> GetValidList(Guid websiteId)
        {
            try
            {
               return new LanguageBO().Select(this.ConnectionHandler, x => x.WebSiteLanguage, x => x.WebId == websiteId&&x.WebSiteLanguage.Enabled);
               
                

            }
            catch (KnownException knownException)
            {

                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {

                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<Common.DataStructure.Language> GetByWebSiteId(Guid websiteId)
        {
            try
            {
               
               return new LanguageBO().Select(this.ConnectionHandler, x => x.WebSiteLanguage, x => x.WebId == websiteId);
               

            }
            catch (KnownException knownException)
            {

                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {

                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid websiteId, Common.DataStructure.Language language, HttpPostedFileBase image)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                string langId;
                var haslanguage = CommonComponent.Instance.LanguageFacade.Get(language.Id);
                if (haslanguage == null)
                {
                    if (!CommonComponent.Instance.LanguageTransactionalFacade(this.CommonConnection).Insert(language, image))
                        throw new Exception("خطایی در ذخیره زبان وجود دارد");
                    langId = language.Id;
                }
                else langId = haslanguage.Id;
                var congressLanguage = new Language { WebId = websiteId, LanguageId = langId };
                if (!new LanguageBO().Insert(this.ConnectionHandler, congressLanguage))
                    throw new Exception("خطایی در ذخیره زبان وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.CommonConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
