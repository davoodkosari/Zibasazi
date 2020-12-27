using System;
using System.Collections.Generic;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class CongressLanguageBO : BusinessBase<CongressLanguage>
    {
        public IEnumerable<Language> GetValidList(IConnectionHandler connectionHandler, Guid congressId)
        {

            var list = new List<Language>();
            var languageFacade = CommonComponent.Instance.LanguageFacade;
            var enumerable = this.Select(connectionHandler, x => x.LanguageId, x => x.CongressId == congressId);
            foreach (var variable in enumerable)
            {
                var language = languageFacade.Get(variable);
                if (language.Enabled) list.Add(language);
            }
            return list;


        }
    }
}
