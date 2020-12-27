using System;
using System.Collections.Generic;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Web.Parser;

namespace Radyn.ContentManager.BO
{
    internal class MenuHtmlBO : BusinessBase<MenuHtml>
    {
        public override bool Insert(IConnectionHandler connectionHandler, MenuHtml obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

       

        
    }
}
