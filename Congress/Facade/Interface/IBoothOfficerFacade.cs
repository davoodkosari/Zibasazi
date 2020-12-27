using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IBoothOfficerFacade : IBaseFacade<BoothOfficer>
{
    IEnumerable<ModelView.UserCardModel> GetCardList(Guid boothId, Guid userId);
    ModelView.UserCardModel GetBoothOfficerCard(Guid Id,Guid boothId, Guid userId);
    string GetNamesByBoothId(UserBooth userBooth);
   
}
}
