using Radyn.Framework;

namespace Radyn.Reservation.Definition
{
    public class Enums
    {
        public enum ReservStatus : byte
        {
           
            None = 0,
            [Description("Free", Type = typeof(Resources.Reservation))]
            Free = 1,
            [Description("NotForSale", Type = typeof(Resources.Reservation))]
            NotForSale = 2,
            [Description("Reserved", Type = typeof(Resources.Reservation))]
            Reserved = 3,
            [Description("Saled", Type = typeof(Resources.Reservation))]
            Saled = 4



        }
       
    }
}
