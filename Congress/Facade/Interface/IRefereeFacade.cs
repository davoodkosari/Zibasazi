using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IRefereeFacade : IBaseFacade<Referee>
    {
        bool Insert(Referee referee,  HttpPostedFileBase fileBase, List<Guid> pivots);
        bool Update(Referee referee,  HttpPostedFileBase fileBase, List<Guid> pivots);
        Referee Login(string username, string password, Guid CongressId);
        Task<Referee> LoginAsync(string username, string password, Guid CongressId);
        bool ChangePassword(Guid userId, string password);
        bool CheckOldPassword(Guid userId, string password);
      
        Dictionary<Referee, bool> GetAllForArticle(Guid congressId, Guid articleId, bool isSpecial = false);
        List<Referee> SearchRefree(string text, Guid congressId);
        List<Referee> GetAllrefreeWithCartable(Guid homaId);
        Dictionary<Referee, List<string>> ImportFromExcel(HttpPostedFileBase fileBase, Guid congressId);

        bool InsertList(List<Referee> referee);
    }
}
