using System;
using System.Data;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.ContentManager.BO;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.ContentManager.Facade
{
    internal sealed class ContainerFacade : ContentManagerBaseFacade<Container>, IContainerFacade
    {
        internal ContainerFacade()
        {
        }

        internal ContainerFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
               
                var containerBO = new ContainerBO();
                var obj = containerBO.Get(this.ConnectionHandler, keys);
                var contents = new ContentBO().Any(ConnectionHandler,
                    container => container.ContainerId == obj.Id);
                if (contents)
                {
                    throw new Exception("این قالب  در محتوا استفاده شده است و قابل حذف نیست");
                }
                if (!containerBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف  قالب وجود دارد");
               
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

      

       
    }
}
