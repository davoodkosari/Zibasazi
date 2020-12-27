using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class HomaAliasBO : BusinessBase<HomaAlias>
    {
        protected override void CheckConstraint(IConnectionHandler connectionHandler, HomaAlias item)
        {
            base.CheckConstraint(connectionHandler, item);
            if (!string.IsNullOrEmpty(item.Url))
            {
                if (item.Homa!=null&&item.Homa.InstallPath == item.Url)
                    throw new Exception("این مسیر برابر مسیر اصلی همایش است");
                item.Url = StringUtils.Encrypt(item.Url.ToLower());
            }
        }
        public override HomaAlias Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var obj = base.Get(connectionHandler, keys);
            if (obj != null)
                obj.Url = StringUtils.Decrypt(obj.Url).ToLower();
            return obj;
        }

        public List<HomaAlias> GetByCongressId(IConnectionHandler connectionHandler, Guid congressId)
        {
            var list = this.Where(connectionHandler, x => x.CongressId == congressId);
            foreach (var homaAliase in list)
            {
                homaAliase.Url = StringUtils.Decrypt(homaAliase.Url).ToLower();
            }
            return list;
        }
    }
}
