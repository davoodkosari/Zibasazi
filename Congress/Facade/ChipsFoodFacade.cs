using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class ChipsFoodFacade : CongressBaseFacade<ChipsFood>, IChipsFoodFacade
    {
        internal ChipsFoodFacade() { }

        internal ChipsFoodFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

       
        public Dictionary<User, bool> SearchChipFood(Guid congressId, Guid chipFoodId, string txtSearch, User user,EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure)
        {
            try
            {

                var list= new ChipsFoodBO().SearchChipFood(this.ConnectionHandler, congressId, chipFoodId, txtSearch, user, gender, formStructure);
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

        public IEnumerable<ModelView.UserCardModel> SearchChipFoodReport(Guid congressId, Guid chipfoodId, string txtSearch, User user, EnterpriseNode.Tools.Enums.Gender gender,
            FormStructure postFormData)
        {
            try
            {
                var list = new ChipsFoodBO().SearchChipFood(this.ConnectionHandler, congressId, chipfoodId, txtSearch, user, gender, postFormData);
                var enumerable = list.Where(x => x.Value).Select(x => x.Key);
                var userCardModels = new List<ModelView.UserCardModel>();
                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
                var config = homa.Configuration;
                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, congressId,
                    config.CardLanguageId);
              
                var chipsFoodUserBo = new ChipsFoodBO();
                var chipsFood = chipsFoodUserBo.Get(this.ConnectionHandler, chipfoodId);
                foreach (var keyValuePair in enumerable)
                {
                    userCardModels.AddRange(new UserBO().GetChipFootUser(this.ConnectionHandler, keyValuePair, configcontent, homa, new List<ChipsFood> { chipsFood }));
                }
                return userCardModels;
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

      

        public ModelView.UserCardModel SearchChipFoodReport(Guid chipfoodId, Guid userId)
        {
            try
            {
                var userCardModels = new ModelView.UserCardModel();
                var userBo = new UserBO();
                var user = userBo.Get(this.ConnectionHandler, userId);
                var config = new ConfigurationBO().Get(this.ConnectionHandler, user.CongressId);
                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, user.CongressId,config.CardLanguageId);
                var homa = new HomaBO().Get(this.ConnectionHandler, user.CongressId);
                var chipsFoodUserBo = new ChipsFoodUserBO();
                var chipsFoodUsers = chipsFoodUserBo.Get(this.ConnectionHandler, chipfoodId, user.Id);
                var chipsFoodBo = new ChipsFoodBO();
                if (chipsFoodUsers != null)
                {
                    var chipsFood = chipsFoodBo.Get(this.ConnectionHandler, chipsFoodUsers.ChipsFoodId);
                    var cardModels = userBo.GetChipFootUser(this.ConnectionHandler, user, configcontent, homa, new List<ChipsFood> { chipsFood });
                    if (cardModels.Count > 0)
                    {
                        userCardModels = cardModels.FirstOrDefault();
                    }
                }

                return userCardModels;
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

        public bool Insert(ChipsFood chipsFood,List<int> days)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                
                var chipsFoodBo = new ChipsFoodBO();
                if (!chipsFoodBo.Insert(this.ConnectionHandler,  chipsFood, days))
                    throw new Exception(Resources.Congress.ErrorInSaveChipFood);
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

        public bool Update(ChipsFood chipsFood,  List<int> days)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
               var chipsFoodBo = new ChipsFoodBO();
                if (!chipsFoodBo.Update(this.ConnectionHandler,  chipsFood, days))
                    throw new Exception(Resources.Congress.ErrorInSaveChipFood);

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

        public Dictionary<int, bool> GetDaysInfo(Guid congressId, Guid? chipfoodId)
        {
            try
            {
                var dictionary = new Dictionary<int, bool>();
                if (chipfoodId.HasValue)
                {
                    var chipsFood = new ChipsFoodBO().Get(this.ConnectionHandler, chipfoodId);
                    if (chipsFood != null)
                    {
                        if (!string.IsNullOrEmpty(chipsFood.DaysInfo))
                        {
                            var split = chipsFood.DaysInfo.Split('-');
                            foreach (var value in split)
                            {
                                if (string.IsNullOrEmpty(value)) continue;
                                dictionary.Add(value.ToInt(), true);
                            }
                        }
                    }

                }

                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
                if (homa == null || homa.HoldingDays == null || homa.HoldingDays.ToString().ToInt() == 0) return null;
                for (var i = 1; i <= homa.HoldingDays; i++)
                {
                    if (!dictionary.ContainsKey(i))
                        dictionary.Add(i, false);
                }
                return dictionary;
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

        public bool JoinUser(Guid chipsFoodid, List<Guid> list)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var chipsFoodBo = new ChipsFoodBO();
                if (!chipsFoodBo.JoinUser(this.ConnectionHandler, chipsFoodid, list))
                    throw new Exception(Resources.Congress.ErrorInSaveChipFood);
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

   
    }
}
