using System;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class BoothCatgoryFacade : CongressBaseFacade<BoothCatgory>, IBoothCatgoryFacade
    {
        internal BoothCatgoryFacade()
        {
        }

        internal BoothCatgoryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        
        public override bool Delete(params object[] keys)
        {
            try
            {
                var obj = new BoothCatgoryBO().SimpleGet(this.ConnectionHandler, keys);
                var list = new BoothBO().Any(ConnectionHandler,supporter => supporter.CategoryId == obj.Id);
                if (list)
                    throw new Exception(Resources.Congress.ErrorInDeleteBoothcategoryBecauseThereAreBoothWithThisType);
                if (!new BoothCatgoryBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteBoothCategory);
                return true;

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
