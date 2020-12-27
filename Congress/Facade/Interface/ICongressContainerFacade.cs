using System;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressContainerFacade : IBaseFacade<CongressContainer>
    {
        bool Insert(Guid congressId, Container container);
        
    }
}
