using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.Facade
{
    internal sealed class BoothOfficerFacade : CongressBaseFacade<BoothOfficer>, IBoothOfficerFacade
    {
        internal BoothOfficerFacade()
        {
        }

        internal BoothOfficerFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

     
        public IEnumerable<ModelView.UserCardModel> GetCardList(Guid boothId, Guid userId)
        {
            try
            {
                var userBo = new UserBO();
                var boothOfficerBo = new BoothOfficerBO();
                var user = userBo.Get(this.ConnectionHandler, userId);
            
                var homa = new HomaBO().Get(this.ConnectionHandler, user.CongressId);

                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, user.CongressId,
                    homa.Configuration.CardLanguageId);
                var list = new List<ModelView.UserCardModel>();
                var userBooth = new UserBoothBO().Get(this.ConnectionHandler, userId, boothId);
                if (userBooth.Status == (byte)Enums.RezervState.PayConfirm ||
                    userBooth.Status == (byte)Enums.RezervState.Finalconfirm)
                {
                    var boothOfficers = boothOfficerBo.Where(this.ConnectionHandler, x =>
                    
                        x.BoothId == boothId&&
                        x.UserId == userId
                    );
                    list = boothOfficerBo.GetCardList(this.ConnectionHandler, user, configcontent, homa,
                        boothOfficers);

                }
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

        public ModelView.UserCardModel GetBoothOfficerCard(Guid Id, Guid boothId, Guid userId)
        {
            try
            {
                var userBo = new UserBO();
                var boothOfficerBo = new BoothOfficerBO();
                var user = userBo.Get(this.ConnectionHandler, userId);
               
              
                var homa = new HomaBO().Get(this.ConnectionHandler, user.CongressId);
                var configcontent = new ConfigurationContentBO().Get(this.ConnectionHandler, user.CongressId,
                  homa.Configuration.CardLanguageId);
                var list = new ModelView.UserCardModel();
                var userBooth = new UserBoothBO().Get(this.ConnectionHandler, userId, boothId);
                if (userBooth.Status == (byte)Enums.RezervState.PayConfirm ||
                    userBooth.Status == (byte)Enums.RezervState.Finalconfirm)
                {
                    var boothOfficers = boothOfficerBo.Get(this.ConnectionHandler, Id, boothId, userId);
                    if (boothOfficers == null) return list;
                    list =
                        boothOfficerBo.GetCardList(this.ConnectionHandler, user, configcontent, homa,
                            new List<BoothOfficer>() { boothOfficers }).FirstOrDefault();

                }
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

        public string GetNamesByBoothId(UserBooth userBooth)
        {
            try
            {

                return new BoothOfficerBO().GetNamesByBoothId(this.ConnectionHandler, userBooth);
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
