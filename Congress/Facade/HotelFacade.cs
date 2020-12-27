using System;
using System.Collections.Generic;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class HotelFacade : CongressBaseFacade<Hotel>, IHotelFacade
    {
        internal HotelFacade()
        {
        }

        internal HotelFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                var obj = new HotelBO().Get(this.ConnectionHandler, keys);
                var list = new HotelUserBO().Any(ConnectionHandler,
                    supporter => supporter.HotelId == obj.Id);
                if (list)
                    throw new Exception(Resources.Congress.ErrorInDeleteHotelBecauseThisReserved);
                if (!new HotelBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteHotel);
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

      

        public List<Hotel> GetByCongressId(Guid congressId)
        {
            try
            {
                var lst = new HotelBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                new HotelBO().SetCapacity(ConnectionHandler, lst, congressId);
              
                return lst;
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
        public IEnumerable<Hotel> GetReservableHotel(Guid congressId)
        {
            try
            {
                var lst = new HotelBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                new HotelBO().SetCapacity(ConnectionHandler, lst, congressId);
                var hotels = new List<Hotel>();
                foreach (var hotel in lst)
                {
                    if (string.IsNullOrEmpty(hotel.Name)) continue;
                    if (hotel.FreeCapicity > 0)
                        hotels.Add(hotel);
                }
                return hotels;
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
