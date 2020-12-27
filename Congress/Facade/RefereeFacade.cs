using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.Framework;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class RefereeFacade : CongressBaseFacade<Referee>, IRefereeFacade
    {

      
        public List<Referee> GetAllrefreeWithCartable(Guid homaId)
        {
            var bo = new RefereeBO();
            var list = bo.OrderBy(this.ConnectionHandler,x=>x.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.EnterpriseNode.RealEnterpriseNode.LastName,x=>x.CongressId== homaId);
            var refereeBo = new RefereeBO();
            refereeBo.SetArticleCount(ConnectionHandler, list, homaId);
            return list;
        }
        

        public List<Referee> SearchRefree(string text, Guid congressId)
        {
            var list = new RefereeBO().SearchRefree(ConnectionHandler, text, congressId);
            var refereeBo = new RefereeBO();
            refereeBo.SetArticleCount(ConnectionHandler, list, congressId);
            return list;
        }
        public bool Insert(Referee referee, 
            HttpPostedFileBase fileBase, List<Guid> pivots)
        {
            var dictionary = new ModelView.InFormEntitiyList<Referee>();
            var result = false;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!string.IsNullOrEmpty(referee.Password))
                    referee.PasswordWithoutHash = referee.Password;
                if (!new RefereeBO().Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, referee, fileBase, pivots))
                    throw new Exception(Resources.Congress.ErrorInSaveReferee);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                if (referee.SendInform)
                    dictionary.Add(referee, Resources.Congress.RefereeInsertEmail, Resources.Congress.RefereeInsertSMS);
                result = true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

            try
            {
                //ارسال پیغام به داور
                if (result)
                    new RefereeBO().InformRefereeRegister(this.ConnectionHandler,referee.CongressId, dictionary);

            }
            catch
            {


            }
            return result;

        }

        public bool Update(Referee referee, 
            HttpPostedFileBase fileBase, List<Guid> pivots)
        {
            var dictionary = new ModelView.InFormEntitiyList<Referee>();
            bool result;
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!string.IsNullOrEmpty(referee.Password))
                {
                    referee.PasswordWithoutHash = referee.Password;
                    referee.Password = StringUtils.HashPassword(referee.Password);
                }
                else
                    referee.Password = new RefereeBO().Get(this.ConnectionHandler, referee.Id).Password;
                if (!new RefereeBO().Update(this.ConnectionHandler, this.EnterpriseNodeConnection, referee, fileBase, pivots))
                    throw new Exception(Resources.Congress.ErrorInEditReferee);


                if (referee.SendInform)
                {
                    //پر کردن آبجکت برای مشخص کردن نحوه ی ارسال پیغام به داور
                    dictionary.Add(referee,
                        Resources.Congress.RefereeInsertEmail,
                        Resources.Congress.RefereeInsertSMS);
                }
                result = true;

                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

            try
            {
                //ارسال پیغام به داور
                if (result)
                    new RefereeBO().InformRefereeRegister(this.ConnectionHandler, referee.CongressId, dictionary);
            }
            catch
            {
            }
            return result;


        }

        public override bool Delete(params object[] keys)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new RefereeBO().Get(this.ConnectionHandler, keys);
                
                var byFilter = new RefereeCartableBO().Any(this.ConnectionHandler,
                    cartable => cartable.RefereeId == obj.Id);
                if (byFilter)
                    throw new Exception(Resources.Congress.ErrorInDeleteRefereeBecauseHisCartablContainArticle);
                var refereePivotBo = new RefereePivotBO();
                var list = refereePivotBo.Select(this.ConnectionHandler, x => x.PivotId, x => x.RefereeId == obj.Id);
                foreach (var refereePivot in list)
                {
                    if (!refereePivotBo.Delete(this.ConnectionHandler, obj.Id, refereePivot))
                        throw new Exception(Resources.Congress.ErrorInEditReferee);
                }
                if (!new RefereeBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteReferee);
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Delete(keys))
                    return false;

                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public Referee Login(string username, string password, Guid CongressId)
        {
            try
            {

                var hashPassword = StringUtils.HashPassword(password);
                var lower = username.ToLower();
                return new RefereeBO().FirstOrDefault(this.ConnectionHandler, referee =>
                
                    referee.CongressId == CongressId &&
                    referee.Username == lower &&
                    referee.Password == hashPassword
                );

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

        } public async Task<Referee> LoginAsync(string username, string password, Guid CongressId)
        {
            try
            {

                var hashPassword = StringUtils.HashPassword(password);
                var lower = username.ToLower();
                return await new RefereeBO().FirstOrDefaultAsync(this.ConnectionHandler, referee =>
                
                    referee.CongressId == CongressId &&
                    referee.Username == lower &&
                    referee.Password == hashPassword
                );

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

        public bool ChangePassword(Guid refreeId, string password)
        {
            try
            {

                var refereeBO = new RefereeBO();
                var referee = refereeBO.Get(this.ConnectionHandler, refreeId);
                referee.Password = StringUtils.HashPassword(password);
                return refereeBO.Update(this.ConnectionHandler, referee);
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

        public bool CheckOldPassword(Guid refreeId, string password)
        {
            try
            {
                var referee = new RefereeBO().Get(this.ConnectionHandler, refreeId);
                return referee.Password.Equals(StringUtils.HashPassword(password));

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

       

        public Dictionary<Referee, bool> GetAllForArticle(Guid congressId, Guid articleId, bool isSpecial = false)
        {
            try
            {
                var list = new RefereeBO().OrderBy(this.ConnectionHandler,x=>x.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.EnterpriseNode.RealEnterpriseNode.LastName, x => x.CongressId == congressId);
                var getAllForArticle = new Dictionary<Referee, bool>();
                var refereeCartableBo = new RefereeCartableBO();
                var addedList = refereeCartableBo.Select(this.ConnectionHandler,x=>x.RefereeId, x => x.ArticleId == articleId && x.IsActive,true);
                var listAllCartable = refereeCartableBo.Select(this.ConnectionHandler,new Expression<Func<RefereeCartable, object>>[] { x => x.RefereeId ,x=>x.Status}, x => x.Referee.CongressId == congressId,true);

                foreach (var referee in list)
                {
                    var res = addedList.Any(x => x == referee.Id);
                    if (!res&&referee.IsSpecial != isSpecial) continue;
                    referee.AllArticleCount = listAllCartable.Count(x => x.RefereeId == referee.Id);
                    referee.WaitForAnswerArticleCount = listAllCartable.Count(x => x.RefereeId == referee.Id && x.Status == (byte)Enums.FinalState.WaitForAnswer);
                    getAllForArticle.Add(referee, res);
                }
                return getAllForArticle;
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

        public Dictionary<Referee, List<string>> ImportFromExcel(HttpPostedFileBase fileBase, Guid congressId)
        {
            try
            {

                return new RefereeBO().ImportFromExcel(this.ConnectionHandler, fileBase, congressId);
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

        public bool InsertList(List<Referee> referee)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var @select = new RefereeBO().Select(ConnectionHandler, x => x.Id);
                var refereeBo = new RefereeBO();
                foreach (var refe in referee)
                {
                    if (@select.All(x => x != refe.Id))
                    {
                        if (!refereeBo.Insert(this.ConnectionHandler, this.EnterpriseNodeConnection, refe))
                            throw new Exception(Resources.Congress.ErrorInSaveUser);
                        @select.Add(refe.Id);
                    }
                    else
                    {
                        if (!refereeBo.Update(this.ConnectionHandler, this.EnterpriseNodeConnection, refe))
                            throw new Exception(Resources.Congress.ErrorInSaveUser);
                    }
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                return true;

            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }
    }
}
