using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.EnterpriseNode;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.BO;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;
using Radyn.Reservation.Facade.Interface;
using Radyn.Utility;

namespace Radyn.Reservation.Facade
{
    internal sealed class ChairFacade : ReservationBaseFacade<Chair>, IChairFacade
    {
        internal ChairFacade() { }

        internal ChairFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

     

        public bool ChangeStatusAndSetOwner(Guid chairId, Enums.ReservStatus reservStatus, Guid? ownerId = null)
        {
            try
            {
                var chair = new ChairBO().Get(this.ConnectionHandler, chairId);
                if (chair == null) return false;
                chair.OwnerId = ownerId;
                chair.Status = (byte)reservStatus;
                return new ChairBO().Update(this.ConnectionHandler, chair);
            }
            catch (KnownException knownException)
            {
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public IEnumerable<Chair> GetListChairByIdList(List<Guid> list)
        {
            try
            {
                if (list.Any()) return new List<Chair>();
                var chairByIdList = new ChairBO().Where(this.ConnectionHandler, x => x.Id.In(list));
                return chairByIdList;


            }
            catch (KnownException knownException)
            {
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
