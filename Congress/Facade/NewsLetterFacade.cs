using System;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class NewsLetterFacade : CongressBaseFacade<NewsLetter>, INewsLetterFacade
    {
        internal NewsLetterFacade()
        {
        }

        internal NewsLetterFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

       

       

      
       

        public bool RegsiterCongressUserModify(Guid congressId,Guid userId, bool addinnews = true)
        {
            try
            {
                return new NewsLetterBO().RegsiterCongressUserModify(this.ConnectionHandler,congressId, userId, addinnews);

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

        public bool SentToUser(Guid congressId, int newsId)
        {
            try
            {
                return new NewsLetterBO().SentToUser(this.ConnectionHandler, congressId, newsId);

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
