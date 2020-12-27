using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.Tools;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.BO
{
    internal class BoothOfficerBO : BusinessBase<BoothOfficer>
    {

       
        public bool BoothOfficerModify(IConnectionHandler connectionHandler, IConnectionHandler enterpriseNodeConnection, List<BoothOfficer> boothOfficers, UserBooth userBooth)
        {
           
            var enterpriseNodeTransactionalFacade = EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeConnection);

            var list = this.Where(connectionHandler, x =>
            
                x.UserId == userBooth.UserId&&
                x.BoothId == userBooth.BoothId
            );
            foreach (var boothOfficer in boothOfficers)
            {
                var officer = this.Get(connectionHandler, boothOfficer.Id, userBooth.BoothId,userBooth.UserId);
                if (officer != null)
                {
                    if (!enterpriseNodeTransactionalFacade.Update(boothOfficer.EnterpriseNode, boothOfficer.AttachFile))
                        return false;
                }
                else
                {
                    boothOfficer.EnterpriseNode.Id = boothOfficer.Id;
                    if (!enterpriseNodeTransactionalFacade.Insert(boothOfficer.EnterpriseNode, boothOfficer.AttachFile))
                        return false;
                    boothOfficer.BoothId = userBooth.BoothId;
                    boothOfficer.UserId = userBooth.UserId;
                    if (!this.Insert(connectionHandler, boothOfficer))
                        throw new Exception(Resources.Congress.ErrorInSaveBoothOfficer);
                }

            }
            foreach (var boothOfficer in list)
            {
                if (boothOfficers.Any(x => x.BoothId == boothOfficer.BoothId && x.UserId == boothOfficer.UserId && x.Id == boothOfficer.Id)) continue;
                if (!this.Delete(connectionHandler, boothOfficer.Id, boothOfficer.BoothId, boothOfficer.UserId))
                    throw new Exception(Resources.Congress.ErrorInSaveBoothOfficer);
            }

            return true;

        }

        public List<ModelView.UserCardModel> GetCardList(IConnectionHandler connectionHandler,User user,  ConfigurationContent configcontent,
          Homa homa, List<BoothOfficer> boothOfficers)
        {
            var list = new List<ModelView.UserCardModel>();
            var fileFacade = FileManagerComponent.Instance.FileFacade;
           

            foreach (var boothOfficer in boothOfficers)
            {
                var booth = boothOfficer.Booth;
                if(booth==null)continue;
                var userKartModel = new ModelView.UserCardModel();
                if (configcontent != null && configcontent.LogoId.HasValue&& configcontent.Logo != null)
                    userKartModel.CongressLogo = configcontent.Logo.Content;
                var enterpriseNode = boothOfficer.EnterpriseNode;
                if (enterpriseNode != null)
                {
                    if (enterpriseNode.PictureId.HasValue)
                    {
                        var file = fileFacade.Get(enterpriseNode.PictureId);
                        if (file != null) userKartModel.UserImage = file.Content;
                    }

                    var realEnterpriseNode = enterpriseNode.RealEnterpriseNode;
                    if (realEnterpriseNode != null)
                    {
                        userKartModel.FirstName = realEnterpriseNode.FirstName;
                        userKartModel.LastName = realEnterpriseNode.LastName;
                        userKartModel.NationalCode = realEnterpriseNode.NationalCode;
                    }
                  
                }
                userKartModel.Id = user.Id.ToString();
                userKartModel.CongressTitle = homa.CongressTitle;
                userKartModel.UseName = enterpriseNode.Title();
                userKartModel.Type = string.Format(Resources.Congress.BoothOfficerCard, booth.Code);
                userKartModel.CardType = Enums.CardType.BoothOfficerCard;
                userKartModel.CardId = Enums.CardType.BoothOfficerCard+"-"+user.Id;
                list.Add(userKartModel);
            }
            return list;

        }


        public string GetNamesByBoothId(IConnectionHandler connectionHandler, UserBooth userBooth)
        {
            var list = this.Select(connectionHandler,x=>x.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.EnterpriseNode.RealEnterpriseNode.LastName, x =>
            
                x.UserId == userBooth.UserId&&
                x.BoothId == userBooth.BoothId
            );
            var str = "";
            foreach (var boothOfficer in list)
            {
               
                if (!string.IsNullOrEmpty(str)) str += ",";
                str += boothOfficer;
            }
            return str;
        }

        
    }
}
