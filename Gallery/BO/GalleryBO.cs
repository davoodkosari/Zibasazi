using System;
using System.Collections.Generic;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Gallery.DA;
using Radyn.Utility;

namespace Radyn.Gallery.BO
{
    internal class GalleryBO : BusinessBase<DataStructure.Gallery>
    {
        public override bool Insert(IConnectionHandler connectionHandler, DataStructure.Gallery obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            obj.CreateDate = DateTime.Now.ShamsiDate();
            return base.Insert(connectionHandler, obj);
        }

        public bool HasPhoto(IConnectionHandler connectionHandler, Guid id)
        {
            var da = new GalleryDA(connectionHandler);
            return da.HasPhoto(id)>0;
        }

        public bool HasChild(IConnectionHandler connectionHandler, Guid id)
        {
            var da = new GalleryDA(connectionHandler);
            return da.HasChild(id) > 0;
        }

       
    }
}
