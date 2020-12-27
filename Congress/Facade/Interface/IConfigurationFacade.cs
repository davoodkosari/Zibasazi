using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
    public interface IConfigurationFacade : IBaseFacade<Configuration>
    {
        bool Insert(Configuration configuration,ConfigurationContent configurationContent,  HttpPostedFileBase refreeAttachment,
            HttpPostedFileBase boothMapAttachmentId, HttpPostedFileBase orginalPoster, HttpPostedFileBase miniPoster,
            HttpPostedFileBase logo, HttpPostedFileBase header, HttpPostedFileBase footer, HttpPostedFileBase hallMapId, HttpPostedFileBase backgroundImageId, HttpPostedFileBase favIcon, List<DiscountTypeSection> sectiontypes);

        bool Update(Configuration configuration, ConfigurationContent configurationContent,   HttpPostedFileBase refreeAttachment,
            HttpPostedFileBase boothMapAttachmentId, HttpPostedFileBase orginalPoster, HttpPostedFileBase miniPoster,
            HttpPostedFileBase logo, HttpPostedFileBase header, HttpPostedFileBase footer, HttpPostedFileBase hallMapId, HttpPostedFileBase backgroundImageId, HttpPostedFileBase favIcon, string modelName, List<DiscountTypeSection> sectiontypes);


        

    }
}
