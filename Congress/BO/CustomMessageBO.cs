using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;

namespace Radyn.Congress.BO
{
    internal class CustomMessageBO : BusinessBase<CustomMessage>
    {
        protected override void CheckConstraint(IConnectionHandler connectionHandler, CustomMessage item)
        {
            
            if (base.Any(connectionHandler, x => x.Type == item.Type && x.CongressId == item.CongressId && x.Id != item.Id))
                throw new KnownException("این نوع متن پیام برای این همایش قبلا ثبت شده است");
        }
    }
}
