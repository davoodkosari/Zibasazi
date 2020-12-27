using System.Collections.Generic;
using System.Web;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Slider.DataStructure;

namespace Radyn.Slider.Facade.Interface
{
    public interface ISlideItemFacade : IBaseFacade<SlideItem>
    {
        bool Update(SlideItem slideItem,  HttpPostedFileBase image);
        bool Insert(SlideItem slideItem, HttpPostedFileBase image);
    }
}
