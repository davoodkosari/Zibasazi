using System;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.WebDesign.BO
{
    internal class GalleryBO : BusinessBase<DataStructure.Gallery>
    {
        public override bool Insert(IConnectionHandler connectionHandler, DataStructure.Gallery obj)
        {
            obj.WebSiteGallery.CreateDate = Utility.DateTimeUtil.ShamsiDate(DateTime.Now);
            return base.Insert(connectionHandler, obj);
        }
    }
}
