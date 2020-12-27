using System;
using Radyn.FCM;

namespace Radyn.Congress.Tools
{
    public static class FCMRepositiry
    {
        public static async void Work(string title, string body, string tokenId)
        {
            try
            {
            if (!string.IsNullOrEmpty(tokenId))
            {
                //                FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyDkY_MidQWMswkrX5Scms-YOg-5voKkTw4" };
                    FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig()
                    {
                        Key = "AIzaSyCYyVAtg717QBl5c5S786HHo5AYbsL8vqE"
                    };
                FirebaseCloudMessage message = new FirebaseCloudMessage.Builder()
                    .setTo(tokenId)
                    .setPriority(Priority.High)
                        .setNotification(new Notification() { title = title, body = body, sound = "default", icon = "logohoma.png", vibrate = new long[] { 500, 500, 500, 500, 500 } })
                    .setTime_to_live(0)
                    .build();

                FirebaseCloudMessageResult result = await message.SendAsync();
            }
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


