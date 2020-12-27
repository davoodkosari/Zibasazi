using System;
using System.Collections.Generic;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class ArticlePaymentTypeFacade : CongressBaseFacade<ArticlePaymentType>, IArticlePaymentTypeFacade
    {
        internal ArticlePaymentTypeFacade()
        {
        }

        internal ArticlePaymentTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public override bool Delete(params object[] keys)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
               
                var articlePaymentTypeBo = new ArticlePaymentTypeBO();
                var obj = articlePaymentTypeBo.Get(this.ConnectionHandler, keys);
                var configurationSupportTypes = new ArticleBO().Any(ConnectionHandler,
                    supporter => supporter.PaymentTypeId == obj.Id);
                if (configurationSupportTypes)
                    throw new Exception(string.Format(Resources.Congress.ErrorInDeleteArticlePaymentTypeThisUseInArticle, Tools.Extention.GetAtricleTitle(obj.CongressId), Tools.Extention.GetAtricleTitle(obj.CongressId)));
                if (!articlePaymentTypeBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(string.Format(Resources.Congress.ErrorInDeleteArticlePaymentTypeThisUseInArticle, Tools.Extention.GetAtricleTitle(obj.CongressId), Tools.Extention.GetAtricleTitle(obj.CongressId)));
                
                this.ConnectionHandler.CommitTransaction();
               
                return true;

            }

            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
              Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }


        }

       

        public IEnumerable<ArticlePaymentType> GetValidList(Guid congressId)
        {
            try
            {
                var list = new ArticlePaymentTypeBO().Where(this.ConnectionHandler,
                    x => x.CongressId == congressId);
                var outlist = new List<ArticlePaymentType>();
                foreach (var userRegisterPaymentType in list)
                {
                    if (string.IsNullOrEmpty(userRegisterPaymentType.Title)) continue;
                    outlist.Add(userRegisterPaymentType);
                }
                return outlist;
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
