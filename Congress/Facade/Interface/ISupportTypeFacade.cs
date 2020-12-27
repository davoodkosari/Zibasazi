using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ISupportTypeFacade : IBaseFacade<SupportType>
    {
        bool Update(Guid congressId,SupportType supportType);
        bool Insert(Guid congressId,SupportType supportType);
        List<KeyValuePair<SupportType,bool>> GetSupportTypeModel(Guid id);
        SupportType GetSupportWithSupporters(short id);
        Task<SupportType> GetSupportWithSupportersAsync(short id);
    }
}
