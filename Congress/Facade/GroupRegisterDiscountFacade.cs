using System;
using System.Collections.Generic;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class GroupRegisterDiscountFacade : CongressBaseFacade<GroupRegisterDiscount>,
        IGroupRegisterDiscountFacade
    {
        internal GroupRegisterDiscountFacade()
        {
        }

        internal GroupRegisterDiscountFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

     

        public override bool Update(GroupRegisterDiscount obj)
        {
            try
            {
                return new GroupRegisterDiscountBO().Update(this.ConnectionHandler, obj);
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

        public IEnumerable<GroupRegisterDiscount> GetValidList(Guid congressId)
        {
            try
            {
                return new GroupRegisterDiscountBO().GetValidList(this.ConnectionHandler, congressId);

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

      

        public override bool Insert(GroupRegisterDiscount obj)
        {
            try
            {
                return new GroupRegisterDiscountBO().Insert(this.ConnectionHandler, obj);
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
