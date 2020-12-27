using System;
using Radyn.Framework;
using Radyn.Slider.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface ISliderFacade : IBaseFacade<DataStructure.Slider>
{
    bool Insert(Guid websiteId, Slide slide, string url);
    bool Update(Guid websiteId, Slide slide,  string url);
}
}
