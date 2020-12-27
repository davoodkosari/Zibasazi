using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
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
    internal class WorkShopUserBO : BusinessBase<WorkShopUser>
    {
        public override bool Insert(IConnectionHandler connectionHandler, WorkShopUser obj)
        {
            obj.RegisterDate = DateTime.Now.ShamsiDate();
            if (obj.Status == 0)
            {
                obj.Status = (byte)Enums.WorkShopRezervState.RegisterRequest;
            }

            return base.Insert(connectionHandler, obj);
        }



        public bool Delete(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, params object[] keys)
        {
            WorkShopUser obj = Get(connectionHandler, keys);
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            if (obj == null)
            {
                return false;
            }

            if (obj.TempId.HasValue) tempTransactionalFacade.Delete(obj.TempId);
            if (!base.Delete(connectionHandler, keys))
            {
                throw new Exception(Resources.Congress.ErrorInDeleteWorkShop);
            }

            return true;

        }
        public IEnumerable<WorkShopUser> Search(IConnectionHandler connectionHandler, Guid workshopId, byte? status, string registerDate, string txtSearch,
            FormStructure formStructure)
        {
            PredicateBuilder<WorkShopUser> predicateBuilder = new PredicateBuilder<WorkShopUser>();
            predicateBuilder.And(x => x.WorkShopId == workshopId);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                predicateBuilder.And((x => x.User.Username.Contains(txtSearch) || x.User.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(txtSearch) || x.User.EnterpriseNode.RealEnterpriseNode.LastName.Contains(txtSearch)
                || x.User.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(txtSearch) || x.User.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(txtSearch) || x.User.EnterpriseNode.Address.Contains(txtSearch)
                || x.User.EnterpriseNode.Website.Contains(txtSearch) || x.User.EnterpriseNode.Email.Contains(txtSearch) || x.User.EnterpriseNode.Tel.Contains(txtSearch)));

            }

            if (formStructure != null)
            {
                IEnumerable<string> @where = FormGeneratorComponent.Instance.FormDataFacade.Search(formStructure);
                if (@where.Any())
                {
                    predicateBuilder.And(x => x.UserId.In(@where.Select(s => s.ToGuid())));
                }
            }
            if (status != null)
            {
                predicateBuilder.And(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(registerDate))
            {
                predicateBuilder.And(x => x.RegisterDate == registerDate);
            }

            return OrderByDescending(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression());

        }
        public IEnumerable<ModelView.ReportChartModel> ChartWorkShopCountByReserv(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<WorkShopUser, object>>[] { x => x.WorkShop.Subject },
                new GroupByModel<WorkShopUser>[]
                {
                    new GroupByModel<WorkShopUser>()
                    {
                        Expression = x => x.WorkShopId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.WorkShop.CongressId == congressId);
            List<dynamic> allType = new WorkShopBO().Select(connectionHandler, new Expression<Func<WorkShop, object>>[]
            {
                x => x.Subject
            }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Subject is string) && (string)x.Subject == (string)item.Subject);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountWorkShopId ?? 0,
                    Value = (string)item.Subject
                });
            }
            return listout;
        }

        public IEnumerable<ModelView.ReportChartModel> ChartWorkShopCountStatus(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<WorkShopUser, object>>[] { x => x.Status },
                new GroupByModel<WorkShopUser>[]
                {
                    new GroupByModel<WorkShopUser>()
                    {
                        Expression = x => x.Status,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.WorkShop.CongressId == congressId);
            IEnumerable<KeyValuePair<byte, string>> enums = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.WorkShopRezervState>().Select(
            keyValuePair =>
                new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.WorkShopRezervState>(),
                    keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in enums)
            {
                dynamic first = list.FirstOrDefault(x => (x.Status is byte) && (byte)x.Status == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountStatus ?? 0,
                    Value = ((Enums.WorkShopRezervState)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }


        public KeyValuePair<bool, Guid> WorkShopUserInsert(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler formGeneratorConnection, Guid workShopId, User parentUser, List<DiscountType> discountAttaches, string callBackurl, FormStructure formModel, List<Guid> users)
        {

            Guid? outvalue = null;
            WorkShopBO workShopBo = new WorkShopBO();
            WorkShop workShop = workShopBo.Get(connectionHandler, workShopId);
            int newlistCount = 0;
            List<Guid> validlist = new List<Guid>();
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            List<Guid> @select = Select(connectionHandler, x => x.UserId, x => x.WorkShopId == workShopId);
            CongressDiscountTypeBO congressDiscountTypeBo = new CongressDiscountTypeBO();
            string additionalData = congressDiscountTypeBo.FillTempAdditionalData(connectionHandler, workShop.CongressId);

            foreach (Guid guid1 in users)
            {
                bool hotelUser = @select.Any(x => x == guid1);
                if (!hotelUser)
                {
                    newlistCount++;
                    validlist.Add(guid1);
                }
                else
                {
                    validlist.Add(guid1);
                }
            }
            if (workShop.FreeCapicity < newlistCount)
            {
                throw new Exception(Resources.Congress.WorkShopCapacityLessThanNumberOfSelectedUser);
            }

            List<WorkShopUser> list = Where(connectionHandler, x => (x.UserId == parentUser.Id || x.User.ParentId == parentUser.Id) && x.WorkShopId == workShopId, true);
            foreach (WorkShopUser workShopUser in list)
            {
                if (validlist.Any(x => x.Equals(workShopUser.UserId)))
                {
                    continue;
                }

                if (workShopUser.Status != (byte)Enums.WorkShopRezervState.RegisterRequest &&
                    workShopUser.Status != (byte)Enums.WorkShopRezervState.DenialPay)
                {
                    continue;
                }

                if (workShopUser.TempId.HasValue)
                {
                    tempTransactionalFacade.Delete(workShopUser.TempId);
                }
                if (!base.Delete(connectionHandler, workShopUser))
                {
                    throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                }
            }
            if (workShop.ValidCost.ToDecimal() > 0 && validlist.Any())
            {
                outvalue = SelectFirstOrDefault(connectionHandler, x => x.TempId, x => x.UserId.In(validlist) && x.WorkShopId == workShopId);
                decimal calulateAmountNew = congressDiscountTypeBo.CalulateAmountNew(paymentConnection, (workShop.ValidCost.ToDecimal()) * validlist.Count, discountAttaches);
                if (outvalue == null)
                {
                    outvalue = Guid.NewGuid();
                    Temp temp = new Temp
                    {
                        Id = (Guid)outvalue,
                        PayerId = parentUser.Id,
                        CallBackUrl = callBackurl + outvalue,
                        Description = Resources.Congress.PaymentWorkShopReserv + " " + workShop.Subject,
                        PayerTitle = parentUser.DescriptionField,
                        Amount = calulateAmountNew,
                        CurrencyType = (byte)workShop.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                        AdditionalData = additionalData

                    };


                    if (!tempTransactionalFacade.Insert(temp, discountAttaches))
                    {
                        return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    }
                }
                else
                {
                    Temp tr = tempTransactionalFacade.Get(outvalue);
                    tr.Amount = calulateAmountNew;
                    tr.CurrencyType = (byte)workShop.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    tr.AdditionalData = additionalData;
                    if (!tempTransactionalFacade.Update(tr, discountAttaches))
                    {
                        return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    }
                }
            }
            FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
            List<WorkShopUser> @selectd = Where(connectionHandler, x => x.WorkShopId == workShopId && x.UserId.In(validlist), true);
            foreach (Guid id in validlist)
            {
                WorkShopUser workShopUser = @selectd.FirstOrDefault(x => x.UserId == id);
                if (workShopUser == null)
                {
                    WorkShopUser shopUser = new WorkShopUser() { UserId = id, WorkShopId = workShopId };
                    if (outvalue != null)
                    {
                        shopUser.TempId = outvalue;
                    }
                    else
                    {
                        shopUser.TempId = null;
                        shopUser.Status = (byte)Enums.WorkShopRezervState.PayConfirm;
                    }
                    if (!Insert(connectionHandler, shopUser))
                    {
                        throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                    }
                }
                else
                {
                    if (outvalue != null)
                    {
                        workShopUser.TempId = outvalue;
                    }
                    else
                    {
                        if (workShopUser.TempId.HasValue)
                            tempTransactionalFacade.Delete(workShopUser.TempId);
                        workShopUser.TempId = null;
                        workShopUser.Status = (byte)Enums.WorkShopRezervState.PayConfirm;
                        
                    }
                    if (!Update(connectionHandler, workShopUser))
                    {
                        throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                    }
                }
                formModel.RefId = id + "," + workShopId;
                if (!formDataTransactionalFacade.ModifyFormData(formModel))
                {
                    throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                }
            }
            return new KeyValuePair<bool, Guid>(true, outvalue != null ? (Guid)outvalue : Guid.Empty);
        }

        public bool WorkShopUserInsert(IConnectionHandler connectionHandler, IConnectionHandler formGeneratorConnection, WorkShopUser WorkShopUser, FormGenerator.DataStructure.FormStructure formModel)
        {

            
            FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
            if (!Insert(connectionHandler, WorkShopUser))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            formModel.RefId = WorkShopUser.UserId + "," + WorkShopUser.WorkShopId;
            if (!formDataTransactionalFacade.ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            return true;


        }
        public bool WorkShopUserUpdate(IConnectionHandler connectionHandler, IConnectionHandler formGeneratorConnection, WorkShopUser WorkShopUser, FormGenerator.DataStructure.FormStructure formModel)
        {

           
            FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
            if (!Update(connectionHandler, WorkShopUser))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            formModel.RefId = WorkShopUser.UserId + "," + WorkShopUser.WorkShopId;
            if (!formDataTransactionalFacade.ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            return true;


        }
        public void InformWorkShopReserv(IConnectionHandler connectionHandler,Guid congressId, ModelView.InFormEntitiyList<WorkShopUser> workShopUser)
        {
            if (!workShopUser.Any())
            {
                return;
            }
            Homa homa = new HomaBO().Get(connectionHandler, congressId);
            Configuration config = homa.Configuration;
            if (config.UserWorkReserveShopInformType == null)return;
            string titlehoma = homa.CongressTitle;
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Type == MessageInformType.Workshop);
            var @where = this.Where(connectionHandler,
                x => x.WorkShopId.In(workShopUser.Select(i => i.obj.WorkShopId)) &&
                     x.UserId.In(workShopUser.Select(i => i.obj.UserId)));
            foreach (var  shopUser in workShopUser)
            {

                var firstOrDefault = @where.FirstOrDefault(x =>x.WorkShopId == shopUser.obj.WorkShopId && x.UserId == shopUser.obj.UserId);
                if (firstOrDefault == null)
                    continue;
                var enterpriseNode = firstOrDefault.User.EnterpriseNode;
                var workShop = firstOrDefault.WorkShop;
              
                var homaCompleteUrl = homa.GetHomaCompleteUrl();
                var status = ((Enums.WorkShopRezervState)shopUser.obj.Status).GetDescriptionInLocalization();
                var workShopSubject = workShop.Subject;
                string sms = string.Format(shopUser.SmsBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender, workShopSubject, status);
                string email = string.Format(shopUser.EmailBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender, workShopSubject, homaCompleteUrl, status);
                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{WorkshopMessageKey.WorkshopName.ToString()}]", workShopSubject);
                        email = email.Replace($"[{WorkshopMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        email = email.Replace($"[{WorkshopMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        email = email.Replace($"[{WorkshopMessageKey.Status.ToString()}]", status);
                        email = email.Replace($"[{WorkshopMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);




                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{WorkshopMessageKey.WorkshopName.ToString()}]", workShopSubject);
                        sms = sms.Replace($"[{WorkshopMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        sms = sms.Replace($"[{WorkshopMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        sms = sms.Replace($"[{WorkshopMessageKey.Status.ToString()}]", status);
                        sms = sms.Replace($"[{WorkshopMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                    }

                }




                Message.Tools.ModelView.MessageModel inform = new Message.Tools.ModelView.MessageModel()
                {

                    Email = enterpriseNode.Email,
                    Mobile = enterpriseNode.Cellphone,
                    EmailTitle = homa.CongressTitle,
                    EmailBody = email,
                    SMSBody = sms

                };
                new HomaBO().SendInform((byte)config.UserWorkReserveShopInformType, inform, config, titlehoma);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa.OwnerId, config.CongressId,
              new[] { enterpriseNode.Id.ToString() }, homa.CongressTitle, inform.SMSBody);
            }
        }



        public decimal GetTransactionId(IConnectionHandler connectionHandler, Guid congressId, string year = "", string moth = "")
        {

            PredicateBuilder<WorkShopUser> predicateBuilder = new PredicateBuilder<WorkShopUser>();
            predicateBuilder.And(x => x.WorkShop.CongressId == congressId && x.TransactionId.HasValue && x.Transaction.Done);
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

        public ModelView.ModifyResult<WorkShopUser> UpdateStatusAfterTransaction(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, Guid userId, Guid tempId)
        {
            
            WorkShopUserBO workShopUserBo = new WorkShopUserBO();
            var result = new ModelView.ModifyResult<WorkShopUser>();
            List<WorkShopUser> shopUser = workShopUserBo.Where(connectionHandler, x => x.TempId == tempId);
            if (!shopUser.Any())
            {
                return result;
            }

            Transaction tr = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection).RemoveTempAndReturnTransaction(tempId);
            if (tr == null)
            {
                return result;
            }

            foreach (WorkShopUser valuePair in shopUser)
            {
                valuePair.TransactionId = tr.Id;
                valuePair.TempId = null;
                if (tr.PreDone)
                {
                    valuePair.Status = (byte)Enums.WorkShopRezervState.Pay;
                }

                if (!workShopUserBo.Update(connectionHandler, valuePair))
                {
                    throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                }
            }
            WorkShopUser workshop = shopUser.FirstOrDefault();
            if (workshop != null)
            {
                if (tr.PreDone)
                {
                    result.AddInform(new WorkShopUser() { WorkShopId = workshop.WorkShopId, UserId = userId },
                        Resources.Congress.WorkShopPaymentEmail,
                        Resources.Congress.WorkShopPaymentSMS);
                   
                }
            }
            result.TransactionId = tr.Id;
            result.Result =true;
            result.SendInform =true;
           return result;

            
        }



    }
}
