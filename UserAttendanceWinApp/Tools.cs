using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.XmlModel;

namespace Radyn.AttendanceWinApp
{
    public static class Extentions
    {
        public static List<KeyValuePair<string, string>> GetCongressList(this CongressXml  congressXml)
        {
            var getCongressList=new List<KeyValuePair<string,string>>();
            foreach (var congressModelXml in congressXml.CongressModelXml)
            {
                getCongressList.Add(new KeyValuePair<string, string>(congressModelXml.CongressId.ToString(),congressModelXml.Title));

            }

            return getCongressList;
        }
        public static List<KeyValuePair<string, string>> GetWorkShopList(this  CongressXml congressXml,Guid congressId)
        {
            var getCongressList = new List<KeyValuePair<string, string>>();
            var firstOrDefault = congressXml.CongressModelXml.FirstOrDefault(x=>x.CongressId==congressId);
            var workShopModelXmls = firstOrDefault.WorkShopModelList;
            foreach (var congressModelXml in workShopModelXmls)
            {
                getCongressList.Add(new KeyValuePair<string, string>(congressModelXml.Key, congressModelXml.Value));

            }

            return getCongressList;
        }
    }
}
