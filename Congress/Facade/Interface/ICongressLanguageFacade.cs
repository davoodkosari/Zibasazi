using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Common.DataStructure;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressLanguageFacade : IBaseFacade<CongressLanguage>
    {
        bool Insert(Guid congressId, Language language,HttpPostedFileBase lanuagelogo);
        List<Language> GetValidList(Guid congressId);
       
    }
}
