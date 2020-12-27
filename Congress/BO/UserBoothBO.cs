using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.BO
{
    internal class UserBoothBO : BusinessBase<UserBooth>
    {
        public override bool Insert(IConnectionHandler connectionHandler, UserBooth obj)
        {
            obj.RegisterDate = DateTime.Now.ShamsiDate();
            if (obj.Status == 0)
            {
                obj.Status = (byte)Enums.RezervState.RegisterRequest;
            }

            return base.Insert(connectionHandler, obj);



        }


        public bool Delete(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, params object[] keys)
        {
            UserBooth obj = Get(connectionHandler, keys);
            if (obj.TempId.HasValue)
            {

                PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection)
                    .Delete(obj.TempId);
            }
            BoothOfficerBO boothOfficerBo = new BoothOfficerBO();
            List<BoothOfficer> list = boothOfficerBo.Where(connectionHandler, x =>

                x.UserId == obj.UserId &&
                x.BoothId == obj.BoothId, true
            );
            foreach (BoothOfficer boothOfficer in list)
            {

                if (!boothOfficerBo.Delete(connectionHandler, boothOfficer.Id, boothOfficer.BoothId, boothOfficer.UserId))
                {
                    throw new Exception("خطایی در حذف مسئول غرفه وجود دارد");
                }
            }
            return base.Delete(connectionHandler, keys);

        }
        public IEnumerable<UserBooth> Search(IConnectionHandler connectionHandler, string serachvalue, Guid boothId, byte? status, string registerDate, FormStructure formStructure)
        {
            PredicateBuilder<UserBooth> predicateBuilder = new PredicateBuilder<UserBooth>();
            predicateBuilder.And(x => x.BoothId == boothId);
            if (!string.IsNullOrEmpty(serachvalue))
            {

                predicateBuilder.And(
                (x =>
                    x.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(serachvalue) || x.EnterpriseNode.RealEnterpriseNode.LastName.Contains(serachvalue)
                || x.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(serachvalue) || x.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(serachvalue) || x.EnterpriseNode.Address.Contains(serachvalue)
                || x.EnterpriseNode.Website.Contains(serachvalue) || x.EnterpriseNode.Email.Contains(serachvalue) || x.EnterpriseNode.Tel.Contains(serachvalue)));


            }
            if (formStructure != null)
            {
                IEnumerable<string> search = FormGeneratorComponent.Instance.FormDataFacade.Search(formStructure);
                if (search.Any())
                {
                    IEnumerable<Guid> @select = search.Select(i => i.Split(',')[0].ToGuid());
                    predicateBuilder.And(x => x.UserId.In(@select));

                }

            }
            if (status.HasValue)
            {
                predicateBuilder.And(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(registerDate))
            {
                predicateBuilder.And(x => x.RegisterDate == registerDate);
            }

            return OrderByDescending(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression());

        }

        public void InformUserboothReserv(IConnectionHandler connectionHandler,Guid CongressId, ModelView.InFormEntitiyList<UserBooth> keyValuePairs)
        {


            if (!keyValuePairs.Any())
            {
                return;
            }
            Homa homa = new HomaBO().Get(connectionHandler, CongressId);
            if (homa.Configuration.BoothReserveInformType == null)return;
            string titlehoma = homa.CongressTitle;
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == CongressId && x.Type == MessageInformType.Booth);
            var @where = this.Where(connectionHandler,
                x => x.BoothId.In(keyValuePairs.Select(i => i.obj.BoothId)) &&
                     x.UserId.In(keyValuePairs.Select(i => i.obj.UserId)));
            foreach (var  shopUser in keyValuePairs)
            {
                var orDefault = @where.FirstOrDefault(x => x.BoothId == shopUser.obj.BoothId && x.UserId == shopUser.obj.UserId);
                if (orDefault == null)
                {
                    continue;
                }
                var enterpriseNode = orDefault.EnterpriseNode;
                var status = ((Enums.RezervState)orDefault.Status).GetDescriptionInLocalization();
                var homaCompleteUrl = homa.GetHomaCompleteUrl();
                var boothCode = orDefault.Booth.Code;
                string sms = string.Format(shopUser.EmailBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender,boothCode, status);
                string email = string.Format(shopUser.SmsBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender,boothCode, homaCompleteUrl,status);
                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{BoothMessageKey.BoothCode.ToString()}]", boothCode);
                        email = email.Replace($"[{BoothMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        email = email.Replace($"[{BoothMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        email = email.Replace($"[{BoothMessageKey.Status.ToString()}]", status);
                        email = email.Replace($"[{BoothMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);



                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{BoothMessageKey.BoothCode.ToString()}]", boothCode);
                        sms = sms.Replace($"[{BoothMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        sms = sms.Replace($"[{BoothMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        sms = sms.Replace($"[{BoothMessageKey.Status.ToString()}]", status);
                        sms = sms.Replace($"[{BoothMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);

                    }

                }
                

                Message.Tools.ModelView.MessageModel inform = new Message.Tools.ModelView.MessageModel
                {
                    Email = enterpriseNode.Email,
                    Mobile = enterpriseNode.Cellphone,
                    EmailTitle = homa.CongressTitle,
                    EmailBody =email,
                    SMSBody =sms
                };
                new HomaBO().SendInform((byte)homa.Configuration.BoothReserveInformType, inform, homa.Configuration, titlehoma);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa.OwnerId, homa.Configuration.CongressId,
              new[] { enterpriseNode.Id.ToString() }, homa.CongressTitle, inform.SMSBody);
            }



        }

        internal IEnumerable<ModelView.ReportChartModel> ChartNumberBoothsByDivision(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
              new Expression<Func<UserBooth, object>>[] { x => x.Status },
              new GroupByModel<UserBooth>[]
              {
                    new GroupByModel<UserBooth>()
                    {
                        Expression = x => x.Status,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
              }, x => x.Booth.CongressId == congressId);


            IEnumerable<KeyValuePair<byte, string>> enums = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.RezervState>().Select(
          keyValuePair =>
              new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.RezervState>(),
                  keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in enums)
            {
                dynamic first = list.FirstOrDefault(x => (x.Status is byte) && (byte)x.Status == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountStatus ?? 0,
                    Value = ((Enums.RezervState)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }

        public decimal GetTransactionId(IConnectionHandler connectionHandler, Guid congressId, string year = "", string moth = "")
        {

            PredicateBuilder<UserBooth> predicateBuilder = new PredicateBuilder<UserBooth>();
            predicateBuilder.And(x => x.Booth.CongressId == congressId && x.TransactionId.HasValue && x.Transaction.Done);
            if (!string.IsNullOrEmpty(moth) && !string.IsNullOrEmpty(year))
            {
                predicateBuilder.And(
                    x => x.RegisterDate.Substring(5, 2) == moth && x.RegisterDate.Substring(0, 4) == year);
            }
            else if (!string.IsNullOrEmpty(year))
            {
                predicateBuilder.And(x => x.RegisterDate.Substring(0, 4) == year);
            }

            return Sum(connectionHandler, x => x.Transaction.Amount, predicateBuilder.GetExpression());



        }


        public ModelView.ModifyResult<UserBooth> UpdateStatusAfterTransaction(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, Guid tempId)
        {
            
            UserBoothBO userBoothBo = new UserBoothBO();
            var result = new ModelView.ModifyResult<UserBooth>();
            UserBooth userBooth = userBoothBo.FirstOrDefault(connectionHandler, x => x.TempId == tempId);
            if (userBooth == null)
            {
                return result;
            }

            Transaction tr = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection).RemoveTempAndReturnTransaction(tempId);
            if (tr == null)
            {
                return result;
            }

            userBooth.TransactionId = tr.Id;
            userBooth.TempId = null;
            if (tr.PreDone)
            {
                userBooth.Status = (byte)Enums.RezervState.Pay;
                result.AddInform(userBooth, Resources.Congress.BoothPaymentEmail ,Resources.Congress.BoothPaymentSMS);
                
            }
            if (!userBoothBo.Update(paymentConnection, userBooth))
            {
                throw new Exception(Resources.Congress.ErrorInReservBooth);
            }
            result.TransactionId = tr.Id;
            result.SendInform = true;
            return result;
            
        }

        public ModelView.ModifyResult<UserBooth> UserBoothInsert(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler formGeneratorConnection, IConnectionHandler enterpriseNodeConnection, UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl,
             FormGenerator.DataStructure.FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            
            Dictionary<UserBooth, string> shopUsers = new Dictionary<UserBooth, string>();
            var modifyResult = new ModelView.ModifyResult<UserBooth>();
            Booth booth = new BoothBO().Get(connectionHandler, userBooth.BoothId);
            string additionalData = new CongressDiscountTypeBO().FillTempAdditionalData(connectionHandler, booth.CongressId);
            if (booth.ValidCost.ToDecimal() > 0)
            {

                string payer =
                    userBooth.EnterpriseNode.DescriptionField;
                Temp temp = new Temp
                {
                    PayerId = userBooth.UserId,
                    CallBackUrl = callBackurl,
                    PayerTitle = payer,
                    Description = Resources.Congress.PaymentBoothReserv + " " + booth.Code,
                    Amount = new CongressDiscountTypeBO().CalulateAmountNew(paymentConnection, (booth.ValidCost.ToDecimal()), discountAttaches),
                    CurrencyType = (byte)booth.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                    AdditionalData = additionalData

                };

                if (
                    !PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection)
                        .Insert(temp, discountAttaches))
                {
                    return modifyResult;
                }

                userBooth.TempId = temp.Id;
            }
            else
            {
                if (userBooth.TempId.HasValue)
                    PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection).Delete(userBooth.TempId);
                userBooth.TempId = null;
            }

            if (userBooth.TempId == null)
            {
                userBooth.Status = (byte)Enums.RezervState.PayConfirm;
            }
            return UserBoothInsert(connectionHandler, formGeneratorConnection, enterpriseNodeConnection, userBooth,
                formModel, boothOfficers);

          
        }

        public ModelView.ModifyResult<UserBooth> UserBoothInsert(IConnectionHandler connectionHandler,  IConnectionHandler formGeneratorConnection, IConnectionHandler enterpriseNodeConnection, UserBooth userBooth, 
           FormGenerator.DataStructure.FormStructure formModel, List<BoothOfficer> boothOfficers)
        {

            
            var modifyResult = new ModelView.ModifyResult<UserBooth>();
            
            formModel.RefId = userBooth.UserId + "," + userBooth.BoothId;
            if (
                !FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection)
                    .ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveBoothReserv);
            }

            if (!Insert(connectionHandler, userBooth))
            {
                throw new Exception(Resources.Congress.ErrorInReservBooth);
            }

            if (boothOfficers != null && boothOfficers.Any())
            {
                if (
                    !new BoothOfficerBO().BoothOfficerModify(connectionHandler, enterpriseNodeConnection,
                        boothOfficers, userBooth))
                {
                    return modifyResult;
                }
            }

            modifyResult.AddInform(userBooth, Resources.Congress.BoothInsertEmail, Resources.Congress.BoothInsertSMS);
            modifyResult.Result = true;
            modifyResult.SendInform = true;
            return modifyResult;
        }
        public ModelView.ModifyResult<UserBooth> UserBoothUpdate(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler formGeneratorConnection, IConnectionHandler enterpriseNodeConnection, UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl,
            FormGenerator.DataStructure.FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            BoothBO boothBo = new BoothBO();
            var modifyResult = new ModelView.ModifyResult<UserBooth>();
            if (userBooth.TransactionId.HasValue)
            {
                throw new Exception(Resources.Congress.ThisTransactionIsEndPleaseStartNewPayment);
            }

            Booth booth = boothBo.Get(connectionHandler, userBooth.BoothId);
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            string additionalData = new CongressDiscountTypeBO().FillTempAdditionalData(connectionHandler, booth.CongressId);
            if (booth.ValidCost.ToDecimal() > 0)
            {
                if (userBooth.TempId.HasValue)
                {
                    Temp tr = PaymentComponenets.Instance.TempFacade.Get(userBooth.TempId);
                    tr.Amount = new CongressDiscountTypeBO().CalulateAmountNew(paymentConnection,
                        (booth.ValidCost.ToDecimal()), discountAttaches);
                    tr.CurrencyType = (byte)booth.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    if (
                        !tempTransactionalFacade
                            .Update(tr, discountAttaches))
                    {
                        return modifyResult;
                    }
                }
                else
                {
                    string payer =
                      userBooth.EnterpriseNode.DescriptionField;
                    Temp temp = new Temp
                    {
                        PayerId = userBooth.UserId,
                        CallBackUrl = callBackurl,
                        PayerTitle = payer,
                        Description = Resources.Congress.PaymentBoothReserv + " " + booth.Code,
                        Amount =
                            new CongressDiscountTypeBO().CalulateAmountNew(paymentConnection,
                                (booth.ValidCost.ToDecimal()), discountAttaches),
                        CurrencyType =
                            (byte)booth.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                        AdditionalData = additionalData,

                    };

                    if (
                        !tempTransactionalFacade
                            .Insert(temp, discountAttaches))
                    {
                        return modifyResult;
                    }

                    userBooth.TempId = temp.Id;
                }

            }
            else
            {
                if (userBooth.TempId.HasValue) 
                    tempTransactionalFacade.Delete(userBooth.TempId);
                userBooth.TempId = null;
               
            }
            if (userBooth.TempId == null) userBooth.Status = (byte) Enums.RezervState.PayConfirm;
           

            return UserBoothUpdate(connectionHandler, formGeneratorConnection, enterpriseNodeConnection, userBooth,
                formModel, boothOfficers);
            
        }

        public ModelView.ModifyResult<UserBooth> UserBoothUpdate(IConnectionHandler connectionHandler,IConnectionHandler formGeneratorConnection, IConnectionHandler enterpriseNodeConnection, UserBooth userBooth, 
           FormGenerator.DataStructure.FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
           
            var modifyResult = new ModelView.ModifyResult<UserBooth>();
           
            
            if (
                !FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection)
                    .ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveBoothReserv);
            }

            if (!Update(connectionHandler, userBooth))
            {
                throw new Exception(Resources.Congress.ErrorInReservBooth);
            }

            if (boothOfficers != null && boothOfficers.Any())
            {
                if (!new BoothOfficerBO().BoothOfficerModify(connectionHandler, enterpriseNodeConnection,
                    boothOfficers, userBooth))
                {
                    return modifyResult;
                }
            }


            modifyResult.Result = true;
            modifyResult.AddInform(userBooth, Resources.Congress.BoothUpdateEmail, Resources.Congress.BoothUpdateSMS);
            modifyResult.SendInform = true;
            return modifyResult;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartNumberStandsWithReservationSeparation(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<UserBooth, object>>[] { x => x.Booth.Code },
                new GroupByModel<UserBooth>[]
                {
                    new GroupByModel<UserBooth>()
                    {
                        Expression = x => x.BoothId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.Booth.CongressId == congressId);
            List<dynamic> allType = new BoothBO().Select(connectionHandler, new Expression<Func<Booth, object>>[]
            {
                x => x.Code,
            }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Code is string) && (string)x.Code == (string)item.Code);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountBoothId ?? 0,
                    Value = (string)item.Code
                });
            }
            return listout;
        }
    }
}
