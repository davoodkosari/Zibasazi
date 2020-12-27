using System;
using System.Collections.Generic;
using Radyn.Common.DataStructure;
using Radyn.Framework;

namespace Radyn.Common.Facade.Interface
{
    public interface ILanguageContentFacade : IBaseFacade<LanguageContent>
    {     
       
      
      
        List<LanguageContent> GetByMoudelname(string moudelname, string culture);
       
       
        LanguageContent GetReource(string key, string culture);
        
       
    }
}
