using System;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressFormsFacade : IBaseFacade<CongressForms>
    {
        bool Insert(Guid congressId, FormStructure formStructure);


        bool UpdateAndAssgine(Guid congressId, FormStructure structure, string urls, bool forUser);
    }
}
