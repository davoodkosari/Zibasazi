using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security;
using User = Radyn.Security.DataStructure.User;

namespace Radyn.Congress.Facade
{
    internal sealed class SecurityUserFacade : CongressBaseFacade<SecurityUser>, ISecurityUserFacade
    {
        internal SecurityUserFacade()
        {
        }

        internal SecurityUserFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

  

        public User Login(string username, string password, Guid congressId)
        {
            try
            {
                var user = SecurityComponent.Instance.UserFacade.Login(username, password);
                if (user == null) return null;
                var selectFirstOrDefault = new SecurityUserBO().Select(this.ConnectionHandler, x => x.CongressId, x => x.UserId == user.Id);
                if (!selectFirstOrDefault.Any() || selectFirstOrDefault.Any(x => x == congressId))
                    return user;
                return null;

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
        public async Task<User> LoginAsync(string username, string password, Guid congressId)
        {
            try
            {
                var user =await SecurityComponent.Instance.UserFacade.LoginAsync(username, password);
                if (user == null) return null;
                var selectFirstOrDefault =await new SecurityUserBO().SelectAsync(this.ConnectionHandler, x => x.CongressId, x => x.UserId == user.Id);
                if (!selectFirstOrDefault.Any() || selectFirstOrDefault.Any(x => x == congressId))
                    return user;
                return null;

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
        public bool DeleteByUserId(Guid userId)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SecurityConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new SecurityUserBO().DeleteByUserId(this.ConnectionHandler, this.SecurityConnection, userId))
                    throw new Exception("خطایی در حذف کاربر وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.SecurityConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public Dictionary<Homa, bool> GetUserCongressList(Guid? userId)
        {
            try
            {
                return new SecurityUserBO().GetUserCongressList(this.ConnectionHandler, userId);
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
        public bool Update(User user1,  HttpPostedFileBase file, List<Guid> list)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SecurityConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new SecurityUserBO().Update(this.ConnectionHandler, this.SecurityConnection, user1, file, list))
                    throw new Exception("خطایی در ذخیره کاربر وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.SecurityConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(User user1,  HttpPostedFileBase file, List<Guid> list)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SecurityConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new SecurityUserBO().Insert(this.ConnectionHandler, this.SecurityConnection, user1, file, list))
                    throw new Exception("خطایی در ذخیره کاربر وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.SecurityConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SecurityConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


    }
}
