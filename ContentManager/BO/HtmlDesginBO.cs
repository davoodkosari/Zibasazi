using System;
using System.Collections.Generic;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Web.Parser;

namespace Radyn.ContentManager.BO
{
    internal class HtmlDesginBO : BusinessBase<HtmlDesgin>
    {
        public override bool Insert(IConnectionHandler connectionHandler, HtmlDesgin obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public Dictionary<string, string> ReturnCustomeAttributes(IConnectionHandler connectionHandler, Guid htmdesignId)
        {
            var parser = new HtmlParser { ParseMethod = ParseMethod.All };
            var htmlDesgin = this.Get(connectionHandler, htmdesignId);
            if (htmlDesgin == null) return null;
            var htmlString = Utility.Utils.ConvertHtmlToString(htmlDesgin.Body);
            parser.Parse(htmlString);
            var list = new Dictionary<string, string>();
            foreach (var tag1 in parser.Tags)
            {
                if (tag1 == null) continue;
                var tag2 = (HtmlTag)tag1;
                if (tag2.Attributes.AttributeByName("customId") != null)
                {
                    if (!string.IsNullOrEmpty(tag2.Attributes.AttributeValue("customId")))
                    {
                        var str = tag2.Attributes.AttributeValue("customId");
                        if (tag2.Attributes.AttributeByName("dec") != null)
                        {
                            if (!string.IsNullOrEmpty(tag2.Attributes.AttributeValue("dec")))
                                str = tag2.Attributes.AttributeValue("dec");
                        }
                        list.Add(tag2.Attributes.AttributeValue("customId"), str);
                    }
                }

            }
            return list;
        }

        
    }
}
