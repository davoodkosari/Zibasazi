using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.XmlModel;

namespace Radyn.Congress.Facade.Interface
{
    public interface IHomaFacade : IBaseFacade<Homa>
    {
        bool Insert(Homa homa,  HttpPostedFileBase file,List<HomaAlias> homaAliases);
        bool Update(Homa homa,  HttpPostedFileBase file, List<HomaAlias> homaAliases);
        bool ConfigByDefaulToHoma(Guid homaId);
        List<ModelView.SerachResultvalue> SearchInHoma(Guid congressId, Enums.SearchType SerachType, string txtvalue);
        Homa GetUrlHomaId(string authority, string url);
        Task<Homa> GetUrlHomaIdAsync(string authority, string url);
       void SendInform(Homa homa, byte type, Message.Tools.ModelView.MessageModel inform);
        bool HasLicense();
        Task<bool> HasLicenseAsync();
        List<Homa> GetLastCongress();
        List<Homa> GetCurrentCongress();
        List<Homa> GetNextCongress();
         void DailyEvaulation();
         CongressXml GetXmlData();
         bool SetXmlData(string data);
    }
}
