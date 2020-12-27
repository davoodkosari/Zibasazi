using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.FCM
{
    public class Notification
    {

        public string body;

        public string title;

        public string icon;

        public string sound;

        public long[] vibrate;
        public class Builder
        {
            Notification notification = new Notification();

            public Builder setBody(String value)
            {
                notification.body = value;
                return this;
            }

            public Builder setTitle(String value)
            {
                notification.title = value;
                return this;
            }

            public Builder setIcon(String value)
            {
                notification.icon = value;
                return this;
            }

            public Builder setSound(String value)
            {
                notification.sound = value;
                return this;
            }

            public Builder setSound(long[] pattern)
            {
                notification.vibrate = pattern;
                return this;
            }



            public Notification build()
            {
                return notification;
            }

        }

    }
}
