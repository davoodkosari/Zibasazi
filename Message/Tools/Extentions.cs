using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radyn.Message.Facade;

namespace Radyn.Message.Tools
{
    public static class Extentions
    {
        public static int UnreadCount(Guid userId)
        {
            return new MailBoxFacade().GetUnReadInboxCount(userId);
        }
    }
}
