using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.FCM;

namespace Radyn.FCM
{
    public class Test
    {
        static async void Work()
        {
            FirebaseCloudMessaging.Config = new FirebaseCloudMessagingConfig() { Key = "AIzaSyDkY_MidQWMswkrX5Scms-YOg-5voKkTw4" };
            FirebaseCloudMessage message = new FirebaseCloudMessage.Builder()
                .setTo("fpOXIUprcp4:APA91bHhXLNiUDIe9lYMaz-RyiutedikJ3SLJ8kDadrcEZLYsxUhCEaUD23WKiN3A73bWWJ3ZLvEbVf9nQydlQniYhBXeL7Pfh1siiRYWtwtMa8C0KVFGZezKJSBtOKCN2jQaZpHjlYH")
                .setPriority(Priority.High)
                .setNotification(new Notification() { title = "ssss", body = "ss" })
                .build();

            FirebaseCloudMessageResult result = await message.SendAsync();
        }
    }
}
