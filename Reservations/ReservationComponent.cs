using Radyn.Framework.DbHelper;
using Radyn.Reservation.Facade;
using Radyn.Reservation.Facade.Interface;

namespace Radyn.Reservation
{
    public class ReservationComponent
    {

        private ReservationComponent()
        {

        }


        private static ReservationComponent _instance;
        public static ReservationComponent Instance
        {
            get { return _instance ?? (_instance = new ReservationComponent()); }
        }
        public IHallFacade HallTransactionalFacade(IConnectionHandler connectionHandler)
        {
            return new HallFacade(connectionHandler);
        }
        public IHallFacade HallFacade
        {
            get { return new HallFacade(); }
        }
        public IChairTypeFacade ChairTypeFacade
        {
            get { return new ChairTypeFacade(); }
        }
        public IChairTypeFacade ChairTypeTransactionalFacade(IConnectionHandler connectionHandler)
        {
            return new ChairTypeFacade(connectionHandler);
        }
        public IChairFacade ChairTransactionalFacade(IConnectionHandler connectionHandler)
        {
            return new ChairFacade(connectionHandler);
        }
        public IChairFacade ChairFacade
        {
            get { return new ChairFacade(); }
        }
    }
}
