using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class UserFormsFacade : CongressBaseFacade<UserForms>, IUserForms
    {
        internal UserFormsFacade()
        {
        }

        internal UserFormsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public DataTable ReportUserForms(Guid formId, Guid homaId, string culture)
        {
            try
            {
                var result = new UserFormsBO().ReportUserForms(this.ConnectionHandler, formId, homaId, culture);
                return result;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
