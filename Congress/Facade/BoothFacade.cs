using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class BoothFacade : CongressBaseFacade<Booth>, IBoothFacade
    {
        internal BoothFacade()
        {
        }

        internal BoothFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


        public override bool Delete(params object[] keys)
        {
            try
            {
                var obj = new BoothBO().Get(this.ConnectionHandler, keys);
                var list = new UserBoothBO().Any(ConnectionHandler,
                    supporter => supporter.BoothId == obj.Id);
                if (list)
                    throw new Exception(Resources.Congress.ErrorInDeleteBoothBecauseThisReserved);
                if (!new BoothBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteBooth);
                return true;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }


        }

        public IEnumerable<Booth> GetunusedByUserId(Guid userId, Guid congressId)
        {
            try
            {
               
                var lst = new BoothBO().GetunusedByUserId(this.ConnectionHandler, userId, congressId);
                new BoothBO().SetCapacity(ConnectionHandler, lst, congressId);
                return lst.Where(x => x.FreeCapicity > 0);
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

        

        public IEnumerable<Booth> GerReservableBooth(Guid congressId)
        {
            try
            {
                var lst = new BoothBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                new BoothBO().SetCapacity(ConnectionHandler,lst,congressId);
                return lst.Where(x=>x.FreeCapicity>0);
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

      
    

        public List<Booth> GetByCongressId(Guid congressId)
        {
            try
            {
                var list = new BoothBO().OrderBy(this.ConnectionHandler,x=>x.Code, x => x.CongressId == congressId);
                new BoothBO().SetCapacity(ConnectionHandler, list, congressId);
                return list;

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
