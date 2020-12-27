using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.BO;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Facade.Interface;

namespace Radyn.Reservation.Facade
{
    internal sealed class ChairTypeFacade : ReservationBaseFacade<ChairType>, IChairTypeFacade
    {
        internal ChairTypeFacade() { }

        internal ChairTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

       
       

        
    }
}
