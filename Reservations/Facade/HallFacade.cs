using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.BO;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;
using Radyn.Reservation.Facade.Interface;

namespace Radyn.Reservation.Facade
{
    internal sealed class HallFacade : ReservationBaseFacade<Hall>, IHallFacade
    {
        internal HallFacade() { }

        internal HallFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      

       
        //('Row','Column','Status','Number','Amount')
        public Hall GetHallWithChairs(Guid hallId)
        {
            try
            {
                var hall = new HallBO().Get(this.ConnectionHandler, hallId);
                hall.Chairs = new ChairBO().GetHallChairs(this.ConnectionHandler, hallId);
                var chairTypeBo = new ChairTypeBO();
                var chairs = hall.Chairs.Where(x => x.Status != (byte)Enums.ReservStatus.None);
                foreach (var chair in chairs)
                {

                    if (chair.ChairTypeId.HasValue)
                        chair.ChairType = chairTypeBo.Get(base.ConnectionHandler, chair.ChairTypeId);
                    if (!chair.OwnerId.HasValue) continue;
                    if (chair.Owner != null)
                        chair.ColumValue += "," + chair.Owner.DescriptionField+"-"+chair.Owner.Email;
                }
                return hall;
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



        public bool Insert(Hall obj, HttpPostedFileBase fileBase)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new HallBO().Insert(this.ConnectionHandler, this.FileManagerConnection, obj, fileBase))
                    throw new Exception(Resources.Reservation.ErrorInSaveHall);
                this.ConnectionHandler.CommitTransaction();
               this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public bool Update(Hall hall,  HttpPostedFileBase fileBase)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new HallBO().Update(this.ConnectionHandler,  this.FileManagerConnection, hall, fileBase))
                    throw new Exception(Resources.Reservation.ErrorInSaveHall);
                this.ConnectionHandler.CommitTransaction();
               this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
               this.FileManagerConnection.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
               this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool HallChairUpdate(List<string> chairs)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var chairBo = new ChairBO();
                if (!chairBo.UpdateChairs(this.ConnectionHandler, chairs))
                    throw new Exception(Resources.Reservation.ErrorInSaveHall);
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError, knownException.Source, knownException.StackTrace);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }




        public IEnumerable<Hall> GetParents()
        {
            try
            {
                var getParents = new HallBO().GetParents(this.ConnectionHandler);
              return getParents;
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
