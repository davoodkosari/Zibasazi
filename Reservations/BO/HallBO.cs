using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.FileManager;
using Radyn.FileManager.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.DA;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;

namespace Radyn.Reservation.BO
{
    public class HallBO : BusinessBase<Hall>
    {

        public override bool Insert(IConnectionHandler connectionHandler, Hall obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (!base.Insert(connectionHandler, obj))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            if (!this.AddNewChairs(connectionHandler, obj))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            return true;
        }
        public bool Insert(IConnectionHandler connectionHandler,  IConnectionHandler filemanagerConnection, Hall obj,  HttpPostedFileBase fileBase)
        {
            if (fileBase != null)
            {
                var id = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerConnection).Insert(fileBase);
                obj.PhotoId = id;
            }
            if (!this.Insert(connectionHandler, obj))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            
            return true;
        }
        public bool Update(IConnectionHandler connectionHandler,  IConnectionHandler filemanagerConnection, Hall obj, HttpPostedFileBase fileBase)
        {
            if (fileBase != null)
            {
                var fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerConnection);
                if (obj.PhotoId.HasValue)
                    fileTransactionalFacade.Update(fileBase, obj.PhotoId.Value);
                else
                    obj.PhotoId = fileTransactionalFacade.Insert(fileBase); 
                
            }
            if (!this.Update(connectionHandler, obj))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            
            return true;
        }

        public override bool Update(IConnectionHandler connectionHandler, Hall obj)
        {
            var bo = new ChairBO();
            var oldobj = this.Get(connectionHandler, obj.Id);
            if (obj.Width != oldobj.Width || obj.Length != oldobj.Length)
            {
                var allowDelete = bo.AllowDelete(connectionHandler, obj.Id);
                if (!allowDelete)
                    throw new Exception(Resources.Reservation.ErrrorInEditHallBecauseItHasReservedChair);
                if (!this.DeleteChairs(connectionHandler, obj.Id))
                    throw new Exception(Resources.Reservation.ErrorInSaveHall);

                if (!this.AddNewChairs(connectionHandler, obj))
                    throw new Exception(Resources.Reservation.ErrorInSaveHall);
            }
            if (!base.Update(connectionHandler, obj))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            return true;
        }

        public override bool Delete(IConnectionHandler connectionHandler, params object[] keys)
        {
            var obj = this.Get(connectionHandler, keys);
            var allowDelete = new ChairBO().AllowDelete(connectionHandler, obj.Id);
            if (!allowDelete)
                throw new Exception(Resources.Reservation.ErrorInDeleteHallBecauseItHasRservedChair);
            var filter = this.Any(connectionHandler, x => x.ParentId == obj.Id);
            if (filter)
                throw new Exception(Resources.Reservation.ErrorInDeleteHallBecauseItHasChildHall);
            if (!this.DeleteChairs(connectionHandler, obj.Id))
                throw new Exception(Resources.Reservation.ErrorInDeleteHall);
            if (!this.DeleteChairtypes(connectionHandler, obj.Id))
                throw new Exception(Resources.Reservation.ErrorInDeleteHall);
            return base.Delete(connectionHandler, keys);
        }

        private bool DeleteChairtypes(IConnectionHandler connectionHandler, Guid hallId)
        {
            var da = new HallDA(connectionHandler);
            return da.DeleteChairtypes(hallId) >= 0;
        }

        public bool DeleteChairs(IConnectionHandler connectionHandler, Guid hallId)
        {
            var da = new HallDA(connectionHandler);
            return da.DeleteChairs(hallId) >= 0;
        }
        public bool AddNewChairs(IConnectionHandler connectionHandler, Hall obj)
        {
            var bo = new ChairBO();
            var count = obj.Length * obj.Width;
            if (count <= 0) return true;
            short column = 1;
            var list = new List<Chair>();
            for (int r = 1; r <= count/obj.Length; r++)
            {
                for (int c = 1; c <= obj.Length; c++)
                {
                    if (column <= count)
                    {
                        var chair = new Chair()
                        {
                            Id = Guid.NewGuid(),
                            HallId = obj.Id,
                            Column = c,
                            Row = r,
                            ChairTypeId = null,
                            Number = 0,
                            Status = (byte) Enums.ReservStatus.None
                        };
                        list.Add(chair);

                    }
                    column++;
                }
            }
            if (!bo.InsertChairs(connectionHandler, list))
                throw new Exception(Resources.Reservation.ErrorInSaveHall);
            return true;
        }

        public IEnumerable<Hall> GetParents(IConnectionHandler connectionHandler)
        {
            var da = new HallDA(connectionHandler);
            return da.GetParents();
        }
    }
}
