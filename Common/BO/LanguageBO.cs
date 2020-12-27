using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Common.BO
{
    internal class LanguageBO : BusinessBase<Language>
    {
       

        protected override void CheckConstraint(IConnectionHandler connectionHandler, Language item)
        {
            base.CheckConstraint(connectionHandler, item);
            if (!item.IsDefault) return;
            var @where = this.Where(connectionHandler, x => x.IsDefault && x.Id != item.Id);
            foreach (var language in @where)
            {
                language.IsDefault = false;
                base.Update(connectionHandler, language);
            }
        }
    }
}
