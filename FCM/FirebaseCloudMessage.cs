using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.FCM
{
    public class FirebaseCloudMessage
    {

        public string to;

        public Notification notification;

        public string priority;

        public int time_to_live;

        public Dictionary<string, string> data;



        public Task<string> getAsStringAsync()
        {
            return Task.Factory.StartNew(() => JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        public async Task<FirebaseCloudMessageResult> SendAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage rMessage = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
                rMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + FirebaseCloudMessaging.Config.Key);
                rMessage.Content = new StringContent(await getAsStringAsync(), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(rMessage);
                string responseString = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<FirebaseCloudMessageResult>(responseString));
            }
        }

        public class Builder
        {
            FirebaseCloudMessage message = new FirebaseCloudMessage();

            public Builder setTo(string to)
            {
                message.to = to;
                return this;
            }

            public Builder setNotification(Notification notification)
            {
                message.notification = notification;
                return this;
            }

            public Builder setPriority(string priority)
            {
                message.priority = priority;
                return this;
            }

            public Builder setTime_to_live(int time_to_live)
            {
                message.time_to_live = time_to_live;
                return this;
            }





            public Builder setData(Dictionary<string, string> data)
            {
                message.data = data;
                return this;
            }

            public FirebaseCloudMessage build()
            {
                return message;
            }
        }

    }
}
