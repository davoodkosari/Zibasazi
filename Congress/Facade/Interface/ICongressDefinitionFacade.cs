using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressDefinitionFacade : IBaseFacade<CongressDefinition>
{
    CongressDefinition GetValidDefinition(Guid congressId);
    List<Enums.CongressDefinitionReportTypes> CongressDefinitionGetReportList(Guid homaId);
    bool ResetFactoryList(Guid homaId, List<string> list);
    bool ModifyReports(CongressDefinition congressDefinition, Dictionary<string, HttpPostedFileBase> dictionary);
}
}
