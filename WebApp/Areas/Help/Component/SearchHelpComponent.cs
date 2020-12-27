﻿using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using Radyn.Help;
using Radyn.Web.Mvc;

namespace Radyn.WebApp.Areas.Help.Component
{
    public static class SearchHelpComponent
    {
        public static void SearchRealtedHelp<TModel, TValue>(this HtmlHelper helper, TModel model, System.Linq.Expressions.Expression<Func<TModel, TValue>> value, string url = "/Help/Help/Search", int width = 800, int height = 600) where TModel : class
        {
            var s = value.Body.ToString();
            var lastIndexOf = s.LastIndexOf(".");
            var id = s.Substring(lastIndexOf + 1, s.Length - lastIndexOf - 1);

            var stringWriter = new StringWriter();
            var writer = new Html32TextWriter(stringWriter);

            helper.ViewContext.AddScript("/Scripts/Radyn/ModalWindows.js");

            writer.AddAttribute("Type", "text");
            writer.AddAttribute("readonly", "readonly");
            writer.AddAttribute("name", "txt" + id);
            writer.AddAttribute("id", "txt" + id);
            if (model != null)
            {
                var val = model.GetType().GetProperty(id).GetValue(model, null);
                if (val != null)
                {
                    Guid eid;
                    if (Guid.TryParse(val.ToString(), out eid))
                    {
                        var help = HelpComponent.Instance.HelpFacade.Get(eid);
                        if (help != null)
                            writer.AddAttribute("value", help.DefaultTitle);
                    }
                }
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute("Type", "hidden");
            writer.AddAttribute("name", id);
            writer.AddAttribute("id", id);
            if (model != null)
            {
                var val = model.GetType().GetProperty(id).GetValue(model, null);
                if (val != null)
                    writer.AddAttribute("value", val.ToString());
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute("class", "m-btn waves-blue i-search icon-btn");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.AddAttribute("onclick", string.Format("ShowModalWithReturnValue('{0}','" + id + "','txt" + id + "');", url));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();


            writer.AddAttribute("class", "m-btn waves-blue i-eraser icon-btn");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            writer.AddAttribute("onclick", string.Format("document.getElementById('txt" + id + "').value='';document.getElementById('" + id + "').value='';", url, width, height));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            var resourceScript = helper.ViewContext.GenerateResourceScript();

            helper.ViewContext.Writer.Write(resourceScript + stringWriter);
        }

        public class HelpComponentView : IView
        {
            private string str = "";
            public HelpComponentView(string content)
            {
                this.str = content;
            }

            public void Render(ViewContext viewContext, TextWriter writer)
            {
                writer.Write(str);
            }
        }
    }
}