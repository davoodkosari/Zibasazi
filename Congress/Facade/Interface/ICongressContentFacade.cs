using System;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressContentFacade : IBaseFacade<CongressContent>
{
    bool Insert(Guid congressId, Content content, ContentContent contentContent);
   

}
}
