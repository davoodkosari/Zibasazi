using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IRefereeCartableFacade : IBaseFacade<RefereeCartable>
    {
        bool ModifyCartable(Guid articleId, Guid refreeId, byte state , bool visited , bool active);
        bool AnswerArticle(Guid congressId, RefereeCartable refereeCartable, Guid answeredrefreeId, string comments, HttpPostedFileBase attachment);
        bool SpecialRefereeAssignArticle(Guid congressId, RefereeCartable refereeCartable, Guid answeredrefreeId, string comments, HttpPostedFileBase attachment, List<Guid> refreeIdlist);
        bool DeleteFromRefreeCartable(List<Guid> list, Guid refreeId);
        bool AssigneArticleToRefreeCartabl(Guid congressId, Guid articleId, Guid flowsender, List<Guid> refereesId);
        bool Refereeopinion(RefereeCartable refereeCartable, FormStructure postFormData);
    
    }
}
