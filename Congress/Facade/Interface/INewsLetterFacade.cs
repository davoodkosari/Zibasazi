using System;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface INewsLetterFacade : IBaseFacade<NewsLetter>
    {

        
        bool RegsiterCongressUserModify(Guid congressId, Guid userId, bool addinnews = true);
        bool SentToUser(Guid congressId, int newsId);
    }
}
