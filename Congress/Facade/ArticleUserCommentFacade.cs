using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace Radyn.Congress.Facade
{
    public sealed class ArticleUserCommentFacade : CongressBaseFacade<ArticleUserComment>, IArticleUserCommentFacade
    {
        public bool UpdateConfirmAdmin(Guid articleId, List<Guid> confirmList)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new ArticleUserCommentBO().UpdateConfirmAdmin(ConnectionHandler, articleId, confirmList))
                {
                    throw new Exception(Resources.Congress.ErrorInUpdateComments);
                }

                ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ConnectionHandler.RollBack();
                throw new KnownException(ex.Message, ex);
            }

        }
    }
}
