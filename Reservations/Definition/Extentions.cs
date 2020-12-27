using System.Collections.Generic;
using System.Linq;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Facade;

namespace Radyn.Reservation.Definition
{
    public static class Extentions
    {
        public static bool HaveChild(this Hall hall, bool onlyenalbe = true)
        {
          
            return onlyenalbe ? new HallFacade().Any(x => x.Enabled&&x.ParentId==hall.Id) : new HallFacade().Any(x=>x.ParentId == hall.Id);
        }
        public static IEnumerable<Hall> Childs(this Hall hall, bool onlyenalbe = true)
        {
           
            return onlyenalbe ? new HallFacade().Where(x => x.Enabled&& x.ParentId == hall.Id) :  new HallFacade().Where(x => x.ParentId == hall.Id);
        }

       
    }
}
