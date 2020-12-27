using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation;
using Radyn.Reservation.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class CongressHallBO : BusinessBase<CongressHall>
    {
        public List<Hall> GetByCongressId(IConnectionHandler connectionHandler, Guid congressId, bool onlyEnalbe)
        {

          
            var predicateBuilder=new PredicateBuilder<CongressHall>();
            if(onlyEnalbe)
                predicateBuilder.And(c=>c.Hall.Enabled);
            predicateBuilder.And(x=>x.CongressId==congressId);
            return this.Select(connectionHandler, x => x.Hall, predicateBuilder.GetExpression());
          

        }
        public bool UpdateChairTypes(IConnectionHandler connectionHandler, IConnectionHandler reservationConnection, Hall hall, List<Guid> userregistertype)
        {
           
            var userRegisterPaymentTypeBo = new UserRegisterPaymentTypeBO();
            var chairTypeTransactionalFacade =
                ReservationComponent.Instance.ChairTypeTransactionalFacade(reservationConnection);
            var chairTypes = chairTypeTransactionalFacade.Where(x => x.HallId == hall.Id);
            var types = userRegisterPaymentTypeBo.Where(connectionHandler,x => x.Id.In(userregistertype));
            foreach (var guid in userregistertype)
            {
               
                var paymentType = types.FirstOrDefault(x=>x.Id==guid);
                if (paymentType == null) continue;
                var byRefId = chairTypeTransactionalFacade.FirstOrDefault(x=>x.HallId==hall.Id&&x.RefId==paymentType.Id.ToString());
                if (byRefId == null)
                {
                    var chairType =
                        new ChairType { RefId = paymentType.Id.ToString(),Title = paymentType.Title, HallId = hall.Id,CurrentUICultureName = hall.CurrentUICultureName};
                    if (!chairTypeTransactionalFacade.Insert(chairType))
                        throw new Exception("خطایی در ذخیره سالن وجود دارد");
                }
                else
                {
                    byRefId.Title = paymentType.Title;
                    byRefId.CurrentUICultureName = hall.CurrentUICultureName;
                    if (!chairTypeTransactionalFacade.Update(byRefId))
                        throw new Exception("خطایی در ذخیره سالن وجود دارد");
                }
            }
            foreach (var chairType in chairTypes)
            {
                if (userregistertype.Any(x => x == chairType.RefId.ToGuid())) continue;
                if (!chairTypeTransactionalFacade.Delete(chairType.Id))
                    throw new Exception("خطایی در ذخیره سالن وجود دارد");
            }
            return true;
        }
    }
}
