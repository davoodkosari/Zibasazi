using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.ContentManager.BO;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.ContentManager.Facade.Interface;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security;
using Radyn.Security.DataStructure;
using Radyn.Utility;

namespace Radyn.ContentManager.Facade
{
    internal sealed class PartialsFacade : ContentManagerBaseFacade<Partials>, IPartialsFacade
    {
        internal PartialsFacade()
        {
        }

        internal PartialsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var partialsBO = new PartialsBO();
                if (!partialsBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف صفحه وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool DeletePartialWithUrl(string url)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var partialsBO = new PartialsBO();
                var @where = partialsBO.Where(ConnectionHandler, partials => partials.Url == url.ToLower());
                if (!@where.Any()) return true;
                foreach (var partialse in where)
                {
                    if (!partialsBO.Delete(this.ConnectionHandler, partialse.Id))
                        throw new Exception("خطایی در حذف صفحه وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

       
        

        public List<Partials> GetContentPartials(IEnumerable<int> idlist, string culture)
        {

            try
            {

                var contentBo = new ContentBO();
                var contents = contentBo.Where(ConnectionHandler, x => x.Id.In(idlist));
                return new PartialsBO().GetContentPartials(ConnectionHandler, contents, culture);
            }
            catch (KnownException ex)
            {
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<Partials> GetOperationPartials(Guid OperationId)
        {
            try
            {
                return new PartialsBO().GetOperationPartials(ConnectionHandler, OperationId);

            }
            catch (KnownException ex)
            {
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<Partials> GetContentPartials(string culture)
        {
            try
            {

                var contentBo = new ContentBO();
                var contents = contentBo.Where(ConnectionHandler, x => x.Enabled && x.IsExternal == false);
                return new PartialsBO().GetContentPartials(ConnectionHandler, contents, culture);
            }
            catch (KnownException ex)
            {
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
