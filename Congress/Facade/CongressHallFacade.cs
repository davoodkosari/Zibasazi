using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressHallFacade : CongressBaseFacade<CongressHall>, ICongressHallFacade
    {
        internal CongressHallFacade()
        {
        }

        internal CongressHallFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressHallBo = new CongressHallBO();
                var obj = congressHallBo.Get(this.ConnectionHandler, keys);
                if (!congressHallBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف سالن وجود دارد");
                if (
                    !ReservationComponent.Instance.HallTransactionalFacade(this.ContentManagerConnection)
                        .Delete(obj.HallId))
                    throw new Exception("خطایی در حذف سالن وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, Hall hall, HttpPostedFileBase fileBase, List<Guid> userregistertype)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                hall.IsExternal = true;
                if (
                    !ReservationComponent.Instance.HallTransactionalFacade(this.ReservationConnection)
                        .Insert(hall, fileBase))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
                var congressHall = new CongressHall() { HallId = hall.Id, CongressId = congressId };
                if (!new CongressHallBO().Insert(this.ConnectionHandler, congressHall))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
                if (
                    !new CongressHallBO().UpdateChairTypes(this.ConnectionHandler, this.ReservationConnection, hall,
                        userregistertype))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Guid congressId, Hall hall,  HttpPostedFileBase fileBase, List<Guid> userregistertype)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !ReservationComponent.Instance.HallTransactionalFacade(this.ReservationConnection)
                        .Update(hall, fileBase))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
                if (
                    !new CongressHallBO().UpdateChairTypes(this.ConnectionHandler, this.ReservationConnection, hall,
                        userregistertype))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public IEnumerable<Hall> GetByCongressId(Guid congressId, bool onlyEnalbe = false)
        {
            try
            {

                return new CongressHallBO().GetByCongressId(this.ConnectionHandler, congressId, onlyEnalbe);

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


        public IEnumerable<Hall> GetParents(Guid congressId, bool onlyEnalbe)
        {
            try
            {

                return
                    new CongressHallBO().GetByCongressId(this.ConnectionHandler, congressId, onlyEnalbe)
                        .Where(x => x.ParentId == null);
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

        public Dictionary<UserRegisterPaymentType, bool> GetChairTypes(Guid congressId, Guid? hallId)
        {
            try
            {
                var list = new Dictionary<UserRegisterPaymentType, bool>();
                var byFilter = new UserRegisterPaymentTypeBO().Where(this.ConnectionHandler,
                    x => x.CongressId == congressId);
                if (!byFilter.Any()) return list;
                var chairTypeFacade = ReservationComponent.Instance.ChairTypeFacade;
                var @select = hallId.HasValue? chairTypeFacade.Select(x => x.RefId,
                    x => x.HallId==hallId.Value):new List<string>();
                foreach (var userRegisterPaymentType in byFilter)
                {
                    if (string.IsNullOrEmpty(userRegisterPaymentType.Title)) continue;
                    var added = @select.Any(x=>x.Equals(userRegisterPaymentType.Id.ToString()));
                   list.Add(userRegisterPaymentType, added);
                }
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


        public IEnumerable<User> GetReservedList(Guid congressId, Guid hallId)
        {
            try
            {
              
                return new UserBO().OrderBy(this.ConnectionHandler,
                    new[]
                    {
                        new OrderByModel<User>() {Expression = x => x.Chair.Row},
                        new OrderByModel<User>() {Expression = x => x.Chair.Column},
                    },
                    x => x.CongressId == congressId && x.ChairId.HasValue);
              
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

        public IEnumerable<Hall> GetByParents(Guid congressId, Guid hallId, bool onlyEnalbe)
        {
            try
            {

                return
                    new CongressHallBO().GetByCongressId(this.ConnectionHandler, congressId, onlyEnalbe)
                        .Where(x => x.ParentId == hallId);
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
