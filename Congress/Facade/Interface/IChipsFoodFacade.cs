using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IChipsFoodFacade : IBaseFacade<ChipsFood>
{
    bool Insert(ChipsFood chipsFood, List<int> days);
    bool Update(ChipsFood chipsFood,  List<int> days);
    Dictionary<int,bool> GetDaysInfo(Guid congressId,Guid? chipfoodId);
    bool JoinUser(Guid chipsFoodid, List<Guid> list);

    IEnumerable<ModelView.UserCardModel> SearchChipFoodReport(Guid congressId, Guid chipfoodId, string txtSearch,
        User user,
        EnterpriseNode.Tools.Enums.Gender gender,
        FormStructure postFormData);

    Dictionary<User, bool> SearchChipFood(Guid congressId, Guid chipFoodId, string txtSearch, User user,
        EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure);
    
    ModelView.UserCardModel SearchChipFoodReport(Guid chipfoodId, Guid userId);
}
}
