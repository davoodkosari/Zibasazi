using System.Linq;
using Radyn.Web.Mvc;
using Radyn.Web.Mvc.UI;
using Radyn.Web.Mvc.UI.Theme;

namespace Radyn.WebApp.AppCode.Themes
{
    public class HomaThemesRegister
    {
        public static void Register()
        {




            ThemeManager.AddTheme("HomaBase", true)

                .AddResource("/App_Themes/HomaBase/CSS/Menu/jquery-ui-1.10.1.custom.min.css", ResourceType.CssFile)
                .AddResource("/App_Themes/HomaBase/Scripts/hoverIntent.js", ResourceType.JSFile)
                .AddResource("/App_Themes/HomaBase/Scripts/superfish.js", ResourceType.JSFile)
                .AddResource("/App_Themes/HomaBase/CSS/Homa-Common.css", ResourceType.CssFile)
                .AddResource("/App_Themes/HomaBase/CSS/Menu/superfish.css", ResourceType.CssFile)
                .AddResource("/App_Themes/HomaBase/CSS/Menu/superfish-vertical.css", ResourceType.CssFile)
                .AddResource("/App_Themes/HomaBase/CSS/Homa-Common-Responsive.css", ResourceType.CssFile);


            ThemeManager.AddTheme("Homa-Advanced")
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/CongressPanel.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/Message.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Homa-Advanced/CSS/Homa-Main.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Homa-Advanced/Scripts/Homa-Main.js", ResourceType.JSFile)
            ;

            ThemeManager.AddTheme("Avas")
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/CongressPanel.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/Message.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Avas/CSS/navbar.css", ResourceType.CssFile)

                .AddResource("/App_Themes/Avas/Scripts/navbar.js", ResourceType.JSFile)
                .AddResource("/App_Themes/Avas/CSS/materialize.min.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Avas/Scripts/materialize.min.js", ResourceType.JSFile)
                .AddResource("/App_Themes/Avas/CSS/style.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Avas/CSS/Color/White.css", ResourceType.CssFile)
                ;

            ThemeManager.AddTheme("Exhibz")
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/CongressPanel.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Homa-Silver-RedMenu/CSS/Message.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Exhibz/CSS/navbar.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Exhibz/Scripts/navbar.js", ResourceType.JSFile)
                .AddResource("/App_Themes/Exhibz/CSS/materialize.min.css", ResourceType.CssFile)
                .AddResource("/App_Themes/Exhibz/Scripts/materialize.min.js", ResourceType.JSFile)
                .AddResource("/App_Themes/Exhibz/CSS/style.css", ResourceType.CssFile)
                ;

            ThemeManager.AddTheme("Homa-Flat")
                .AddResource("/App_Themes/Homa-Flat/CSS/Homa-Main-Flat.css", ResourceType.CssFile)
                ;
            ThemeManager.AddTheme("Default")
            .AddResource("/App_Themes/Default/CSS/Default.css", ResourceType.CssFile)
            .AddResource("/App_Themes/Default/CSS/CongressPanel.css", ResourceType.CssFile)
            .AddResource("/App_Themes/Default/CSS/Message.css", ResourceType.CssFile)
            .AddResource("/App_Themes/Default/CSS/Green/Homa-Green.css", ResourceType.CssFile)

                ;

        }
    }
}