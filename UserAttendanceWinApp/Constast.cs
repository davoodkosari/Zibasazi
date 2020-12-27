using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.AttendanceWinApp
{
    public class Constast
    {
        public const string UploadxmlAddress = "XmlFile\\UploadData.xml";
        public const string DownloadXmlAddress = "XmlFile\\DownloadData.xml";

        public const string UploadUrl = "/Congress/API/SetAttendanceXmlFile";
        public const string DownloadUrl = "/Congress/API/GetAttendanceXmlFile";

    }
}
