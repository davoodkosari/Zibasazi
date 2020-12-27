using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace Radyn.PackageNotifier
{
    public enum Product
    {
        [System.ComponentModel.Description("سیستم همایش ها")]
        Homa = 1,
        [System.ComponentModel.Description("سیستم قراردادها")]
        Contract = 2
    }

    public enum RegisterType
    {

        [System.ComponentModel.Description("اتوماتیک")]
        Automatic = 1,

        [System.ComponentModel.Description("دستی")]
        Manual = 2,
    }




    //public class Package
    //{


    //    public string PackageName { get; set; }

    //    public string Url { get; set; }

    //    public string StartDate { get; set; }

    //    public string SuportEndDate { get; set; }

    //    public string Comment { get; set; }

    //    public string Version { get; set; }

    //    public byte Product { get; set; }

    //    public byte RegisterType { get; set; }

    //    public string TokenId { get; set; }


    //}

    public sealed class Package
    {


        public string PackageName { get; set; }


        public string Url { get; set; }


        public string StartDate { get; set; }


        public string SupportEndDate { get; set; }


        public string Comment { get; set; }


        public string Version { get; set; }


        public byte? Product { get; set; }


        public byte? RegisterType { get; set; }

    }
}



