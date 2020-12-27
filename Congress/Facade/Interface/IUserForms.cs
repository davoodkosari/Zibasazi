using System;
using System.Data;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IUserForms : IBaseFacade<UserForms>
    {
        DataTable ReportUserForms(Guid formId, Guid homaId, string culture);
    }
}
