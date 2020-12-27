using System;
using System.Collections.Generic;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class UserRegisterPaymentTypeFacade : CongressBaseFacade<UserRegisterPaymentType>, IUserRegisterPaymentTypeFacade
    {
        internal UserRegisterPaymentTypeFacade()
        {
        }

        internal UserRegisterPaymentTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      
        public IEnumerable<ModelView.ReportChartModel> ChartRegisterTypeCount(Guid congressId)
        {
            try
            {
                return new UserRegisterPaymentTypeBO().ChartRegisterTypeCount(this.ConnectionHandler, congressId);
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

        public bool Insert(UserRegisterPaymentType registerPaymentType, Dictionary<int, decimal> keyValuePairs)
        {
            try
            {
                
                

                var supportTypeBo = new UserRegisterPaymentTypeBO();
                supportTypeBo.SetDaysInfo(ref registerPaymentType, keyValuePairs);
                if (!supportTypeBo.Insert(this.ConnectionHandler, registerPaymentType))
                    throw new Exception(Resources.Congress.ErrorInSaveSupportType);
                
              
               
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

        public bool Update(UserRegisterPaymentType registerPaymentType,  Dictionary<int, decimal> keyValuePairs)
        {
            try
            {
               
                var userRegisterPaymentTypeBo = new UserRegisterPaymentTypeBO();
                userRegisterPaymentTypeBo.SetDaysInfo(ref registerPaymentType, keyValuePairs);
                if (!userRegisterPaymentTypeBo.Update(this.ConnectionHandler, registerPaymentType))
                    throw new Exception(Resources.Congress.ErrorInEditSupportType);
               
              
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

        
        public IEnumerable<UserRegisterPaymentType> GetValidListUser(Guid congressId)
        {
            try
            {
                var list = new UserRegisterPaymentTypeBO().OrderBy(this.ConnectionHandler,x => x.Order,
                    x => x.CongressId == congressId);
                var outlist = new List<UserRegisterPaymentType>();
                foreach (var userRegisterPaymentType in list)
                {
                    if (userRegisterPaymentType.Capacity == 0 || string.IsNullOrEmpty(userRegisterPaymentType.Title)||!userRegisterPaymentType.CanUserSelect)
                        continue;
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

        public Dictionary<int, decimal> GetDaysInfo(Guid congressId, string culture, Guid? registerPaymentTypeId)
        {
            try
            {
                var homa = new HomaBO().Get(this.ConnectionHandler, congressId);
                if (homa == null || homa.HoldingDays == null || homa.HoldingDays.ToString().ToInt() == 0) return null;
                var dictionary = new Dictionary<int, decimal>();
                var oldictionary = new Dictionary<int, decimal>();
                var type = new UserRegisterPaymentType();
                if (registerPaymentTypeId.HasValue)
                {
                    var registerPaymentType = new UserRegisterPaymentTypeBO().GetLanuageContent(this.ConnectionHandler,
                        culture, registerPaymentTypeId);
                    if (registerPaymentType != null) type = registerPaymentType;
                }
                if (!string.IsNullOrEmpty(type.DaysInfo))
                {
                    var split = type.DaysInfo.Split('-');
                    foreach (var value in split)
                    {
                        if (string.IsNullOrEmpty(value)) continue;
                        var strings = value.Split(',');
                        oldictionary.Add(strings[0].ToInt(), strings[1].ToDecimal());
                    }
                }
                for (var i = 1; i <= homa.HoldingDays; i++)
                {
                    dictionary.Add(i,oldictionary.ContainsKey(i)? oldictionary[i].ToString().ToDecimal(): 0);
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

        public Dictionary<int, decimal> GetRegiterTypeInfo(Guid registerPaymentTypeId)
        {
            try
            {

                var dictionary = new Dictionary<int, decimal>();
                var registerPaymentType = new UserRegisterPaymentTypeBO().Get(this.ConnectionHandler, registerPaymentTypeId);
                if (registerPaymentType == null) return dictionary;
                if (!string.IsNullOrEmpty(registerPaymentType.DaysInfo))
                {
                    var split = registerPaymentType.DaysInfo.Split('-');
                    foreach (var value in split)
                    {
                        if (string.IsNullOrEmpty(value)) continue;
                        var strings = value.Split(',');
                        dictionary.Add(strings[0].ToInt(), strings[1].ToDecimal());
                    }
                }
                else
                   dictionary.Add(0, registerPaymentType.ValidAmount.ToDecimal());
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
    }
}
