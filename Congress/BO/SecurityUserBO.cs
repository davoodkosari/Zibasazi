using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security;
using Radyn.Security.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class SecurityUserBO : BusinessBase<SecurityUser>
    {

        public Dictionary<Homa, bool> GetUserCongressList(IConnectionHandler connectionHandler, Guid? userId)
        {

            var getHomalList = new Dictionary<Homa, bool>();
            var predicateBuilder = new PredicateBuilder<SecurityUser>();
            if (userId.HasValue)
                predicateBuilder.And(x => x.UserId == userId);
            var @select = new SecurityUserBO().Select(connectionHandler, x => x.CongressId, predicateBuilder.GetExpression());
            var byFilter = new HomaBO().Where(connectionHandler, x => x.Enabled);
            foreach (var homa in byFilter)
            {
                var added = @select.Any(x => x.Equals(homa.Id));
                getHomalList.Add(homa, added);
            }
            return getHomalList;


        }
        public bool DeleteByUserId(IConnectionHandler connectionHandler, IConnectionHandler securityConnection, Guid userId)
        {


            if (!SecurityComponent.Instance.UserOperationTransactionalFacade(securityConnection)
                .Delete(userId, Common.Constants.OperationId.CongressOperationId))
                throw new Exception("خطایی در حذف کاربر وجود دارد");

            if (!this.DeleteByUserId(connectionHandler, userId))
                throw new Exception("خطایی در حذف کاربر وجود دارد");
            var transactionalFacade = SecurityComponent.Instance.UserTransactionalFacade(securityConnection);
            if (!transactionalFacade.Delete(userId))
                throw new Exception("خطایی در حذف کاربر وجود دارد");


            return true;
        }

        public bool DeleteByUserId(IConnectionHandler connectionHandler, Guid userId)
        {
            var articleFlowBo = new ArticleFlowBO();
            var articleFlows = articleFlowBo.Where(connectionHandler, x => x.SenderId == userId||x.ReceiverId==userId);
            foreach (var articleFlow in articleFlows)
            {

                articleFlowBo.Delete(connectionHandler, articleFlow);
            }
            var list = this.Where(connectionHandler, x => x.UserId == userId);
            foreach (var securityUser in list)
            {

                base.Delete(connectionHandler, securityUser);
            }

            return true;
        }
       
      
        internal bool Update(IConnectionHandler connectionHandler, IConnectionHandler securityConnection, Security.DataStructure.User user1, System.Web.HttpPostedFileBase file, List<Guid> list)
        {
            var transactionalFacade = SecurityComponent.Instance.UserTransactionalFacade(securityConnection);
            if (!transactionalFacade.Update(user1, file))
                throw new Exception("خطایی در ذخیره کاربر وجود دارد");
            foreach (var guid in list)
            {
                var user = this.Get(connectionHandler, guid, user1.Id);
                if (user == null)
                {
                    var securityUser = new SecurityUser { CongressId = guid, UserId = user1.Id };
                    if (!this.Insert(connectionHandler, securityUser))
                        throw new Exception("خطایی در ذخیره کاربر وجود دارد");
                }

            }
            var byFilter = this.Where(connectionHandler, x=>x.UserId==user1.Id);
            foreach (var securityUser in byFilter)
            {
                if (list.All(x => x != securityUser.CongressId))
                {
                    if (!this.Delete(connectionHandler, securityUser.CongressId, securityUser.UserId))
                        throw new Exception("خطایی در حذف کاربر وجود دارد");
                }
            }
            return true;
        }

        internal bool Insert(IConnectionHandler connectionHandler, IConnectionHandler securityConnection, Security.DataStructure.User user1,System.Web.HttpPostedFileBase file, List<Guid> list)
        {
            var transactionalFacade = SecurityComponent.Instance.UserTransactionalFacade(securityConnection);
            var firstOrDefault = transactionalFacade.FirstOrDefault(x => x.Username.ToLower() == user1.Username.ToLower());
            if (firstOrDefault == null)
            {
                if (!transactionalFacade.Insert(user1, file))
                    throw new Exception("خطایی در ذخیره کاربر وجود دارد");
            }
            else
                user1.Id = firstOrDefault.Id;

            foreach (var guid in list)
            {
                var securityUser = new SecurityUser { CongressId = guid, UserId = user1.Id };
                if (!this.Insert(connectionHandler, securityUser))
                    throw new Exception("خطایی در ذخیره کاربر وجود دارد");

            }
            var userOperation = new UserOperation() { UserId = user1.Id, OperationId = Common.Constants.OperationId.CongressOperationId };
            var userOperationTransactionalFacade = SecurityComponent.Instance.UserOperationTransactionalFacade(securityConnection);
            if (!userOperationTransactionalFacade.Any(x => x.UserId == user1.Id && x.OperationId == Common.Constants.OperationId.CongressOperationId))
            {
                if (!userOperationTransactionalFacade.Insert(userOperation))
                    throw new Exception("خطایی در ذخیره کاربر وجود دارد");
            }

            return true;
        }
    }
}
