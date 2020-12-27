using System.Collections.Generic;
using System.Threading.Tasks;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Slider.DataStructure;

namespace Radyn.Slider.Facade.Interface
{
public interface ISlideFacade : IBaseFacade<Slide>
{
   
    Slide GetSlideWithSliders(short slideId);
    Task<Slide> GetSlideWithSlidersAsync(short slideId);
}
}
