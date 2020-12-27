using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.ContentManager.Facade.Interface
{
    public interface IPartialLoadFacade : IBaseFacade<PartialLoad>
    {
        List<PartialLoad> GetHtmlPartials(Guid htmlId, string culture, Container DefaultContrainer = null);
        Task<List<PartialLoad>> GetHtmlPartialsAsync(Guid htmlId, string culture, Container DefaultContrainer = null);
        bool Insert(PartialLoad partialLoad,  string partialPostion);
        bool Insert(string partialId, string customId, Guid htmlId, int? position);
        bool CustomeSwap(string partialId, string customId, Guid htmlId, int getorder);
        bool DeletePartial(string partialId, string customId, Guid htmlId);
        bool SwapPartials(string partialId, string customId, Guid htmlId, string type);
    }
}
