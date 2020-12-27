using System;
using Radyn.Congress.DA;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class ConfigurationBO : BusinessBase<Configuration>
    {

        public override bool Insert(IConnectionHandler connectionHandler, Configuration obj)
        {
            if (!string.IsNullOrEmpty(obj.TerminalPassword))
                obj.TerminalPassword = Utility.StringUtils.Encrypt(obj.TerminalPassword);
            if (!string.IsNullOrEmpty(obj.SMSAccountPassword))
                obj.SMSAccountPassword = Utility.StringUtils.Encrypt(obj.SMSAccountPassword);
            if (!string.IsNullOrEmpty(obj.MailPassword))
                obj.MailPassword = Utility.StringUtils.Encrypt(obj.MailPassword);
            return base.Insert(connectionHandler, obj);
        }

        protected override void CheckConstraint(IConnectionHandler connectionHandler, Configuration item)
        {
            if (item.OrginalFinishDate.CompareTo(item.OrginalStartDate) < 0)
                throw new Exception("تاریخ پایان دریافت اصل  نمیتواند قبل از تاریخ شروع دریافت اصل  باشد");
            if (item.AbstractFinishDate.CompareTo(item.AbstractStartDate) < 0)
                throw new Exception("تاریخ پایان دریافت چکیده نمیتواند قبل از تاریخ شروع دریافت چکیده باشد");
        }

        public Configuration ValidConfig(IConnectionHandler connectionHandler, Guid congressId)
        {
            var configuration = new ConfigurationDA(connectionHandler).Get(congressId);
            return configuration ?? new Configuration();
        }



    }
}
