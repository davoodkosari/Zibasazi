using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FAQ;
using Radyn.FAQ.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressFAQFacade : CongressBaseFacade<CongressFAQ>, ICongressFAQFacade
    {
        internal CongressFAQFacade()
        {
        }

        internal CongressFAQFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FaqConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressFaqbo = new CongressFAQBO();
                var obj = congressFaqbo.Get(this.ConnectionHandler, keys);
                if (!congressFaqbo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressFAQ);
                if (!FAQComponent.Instance.FAQTransactionalFacade(this.FaqConnection).Delete(obj.FAQId))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressFAQ);
                this.ConnectionHandler.CommitTransaction();
                this.FaqConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FaqConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FaqConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<FAQ.DataStructure.FAQ> Search(Guid congressId, string value)
        {
            try
            {

                var list = new List<FAQ.DataStructure.FAQ>();
                var faqFacade = FAQComponent.Instance.FAQFacade.Search(value);
                var congressFaqbo = new CongressFAQBO();
                foreach (var congressHtml in faqFacade)
                {
                    var html = congressFaqbo.Get(this.ConnectionHandler, congressId, congressHtml.Id);
                    if (html == null) continue;
                    list.Add(congressHtml);
                }
                return list;
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

        public bool Insert(Guid congressId, FAQ.DataStructure.FAQ faq, FAQContent faqContent, HttpPostedFileBase image)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FaqConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                faq.IsExternal = true;
                if (!FAQComponent.Instance.FAQTransactionalFacade(this.FaqConnection).Insert(faq, faqContent, image))
                    throw new Exception(Resources.Congress.ErrorInInsertFAQ);
                var congressFaq = new CongressFAQ {FAQId = faq.Id, CongressId = congressId};
                if (!new CongressFAQBO().Insert(this.ConnectionHandler, congressFaq))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressFAQ);
                this.ConnectionHandler.CommitTransaction();
                this.FaqConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FaqConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FaqConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

      


    }
}
