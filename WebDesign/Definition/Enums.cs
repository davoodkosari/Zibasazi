using Radyn.Framework;

namespace Radyn.WebDesign.Definition
{
    public class Enums
    {
        public enum ResourceType
        {
            [Description("JSFile", Type = typeof(Resources.WebDesign))]
            JSFile,
            [Description("CssFile", Type = typeof(Resources.WebDesign))]
            CssFile,
            [Description("JSFunction", Type = typeof(Resources.WebDesign))]
            JSFunction,
            [Description("Style", Type = typeof(Resources.WebDesign))]
            Style,
        }
        public enum WebSiteStatus
        {
            NotRegistered,
            NotConfiged,
            Disabled,
            NoProblem
        }

        
      



    }
}
