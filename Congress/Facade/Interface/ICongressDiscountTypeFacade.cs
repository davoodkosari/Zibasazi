using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressDiscountTypeFacade : IBaseFacade<CongressDiscountType>
{
    IEnumerable<DiscountType> GetByCongressId(Guid congressId);
    string FillTempAdditionalData(Guid congressId);
    bool Insert(Guid congressId, DiscountType discountType, List<DiscountTypeSection> sectiontypes,List<DiscountTypeAutoCode> discountTypeAutoCodes);
    Guid UpdateStatusAfterTransactionGroupTemp(Guid userId,Guid id);
}
}
