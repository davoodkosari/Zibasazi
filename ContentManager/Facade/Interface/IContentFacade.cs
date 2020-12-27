using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.ContentManager.Facade.Interface
{
    public interface IContentFacade : IBaseFacade<Content>
    {
       
        IEnumerable<Content> Search(string qry);
        bool Update(Content content, ContentContent contentContent);
        bool Insert(Content content, ContentContent contentContent);
        string GetHtml(int Id, string culture="fa-IR");
        Task<string> GetHtmlAsync(int Id, string culture="fa-IR");
   

    }
}
