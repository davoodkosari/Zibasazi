using System.Threading.Tasks;
using Radyn.News.DataStructure;
using Radyn.News.Facade;

namespace Radyn.News.Tools
{
    public static class Extentions
    {
        public static NewsContent GetNewsContent(this News.DataStructure.News news, string culture)
        {
            var value = new NewsContentFacade().Get(news.Id, culture);
            if (value != null) return value;
            var newcontent = new NewsContent() { Body = "", Title1 = "", Title2 = "", Lead = "", Sutitr = "", OverTitle = "" };
            return newcontent;
        }  public static async Task<NewsContent> GetNewsContentAsync(this News.DataStructure.News news, string culture)
        {
            var value =await new NewsContentFacade().GetAsync(news.Id, culture);
            if (value != null) return value;
            var newcontent = new NewsContent() { Body = "", Title1 = "", Title2 = "", Lead = "", Sutitr = "", OverTitle = "" };
            return newcontent;
        }
    }
}
