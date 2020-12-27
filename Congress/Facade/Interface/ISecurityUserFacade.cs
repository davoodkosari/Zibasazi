using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using User = Radyn.Security.DataStructure.User;

namespace Radyn.Congress.Facade.Interface
{
    public interface ISecurityUserFacade : IBaseFacade<SecurityUser>
    {

        Dictionary<Homa, bool> GetUserCongressList(Guid? userId);
        bool DeleteByUserId(Guid userId);
        User Login(string username, string password, Guid congressId);
        Task<User> LoginAsync(string username, string password, Guid congressId);
        bool Update(User user1,  HttpPostedFileBase file, List<Guid> list);
        bool Insert(User user1,  HttpPostedFileBase file, List<Guid> list);

      
    }
}
