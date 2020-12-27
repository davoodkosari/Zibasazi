using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.FCM
{
    public class FirebaseCloudMessageResult
    {
        [JsonProperty("message_id")]
        public long id;
    }
}
