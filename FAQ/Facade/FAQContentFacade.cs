using Radyn.Common;
using Radyn.FAQ.BO;
using Radyn.FAQ.DataStructure;
using Radyn.FAQ.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.FAQ.Facade
{
    internal sealed class FAQContentFacade : FAQBaseFacade<FAQContent>, IFAQContentFacade
    {
        internal FAQContentFacade() { }

        internal FAQContentFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      
    }
}
