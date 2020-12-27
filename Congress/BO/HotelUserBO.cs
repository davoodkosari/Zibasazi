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
    internal class HotelUserBO : BusinessBase<HotelUser>
    {
        public override bool Insert(IConnectionHandler connectionHandler, HotelUser obj)
        {
            if (obj.Status == 0)
            {
                obj.Status = (byte)Enums.RezervState.RegisterRequest;
            }

            obj.RegisterDate = DateTime.Now.ShamsiDate();
            return base.Insert(connectionHandler, obj);
        }
        public bool Delete(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, params object[] keys)
        {
            HotelUser obj = Get(connectionHandler, keys);
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            if (obj == null)
                return false;
            if (obj.TempId.HasValue) tempTransactionalFacade.Delete(obj.TempId);
            if (!base.Delete(connectionHandler, keys))
                throw new Exception(Resources.Congress.ErrorInDeleteHotel);

            return true;
        }
        public IEnumerable<HotelUser> Search(IConnectionHandler connectionHandler, Guid hotelId, byte? status, string registerDate, string txtSearch,
            FormStructure formStructure)
        {
            PredicateBuilder<HotelUser> predicateBuilder = new PredicateBuilder<HotelUser>();
            predicateBuilder.And(x => x.HotelId == hotelId);
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
        public IEnumerable<ModelView.ReportChartModel> CharHotelCountWithReserv(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<HotelUser, object>>[] { x => x.Hotel.Address },
                new GroupByModel<HotelUser>[]
                {
                    new GroupByModel<HotelUser>()
                    {
                        Expression = x => x.HotelId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.Hotel.CongressId == congressId);
            List<dynamic> allType = new HotelBO().Select(connectionHandler, new Expression<Func<Hotel, object>>[]
            {
                x => x.Address,
            }, x => x.CongressId == congressId);
            foreach (dynamic item in allType)
            {
                dynamic first = list.FirstOrDefault(x => (x.Address is string) && (string)x.Address == (string)item.Address);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountHotelId ?? 0,
                    Value = (string)item.Address
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> CharHotelCountWithStatus(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler,
                new Expression<Func<HotelUser, object>>[] { x => x.Status },
                new GroupByModel<HotelUser>[]
                {
                    new GroupByModel<HotelUser>()
                    {
                        Expression = x => x.Status,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.Hotel.CongressId == congressId);
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


        public KeyValuePair<bool, Guid> HotelUserInsert(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler formGeneratorConnection, Guid hotelId, User parentUser, List<DiscountType> discountAttaches, string callBackurl, FormGenerator.DataStructure.FormStructure formModel, int dayCount, List<Guid> userIdlist)
        {

            Guid? outvalue = null;
            Hotel hotel = new HotelBO().Get(connectionHandler, hotelId);
            int newlistCount = 0;
            List<Guid> validlist = new List<Guid>();
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            List<Guid> @select = Select(connectionHandler, x => x.UserId, x => x.HotelId == hotelId);
            CongressDiscountTypeBO congressDiscountTypeBo = new CongressDiscountTypeBO();
            string additionalData = congressDiscountTypeBo.FillTempAdditionalData(connectionHandler, hotel.CongressId);

            foreach (Guid guid1 in userIdlist)
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
            List<HotelUser> list = Where(connectionHandler, x => (x.UserId == parentUser.Id || x.User.ParentId == parentUser.Id) && x.HotelId == hotelId, true);
            foreach (HotelUser hotelUser in list)
            {
                if (validlist.Any(x => x.Equals(hotelUser.UserId)))
                    continue;

                if (hotelUser.Status != (byte)Enums.RezervState.RegisterRequest &&
                    hotelUser.Status != (byte)Enums.RezervState.DenialPay)
                    continue;

                if (hotelUser.TempId.HasValue) 
                    tempTransactionalFacade.Delete(hotelUser.TempId);
                if (!base.Delete(connectionHandler, hotelUser))
                    throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            if (hotel.FreeCapicity < newlistCount)
            {
                throw new Exception(Resources.Congress.HotelCapacityLessThanNumberOfSelectedUser);
            }

            if (hotel.ValidCost.ToDecimal() > 0 && validlist.Any())
            {
                outvalue = SelectFirstOrDefault(connectionHandler, x => x.TempId, x => x.UserId.In(validlist) && x.HotelId == hotelId);

                decimal calulateAmountNew = congressDiscountTypeBo.CalulateAmountNew(paymentConnection, ((hotel.ValidCost.ToDecimal()) * dayCount) * validlist.Count, discountAttaches);
                if (outvalue == null)
                {
                    outvalue = Guid.NewGuid();
                    Temp temp = new Temp
                    {
                        Id = (Guid)outvalue,
                        PayerId = parentUser.Id,
                        CallBackUrl = callBackurl + outvalue,
                        Description = Resources.Congress.PaymentHotelReserv + " " + hotel.Name,
                        PayerTitle = parentUser.DescriptionField,
                        Amount = calulateAmountNew,
                        CurrencyType = (byte)hotel.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
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
                    tr.CurrencyType = (byte)hotel.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    tr.AdditionalData = additionalData;
                    if (!tempTransactionalFacade.Update(tr, discountAttaches))
                    {
                        return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    }
                }
            }

            FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
            List<HotelUser> @selectd = Where(connectionHandler, x => x.HotelId == hotelId && x.UserId.In(validlist), true);
            foreach (Guid id in validlist)
            {
                HotelUser hotelUser = @selectd.FirstOrDefault(x => x.UserId == id);
                if (hotelUser == null)
                {
                    HotelUser hotelUser1 = new HotelUser() { UserId = id, HotelId = hotelId, DaysCount = dayCount };
                    if (outvalue != null)
                    {
                        hotelUser1.TempId = outvalue;
                    }
                    else
                    {
                        hotelUser1.TempId = null;
                        hotelUser1.Status = (byte)Enums.RezervState.PayConfirm;
                    }
                    if (!Insert(connectionHandler, hotelUser1))
                    {
                        throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
                    }
                }
                else
                {
                    if (outvalue != null)
                    {
                        hotelUser.TempId = outvalue;
                    }
                    else
                    {
                        if (hotelUser.TempId.HasValue) 
                            tempTransactionalFacade.Delete(hotelUser.TempId);
                        hotelUser.TempId = null;
                        hotelUser.Status = (byte)Enums.RezervState.PayConfirm;
                       
                    }
                    hotelUser.DaysCount = dayCount;
                    if (!Update(connectionHandler, hotelUser))
                    {
                        throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
                    }
                }
                formModel.RefId = id + "," + hotelId;
                if (!formDataTransactionalFacade.ModifyFormData(formModel))
                {
                    throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
                }
            }

            return new KeyValuePair<bool, Guid>(true, outvalue != null ? (Guid)outvalue : Guid.Empty);


        }


        public bool HotelUserInsert(IConnectionHandler connectionHandler,  IConnectionHandler formGeneratorConnection,HotelUser hotelUser  , FormGenerator.DataStructure.FormStructure formModel)
        {

           
           FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
           if (!Insert(connectionHandler, hotelUser))
           {
               throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
           }
            formModel.RefId = hotelUser.UserId + "," + hotelUser.HotelId;
            if (!formDataTransactionalFacade.ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            return true;


        }
        public bool HotelUserUpdate(IConnectionHandler connectionHandler, IConnectionHandler formGeneratorConnection, HotelUser hotelUser, FormGenerator.DataStructure.FormStructure formModel)
        {

           
            FormGenerator.Facade.Interface.IFormDataFacade formDataTransactionalFacade = FormGeneratorComponent.Instance.FormDataTransactionalFacade(formGeneratorConnection);
            if (!Update(connectionHandler, hotelUser))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            formModel.RefId = hotelUser.UserId + "," + hotelUser.HotelId;
            if (!formDataTransactionalFacade.ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveHotelReserv);
            }
            return true;


        }

        public void InformHotelReserv(IConnectionHandler connectionHandler, Guid congressId, ModelView.InFormEntitiyList<HotelUser> hotelUser)
        {

            if (!hotelUser.Any()) return;
            Homa homa = new HomaBO().Get(connectionHandler, congressId);
            Configuration config = homa.Configuration;
            if (config.UserHotelReserveInformType == null) return;
            string titlehoma = homa.CongressTitle;
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Type == MessageInformType.Hotel);
            var @where = this.Where(connectionHandler,
                x => x.HotelId.In(hotelUser.Select(i => i.obj.HotelId)) &&
                     x.UserId.In(hotelUser.Select(i => i.obj.UserId)));
            foreach (var hotelUser1 in hotelUser)
            {

                var firstOrDefault = @where.FirstOrDefault(x => x.HotelId == hotelUser1.obj.HotelId && x.UserId == hotelUser1.obj.UserId);
                if (firstOrDefault == null) continue;
                var enterpriseNode = firstOrDefault.User.EnterpriseNode;
                var hotel = firstOrDefault.Hotel;
                var status = ((Enums.RezervState)hotelUser1.obj.Status).GetDescriptionInLocalization();
                var homaCompleteUrl = homa.GetHomaCompleteUrl();
                string sms = string.Format(hotelUser1.SmsBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender, hotel.Name, status);
                string email = string.Format(hotelUser1.EmailBody, homa.CongressTitle, enterpriseNode.DescriptionFieldWithGender, hotel.Name, homaCompleteUrl, status);
                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{HotelMessageKey.HotelName.ToString()}]", hotel.Name);
                        email = email.Replace($"[{HotelMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        email = email.Replace($"[{HotelMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        email = email.Replace($"[{HotelMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                        email = email.Replace($"[{HotelMessageKey.Status.ToString()}]", status);
                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.SmsText.Replace($"[{HotelMessageKey.HotelName.ToString()}]", hotel.Name);
                        sms = sms.Replace($"[{HotelMessageKey.UsersName.ToString()}]", enterpriseNode.DescriptionFieldWithGender);
                        sms = sms.Replace($"[{HotelMessageKey.CongressTitle.ToString()}]", homa.CongressTitle);
                        sms = sms.Replace($"[{HotelMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                        sms = sms.Replace($"[{HotelMessageKey.Status.ToString()}]", status);
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
                new HomaBO().SendInform((byte)config.UserHotelReserveInformType, inform, config, titlehoma);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa.OwnerId, config.CongressId,
                new[] { enterpriseNode.Id.ToString() }, homa.CongressTitle, inform.SMSBody);
            }



        }




        public decimal GetTransactionId(IConnectionHandler connectionHandler, Guid congressId, string year = "", string moth = "")
        {
            PredicateBuilder<HotelUser> predicateBuilder = new PredicateBuilder<HotelUser>();
            predicateBuilder.And(x => x.Hotel.CongressId == congressId && x.TransactionId.HasValue && x.Transaction.Done);
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

        public ModelView.ModifyResult<HotelUser> UpdateStatusAfterTransaction(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, Guid userId, Guid tempId)
        {
            HotelUserBO hotelUserBo = new HotelUserBO();
            var result = new ModelView.ModifyResult<HotelUser>();


            bool informUser = false;
            List<HotelUser> shopUser = hotelUserBo.Where(connectionHandler, x => x.TempId == tempId);
            if (!shopUser.Any())
            {
                return result;
            }

            Transaction tr = PaymentComponenets.Instance.TempTransactionalFacade(connectionHandler).RemoveTempAndReturnTransaction(tempId);
            if (tr == null)
            {
                return result;
            }

            foreach (HotelUser hotelUser in shopUser)
            {
                hotelUser.TransactionId = tr.Id;
                hotelUser.TempId = null;
                if (tr.PreDone)
                {
                    hotelUser.Status = (byte)Enums.RezervState.Pay;
                }

                if (!hotelUserBo.Update(connectionHandler, hotelUser))
                {
                    throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                }
            }
            HotelUser hotel = shopUser.FirstOrDefault();
            if (hotel != null)
            {
                if (tr.PreDone)
                {
                    result.AddInform(new HotelUser() { HotelId = hotel.HotelId, UserId = userId }, Resources.Congress.HotelPaymentEmail, Resources.Congress.HotelPaymentSMS);
                    result.SendInform = true;
                }
            }
            result.TransactionId = tr.Id;
            return result;

        }



    }
}
