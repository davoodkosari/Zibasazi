using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.WebPages;
using Radyn.Congress;
using Radyn.PackageNotifier;
using Radyn.WebApp.AppCode.Security;

namespace Radyn.WebApp.Tools
{
    public static class PackageRepository
    {


        static void test()
        {

        }
        public static  bool Work()
        {
            try
            {
                var authority = HttpContext.Current.Request.Url.Authority;
                var congress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetUrlHomaId(authority, null);


                var notify = new Notify.Builder()
                  .setPackageName(congress.CongressTitle)
                  .setUrl(authority)
                  .SetSupportEndDate("1396/01/01")
                  .setVersion("2.0.7")
                  .setProduct(Product.Homa)
                  .setRegisterType(RegisterType.Automatic)
                   .build();
               return  notify.Send();



                //if (res)
                //{
                //    congress.IsNotified = true;
                //    if (!CongressComponent.Instance.BaseInfoComponents.HomaFacade.Update(congress))
                //        throw new Exception();
                //    SessionParameters.CurrentCongress.IsNotified = true;
                //}

                //PackageNotifier.Notify notify = new PackageNotifier.Notify.Builder()
                //   // StringUtils.Decrypt(homa.InstallPath)

                //   .setPackageName(congress.CongressTitle)
                //   .setUrl(authority)
                //   .SetSupportEndDate("1396/01/01")
                //   .setVersion("2.0.7")
                //   .setProduct(Product.Homa)
                //   .setRegisterType(RegisterType.Automatic)
                //    .build();
                //notify.SendAsync().Wait();

                //}
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }

    //static async void Work(Ticket ticket, string tokenId)
    //{
    //    if (!string.IsNullOrEmpty(tokenId))
    //    {
    //        //                FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyDkY_MidQWMswkrX5Scms-YOg-5voKkTw4" };
    //        FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyCb17ZQ6DsS3LZSmVkoNaaBFip0fLRu0Rs" };
    //        FirebaseCloudMessage message = new FirebaseCloudMessage.Builder()
    //            .setTo(tokenId)
    //            .setPriority(Priority.High)
    //            .setNotification(new Notification() { title = ticket.Subject + "(" + ticket.ReciveDate + " " + ticket.ReciveTime + ")", body = ticket.Body })
    //            .build();

    //        FirebaseCloudMessageResult result = await message.SendAsync();
    //    }
    //}
    //static async void WorkC(string tokenId,int count)
    //{
    //    if (!string.IsNullOrEmpty(tokenId))
    //    {
    //        //                FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyDkY_MidQWMswkrX5Scms-YOg-5voKkTw4" };
    //        FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyCb17ZQ6DsS3LZSmVkoNaaBFip0fLRu0Rs" };
    //        FirebaseCloudMessage message = new FirebaseCloudMessage.Builder()
    //            .setTo(tokenId)
    //            .setPriority(Priority.High)
    //            .setNotification(new Notification() { title = "تسک جدید در کارتابل", body =string.Format("شما تعداد {0} کار جدید در کارتابل تان دارید", count) })
    //            .build();

    //        FirebaseCloudMessageResult result = await message.SendAsync();
    //    }
    //}
}


