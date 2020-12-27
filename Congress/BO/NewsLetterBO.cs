using System;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.News;
using Radyn.News.Tools;

namespace Radyn.Congress.BO
{
    internal class NewsLetterBO : BusinessBase<NewsLetter>
    {



       
        internal bool Insert(IConnectionHandler connectionHandler, Guid congressId, string email)
        {
            var newsLetter = new NewsLetter { CongressId = congressId, Email = email };
            if (!this.Insert(connectionHandler, newsLetter))
                throw new Exception("خطایی در عضویت در خبر نامه وجود دارد");
            return true;
        }

        public override bool Delete(IConnectionHandler connectionHandler, params object[] keys)
        {
            var byEmail = this.Get(connectionHandler, keys);
            if (byEmail == null)
                throw new Exception("ایمیل شما در خبر نامه ثبت نشده است");
            return base.Delete(connectionHandler, keys);
        }

       

        public bool RegsiterCongressUserModify(IConnectionHandler connectionHandler, Guid congressId, Guid userId, bool addinnews)
        {
            var enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(userId);
            if (enterpriseNode == null || string.IsNullOrEmpty(enterpriseNode.Email)) return false;
            if (addinnews)
            {
                var byEmail = this.Get(connectionHandler, congressId, enterpriseNode.Email);
                if (byEmail != null) return true;
                var newsLetter = new NewsLetter { Email = enterpriseNode.Email, CongressId = congressId };
                if (!this.Insert(connectionHandler, newsLetter))
                    throw new Exception("خطایی در عضویت در خبر نامه وجود دارد");
                return true;
            }
            if (!this.Delete(connectionHandler, congressId, enterpriseNode.Email))
                throw new Exception("خطایی در حذف از خبر نامه وجود دارد");
            return true;
        }

        public bool SentToUser(IConnectionHandler connectionHandler, Guid congressId, int newsId)
        {
            var news = NewsComponent.Instance.NewsFacade.Get(newsId);
            if (news == null) return false;
            var homa = new HomaBO().Get(connectionHandler, congressId);
            var strings = this.Select(connectionHandler, x=>x.Email,x=>x.CongressId==congressId,true).ToArray();
            var configuration = homa.Configuration;
            var newsContent = news.GetNewsContent(System.Globalization.CultureInfo.CurrentUICulture.Name);
            if (
                !MessageComponenet.Instance.MailFacade.SendGroupMailWithInterval(configuration.MailHost,
                    configuration.MailPassword, configuration.MailUserName, configuration.MailFrom,
                    configuration.MailPort, homa.CongressTitle, configuration.EnableSSL,
                    strings, newsContent.Title1,
                   newsContent.Body, intervalSecond: configuration.GroupEmailInterval))
                return false;

            return true;
        }
    }
}
