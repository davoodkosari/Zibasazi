
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Radyn.PackageNotifier
{
    public class Notify
    {


        public string packageName;

        public string url;

        public string startDate;

        public string supportEndDate;

        public string comment;

        public string version;

        public byte product;

        public byte registerType;

        public string tokenId;














        public bool Send()
        {

            using (HttpClient client = new HttpClient())
            {
                //client.Timeout = TimeSpan.Parse("20");
                client.BaseAddress = new Uri("http://localhost:2001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                try
                {
                    var package = new Package()
                    {
                        PackageName = packageName,
                        Comment = comment,
                        Product = product,
                        RegisterType = registerType,
                        StartDate = startDate,
                        SupportEndDate = supportEndDate,
                        Url = this.url,
                        Version = version
                    };
                    return CreatePackage(client, package);
                    
                }
                catch (Exception e)
                {
                    return false;
                }


            }


        }
        static bool CreatePackage(HttpClient client, Package package)
        {
            var res = client.PostAsJsonAsync("api/products", package).Result;

            return res.IsSuccessStatusCode;

            // return URI of the created resource.

        }

        public class Builder
        {
            Notify notify = new Notify();

            //public Builder setTokenId(string tokenId)
            //{
            //    notify.tokenId = tokenId;
            //    return this;
            //}

            public Builder setPackageName(string packagename)
            {
                notify.packageName = packagename;
                return this;
            }
            public Builder setUrl(string url)
            {
                notify.url = url;
                return this;
            }

            public Builder setStartDate(string startDate)
            {
                notify.startDate = startDate;
                return this;
            }
            public Builder SetSupportEndDate(string supportEndDate)
            {
                notify.supportEndDate = supportEndDate;
                return this;
            }
            public Builder setComent(string comment)
            {
                notify.comment = comment;
                return this;
            }
            public Builder setVersion(string version)
            {
                notify.version = version;
                return this;
            }

            public Builder setProduct(Product product)
            {
                notify.product = (byte)product;
                return this;
            }

            public Builder setRegisterType(RegisterType registerType)
            {
                notify.registerType = (byte)registerType;
                return this;
            }












            public Notify build()
            {
                return notify;
            }
        }

    }
}
















