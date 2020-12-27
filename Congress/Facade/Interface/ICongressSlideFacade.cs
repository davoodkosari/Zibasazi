using System;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Slider.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressSlideFacade : IBaseFacade<CongressSlide>
{
    bool Insert(Guid congressId,Slide slide,string usefor);
    bool Update(Guid congressId, Slide slide,string usefor);
}
}
