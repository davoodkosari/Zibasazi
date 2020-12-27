using Excel;
using Radyn.Congress.DA;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.FileManager;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Message;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Reservation;
using Radyn.Utility;
using Radyn.XmlModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml;
using static Radyn.Congress.Tools.Enums;
using Serialize = Radyn.XmlModel.Serialize;
using User = Radyn.Congress.DataStructure.User;

namespace Radyn.Congress.BO
{
    internal class UserBO : BusinessBase<User>
    {

        #region Cards

        public List<ModelView.UserCardModel> GetUserCard(IConnectionHandler connectionHandler, User user, ConfigurationContent configcontent,
          Homa homa, IEnumerable<ArticleType> articleTypes, List<Article> articles, bool isadmin)
        {
            List<ModelView.UserCardModel> list = new List<ModelView.UserCardModel>();
            if (isadmin || homa.Configuration.ShoudBeUserPaymentApprovedForUserCardPrint == false)
            {
                AddRegisterCard(connectionHandler, user, configcontent, homa, ref list);
                foreach (ArticleType articleType in articleTypes)
                {
                    Article byFilter = articles.FirstOrDefault(x =>

                        x.TypeId == articleType.Id &&
                        x.UserId == user.Id
                    );
                    if (byFilter != null)
                    {
                        AddArticleCard(user, configcontent, homa, articleType, byFilter, ref list);
                    }
                }
            }

            else
            {
                if (user.PaymentTypeId.HasValue &&
                    (user.Status == (byte)Enums.UserStatus.PayConfirm ||
                     user.Status == (byte)Enums.UserStatus.ConfirmPresentInHoma))
                {
                    AddRegisterCard(connectionHandler, user, configcontent, homa, ref list);
                }

                foreach (ArticleType articleType in articleTypes)
                {
                    Article article = articles.FirstOrDefault(x => x.TypeId == articleType.Id &&
                        x.FinalState == (byte)Enums.FinalState.Confirm &&
                        x.UserId == user.Id
                    );
                    if (article != null)
                    {
                        AddArticleCard(user, configcontent, homa, articleType, article, ref list);
                    }
                }

            }
            return list;
        }

        public List<ModelView.UserCardModel> GetChipFootUser(IConnectionHandler connectionHandler, User user, ConfigurationContent configcontent,
        Homa homa, IEnumerable<ChipsFood> chipsFoods)
        {
            List<ModelView.UserCardModel> list = new List<ModelView.UserCardModel>();
            foreach (ChipsFood food in chipsFoods)
            {
                ModelView.UserCardModel cardModel = new ModelView.UserCardModel();
                SetInformation(user, configcontent, homa, ref cardModel);

                cardModel.Type = food.Name;
                cardModel.CardId = Enums.CardType.ChipFood + "-" + food.Id;
                cardModel.CardType = Enums.CardType.ChipFood;
                cardModel.Description = food.Description;
                cardModel.ChipFoodDays = food.DaysInfo;
                list.Add(cardModel);

            }
            return list;
        }

        public DataTable ReportFormDataForExcel(IConnectionHandler connectionHandler, string url, List<object> obj, string objectName, string[] refcolumnNames)
        {
            try
            {
                DataTable model = FormGeneratorComponent.Instance.FormDataTransactionalFacade(connectionHandler).ReportFormData(url, obj, objectName, refcolumnNames);

                //حذف کردن ستون های اضافی
                if (model.Columns.Contains("FbTokenId"))
                {
                    model.Columns.Remove("FbTokenId");
                }
                if (model.Columns.Contains("Password"))
                {
                    model.Columns.Remove("Password");
                }
                if (model.Columns.Contains("StatusNullable"))
                {
                    model.Columns.Remove("StatusNullable");
                }
                if (model.Columns.Contains("PaymentTypeDaysInfo"))
                {
                    model.Columns.Remove("PaymentTypeDaysInfo");
                }
                if (model.Columns.Contains("Status"))
                {
                    model.Columns.Remove("Status");
                }
                if (model.Columns.Contains("ReservedItem"))
                {
                    model.Columns.Remove("ReservedItem");
                }
                if (model.Columns.Contains("HasChild"))
                {
                    model.Columns.Remove("HasChild");
                }
                if (model.Columns.Contains("CurrentUICultureName"))
                {
                    model.Columns.Remove("CurrentUICultureName");
                }
                if (model.Columns.Contains("RootId"))
                {
                    model.Columns.Remove("RootId");
                }
                if (model.Columns.Contains("Done"))
                {
                    model.Columns.Remove("Done");
                }
                if (model.Columns.Contains("Culture"))
                {
                    model.Columns.Remove("Culture");
                }
                if (model.Columns.Contains("ChairTypeTitle"))
                {
                    model.Columns.Remove("ChairTypeTitle");
                }
                if (model.Columns.Contains("TitlePaymentType"))
                {
                    model.Columns.Remove("TitlePaymentType");
                }
                if (model.Columns.Contains("LastName"))
                {
                    model.Columns.Remove("LastName");
                }
                if (model.Columns.Contains("RegisterInNewsLetter"))
                {
                    model.Columns.Remove("RegisterInNewsLetter");
                }
                if (model.Columns.Contains("HasSuccedPayment"))
                {
                    model.Columns.Remove("HasSuccedPayment");
                }
                if (model.Columns.Contains("FirstName"))
                {
                    model.Columns.Remove("FirstName");
                }
                //model.Columns.Remove("FullName");
                //model.Columns.Remove("Comment");
                //model.Columns.Remove("DescriptionField");
                //model.Columns.Remove("HallName");

                return model;
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


        private void AddArticleCard(User user, ConfigurationContent configcontent, Homa homa, ArticleType articleType,
            Article article, ref List<ModelView.UserCardModel> list)
        {
            ModelView.UserCardModel model = new ModelView.UserCardModel();
            SetInformation(user, configcontent, homa, ref model);
            model.Type = articleType.Title;
            model.CardId = Enums.CardType.ArticleTypeCard + "-" + article.Id;
            model.CardType = Enums.CardType.ArticleTypeCard;
            list.Add(model);
        }

        private void AddRegisterCard(IConnectionHandler connectionHandler, User user,
            ConfigurationContent configcontent, Homa homa, ref List<ModelView.UserCardModel> list)
        {
            ModelView.UserCardModel userCardModel = new ModelView.UserCardModel();
            SetInformation(user, configcontent, homa, ref userCardModel);
            UserRegisterPaymentType paytype = user.PaymentType;
            if (paytype != null)
            {
                if (!string.IsNullOrEmpty(paytype.DaysInfo))
                {
                    userCardModel.RegisterDays = paytype.DaysInfo;
                }

                if (paytype.Printable)
                {
                    userCardModel.Type = paytype.Title;
                }

                userCardModel.Type = homa.Configuration.CartTypeEmptyValue;
            }
            userCardModel.CardId = Enums.CardType.RegisterCard + "-" + user.Id;
            userCardModel.CardType = Enums.CardType.RegisterCard;
            list.Add(userCardModel);

        }


        public void SetInformation(User user, ConfigurationContent configcontent, Homa homa, ref ModelView.UserCardModel userKartModel)
        {
            FileManager.Facade.Interface.IFileFacade fileFacade = FileManagerComponent.Instance.FileFacade;
            if (configcontent != null && configcontent.LogoId.HasValue)
            {
                if (configcontent.Logo != null)
                {
                    userKartModel.CongressLogo = configcontent.Logo.Content;
                }
            }

            EnterpriseNode.DataStructure.EnterpriseNode enterpriseNode = user.EnterpriseNode;
            if (enterpriseNode != null)
            {
                if (enterpriseNode.PictureId.HasValue)
                {
                    FileManager.DataStructure.File file = fileFacade.Get(enterpriseNode.PictureId);
                    if (file != null)
                    {
                        userKartModel.UserImage = file.Content;
                    }
                }
                if (enterpriseNode.PrefixTitleId.HasValue)
                {
                    userKartModel.Prefix = enterpriseNode.PrefixTitle.Title;
                }
                RealEnterpriseNode realEnterpriseNode = enterpriseNode.RealEnterpriseNode;
                if (realEnterpriseNode != null)
                {
                    userKartModel.FirstName = realEnterpriseNode.FirstName;
                    userKartModel.LastName = realEnterpriseNode.LastName;
                    userKartModel.NationalCode = realEnterpriseNode.NationalCode;
                }
            }
            if (user.ChairId.HasValue)
            {

                Reservation.DataStructure.Chair chair = user.Chair;
                if (chair != null)
                {
                    userKartModel.ChairNumber = chair.Number.ToString();
                    userKartModel.ChairRow = chair.Row.ToString();
                    userKartModel.ChairColumn = chair.Column.ToString();
                    if (chair.Hall != null)
                    {
                        userKartModel.ChairHall = chair.Hall.Name;
                        if (chair.Hall.ParentId.HasValue)
                        {
                            userKartModel.ChairHallParent = chair.Hall.ParentHall.Name;
                        }
                    }
                }
            }
            userKartModel.Id = user.Id.ToString();
            userKartModel.Number = user.Number;
            userKartModel.CongressTitle = homa.CongressTitle;
            userKartModel.UseName = user.Username;

        }

        #endregion
        public IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchText(IConnectionHandler connectionHandler, Guid congressId, string txtSearch, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser)
        {
            PredicateBuilder<User> predicateBuilder = new PredicateBuilder<User>();
            predicateBuilder.And(x => x.CongressId == congressId);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                txtSearch = txtSearch.ToLower();
                predicateBuilder.And((x => x.Username.Contains(txtSearch) || x.Number.ToString().Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.LastName.Contains(txtSearch)
                || x.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(txtSearch) || x.EnterpriseNode.Address.Contains(txtSearch)
                || x.EnterpriseNode.Website.Contains(txtSearch) || x.EnterpriseNode.Email.Contains(txtSearch) || x.EnterpriseNode.Tel.Contains(txtSearch)));


            }

            return Select(connectionHandler, x => x.EnterpriseNode, predicateBuilder.GetExpression());

        }
        public IEnumerable<ModelView.UserCardModel> SearchCards(IConnectionHandler connectionHandler, Guid congressId, string txtSearch, User user,
           Guid? articleTypeId, EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure)
        {
            List<ModelView.UserCardModel> outlist = new List<ModelView.UserCardModel>();
            PredicateBuilder<User> predicateBuilder = UserSerachPredicateBuilder(congressId, txtSearch, user, gender, formStructure);
            if (articleTypeId != null)
            {
                List<Guid> guids = new ArticleBO().Select(connectionHandler, x => x.UserId, x => x.TypeId == articleTypeId);
                if (guids.Any())
                {
                    predicateBuilder.And(x => x.Id.In(guids));
                }
            }
            List<User> search = Where(connectionHandler, predicateBuilder.GetExpression());
            if (!search.Any())
            {
                return outlist;
            }

            Homa homa = new HomaBO().Get(connectionHandler, congressId);
            ConfigurationContent configcontent = new ConfigurationContentBO().Get(connectionHandler, congressId, homa.Configuration.CardLanguageId);
            List<ArticleType> articleTypes = new ArticleTypeBO().Where(connectionHandler, x => x.CongressId == congressId);
            List<Article> articles = new ArticleBO().Where(connectionHandler, x => x.CongressId == homa.Id && x.UserId.In(search.Select(i => i.Id)), true);
            foreach (User item in search)
            {
                outlist.AddRange(GetUserCard(connectionHandler, item, configcontent, homa, articleTypes, articles, true));
            }
            return outlist;

        }
        public IEnumerable<User> Search(IConnectionHandler connectionHandler, Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender,
          FormStructure formStructure)
        {
            PredicateBuilder<User> predicateBuilder = UserSerachPredicateBuilder(congressId, txtSearch, user, gender, formStructure);

            switch (sortuser)
            {
                case Enums.SortAccordingToUser.RegisterDate:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                        ? OrderByDescending(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression())
                        : OrderBy(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression());


                case Enums.SortAccordingToUser.LName:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                      ? OrderByDescending(connectionHandler, x => x.EnterpriseNode.RealEnterpriseNode.LastName, predicateBuilder.GetExpression())
                      : OrderBy(connectionHandler, x => x.EnterpriseNode.RealEnterpriseNode.LastName, predicateBuilder.GetExpression());

                    break;
                case Enums.SortAccordingToUser.PayDate:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                      ? OrderByDescending(connectionHandler, x => x.Transaction.PayDate, predicateBuilder.GetExpression())
                      : OrderBy(connectionHandler, x => x.Transaction.PayDate, predicateBuilder.GetExpression());
                default:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                     ? OrderByDescending(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression())
                     : OrderBy(connectionHandler, x => x.RegisterDate, predicateBuilder.GetExpression());
            }
        }
        public List<dynamic> GetChildsDynamic(IConnectionHandler connectionHandler, Guid parentuserId)
        {

            Expression<Func<User, object>>[] selectcolumn = Selectcolumn();
            return Select(connectionHandler, selectcolumn, x => x.ParentId == parentuserId, new OrderByModel<User>() { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });
        }
        public List<dynamic> SearchDynamic(IConnectionHandler connectionHandler, Guid congressId, string txtSearch, User user, Enums.AscendingDescending ascendingDescending, Enums.SortAccordingToUser sortuser, EnterpriseNode.Tools.Enums.Gender gender,
         FormStructure formStructure)
        {
            PredicateBuilder<User> predicateBuilder = UserSerachPredicateBuilder(congressId, txtSearch, user, gender, formStructure);
            Expression<Func<User, object>>[] selectcolumn = Selectcolumn();
            switch (sortuser)
            {
                case Enums.SortAccordingToUser.RegisterDate:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                        ? Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.RegisterDate, OrderType = OrderType.DESC })
                        : Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                case Enums.SortAccordingToUser.LName:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                      ? Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.EnterpriseNode.RealEnterpriseNode.LastName, OrderType = OrderType.DESC })
                      : Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.EnterpriseNode.RealEnterpriseNode.LastName, OrderType = OrderType.DESC });

                    break;
                case Enums.SortAccordingToUser.PayDate:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                      ? Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.Transaction.PayDate, OrderType = OrderType.DESC })
                      : Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.Transaction.PayDate, OrderType = OrderType.DESC });
                default:
                    return ascendingDescending == Enums.AscendingDescending.Descending
                     ? Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.RegisterDate, OrderType = OrderType.DESC })
                     : Select(connectionHandler, selectcolumn, predicateBuilder.GetExpression(), new OrderByModel<User>() { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });
            }
        }

        private static Expression<Func<User, object>>[] Selectcolumn()
        {
            Expression<Func<User, object>>[] selectcolumn = new Expression<Func<User, object>>[]
            {
                x => x.Id,
                x => x.RegisterDate,
                x => x.EnterpriseNode.RealEnterpriseNode.FirstName + " " + x.EnterpriseNode.RealEnterpriseNode.LastName,
                x => x.EnterpriseNode.Email,
                x => x.Status,
                x => x.Username,
                x => x.ChairId,
                x => x.Chair.HallId,
                x => x.PaymentType.Title,
                x => x.PaymentTypeDaysInfo,
                x => x.TransactionId,
            };
            return selectcolumn;
        }

        private PredicateBuilder<User> UserSerachPredicateBuilder(Guid congressId, string txtSearch, User user, EnterpriseNode.Tools.Enums.Gender gender,
            FormStructure formStructure)
        {
            PredicateBuilder<User> predicateBuilder = new PredicateBuilder<User>();
            predicateBuilder.And(x => x.CongressId == congressId);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                predicateBuilder.And((x => x.Username.Contains(txtSearch) || x.Number.ToString().Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.LastName.Contains(txtSearch)
                || x.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(txtSearch) || x.EnterpriseNode.Address.Contains(txtSearch)
                || x.EnterpriseNode.Website.Contains(txtSearch) || x.EnterpriseNode.Email.Contains(txtSearch) || x.EnterpriseNode.Tel.Contains(txtSearch)));

            }
            if (gender != EnterpriseNode.Tools.Enums.Gender.None)
            {
                bool? genderw = (gender == EnterpriseNode.Tools.Enums.Gender.Man);
                predicateBuilder.And(x => x.EnterpriseNode.RealEnterpriseNode.Gender == genderw);
            }
            if (formStructure != null)
            {
                IEnumerable<string> @where = FormGeneratorComponent.Instance.FormDataFacade.Search(formStructure);
                if (@where.Any())
                {
                    predicateBuilder.And(x => x.Id.In(@where.Select(s => s.ToGuid())));
                }
            }


            if (user != null)
            {
                if (user.PaymentTypeId != null)
                {
                    predicateBuilder.And(x => x.PaymentTypeId == user.PaymentTypeId);
                }

                if (user.ParentId != null)
                {
                    predicateBuilder.And(x => x.ParentId == user.ParentId);
                }

                if (user.StatusNullable != null)
                {
                    predicateBuilder.And(x => x.Status == user.StatusNullable);
                }

                if (!string.IsNullOrEmpty(user.RegisterDate))
                {
                    predicateBuilder.And(x => x.RegisterDate == user.RegisterDate);
                }

                if (user.UserChairStatus != Enums.UserChairStatus.None)
                {
                    if (user.UserChairStatus == Enums.UserChairStatus.HasChair)
                    {
                        predicateBuilder.And(x => x.ChairId.HasValue);
                    }
                    else
                    {
                        predicateBuilder.And(x => !x.ChairId.HasValue);
                    }
                }
            }
            return predicateBuilder;
        }


        public bool SelectChair(IConnectionHandler connectionHandler, IConnectionHandler reservationconnectionHandler, User user)
        {
            Reservation.Facade.Interface.IChairFacade chairTransactionalFacade = ReservationComponent.Instance.ChairTransactionalFacade(reservationconnectionHandler);

            if (user.ChairId.HasValue)
            {
                Reservation.DataStructure.Chair chair = chairTransactionalFacade.Get(user.ChairId);
                if (chair.Status == (byte)Reservation.Definition.Enums.ReservStatus.Free)
                {
                    if (user.Status == (byte)Enums.UserStatus.PayConfirm)
                    {
                        if (!chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)user.ChairId, Reservation.Definition.Enums.ReservStatus.Saled, user.Id))
                        {
                            throw new Exception("خطایی درانتخاب صندلی وجود دارد");
                        }
                    }
                    else
                    {
                        if (!chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)user.ChairId, Reservation.Definition.Enums.ReservStatus.Reserved, user.Id))
                        {
                            throw new Exception("خطایی درانتخاب صندلی وجود دارد");
                        }
                    }
                }
            }
            User olduser = Get(connectionHandler, user.Id);
            if (olduser.ChairId.HasValue && user.ChairId != olduser.ChairId)
            {
                if (!chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)olduser.ChairId, Reservation.Definition.Enums.ReservStatus.Free))
                {
                    throw new Exception("خطایی درانتخاب صندلی وجود دارد");
                }
            }

            if (!Update(connectionHandler, user))
            {
                return false;
            }

            return true;

        }



        public ModelView.ModifyResult<User> UserAttendance(IConnectionHandler connectionHandler, Guid congressId, long number)
        {
          var modifyResult = new ModelView.ModifyResult<User>();
            User user = new UserBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Number == number);
            if (user == null)
            {
                throw new Exception(Resources.Congress.UserNotFound);
            }

            bool isnew = user.Status != (byte)Enums.UserStatus.ConfirmPresentInHoma;
            if (isnew)
            {
                user.Status = (byte)Enums.UserStatus.ConfirmPresentInHoma;
                if (!Update(connectionHandler, user))
                {
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                }

                user.State = Framework.ObjectState.New;
            }
            
            if (isnew)
                modifyResult.AddInform(user,Resources.Congress.UserChangeStatusEmail , Resources.Congress.UserChangeStatusSMS);

            
            modifyResult.Result = true;
            modifyResult.SendInform = true;
            modifyResult.RefObject = user;
            return modifyResult;
        }
        public ModelView.ModifyResult<User> UpdateList(IConnectionHandler connectionHandler, IConnectionHandler paymentconnectionHandler, IConnectionHandler reservationconnectionHandler, List<User> userlist)
        {
          
            Payment.Facade.Interface.ITransactionFacade transactionTransactionalFacade = PaymentComponenets.Instance.TransactionTransactionalFacade(paymentconnectionHandler);
            Reservation.Facade.Interface.IChairFacade chairTransactionalFacade = ReservationComponent.Instance.ChairTransactionalFacade(reservationconnectionHandler);
            var modifyWithInform = new ModelView.ModifyResult<User>();
            List<User> @where = Where(connectionHandler, x => x.Id.In(userlist.Select(i => i.Id)));
            foreach (User user in userlist)
            {

                User user1 = @where.FirstOrDefault(x => x.Id == user.Id);
                if (user1 == null)
                {
                    continue;
                }

                Guid? chairId = user1.ChairId;
                user1.Status = user.Status;
                user1.ChairId = user.ChairId;
                if (user1.Status == (byte)Enums.UserStatus.PayConfirm)
                {
                    if (user1.TransactionId != null)
                    {
                        if (!transactionTransactionalFacade.Done((Guid)user1.TransactionId))
                        {
                            throw new Exception("خطایی در ثبت تراکنش وجود دارد");
                        }
                    }
                }
                if (user1.ChairId != null)
                {
                    if (
                        !chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)user1.ChairId,
                            (user1.Status == (byte)Enums.UserStatus.PayConfirm)
                                ? Reservation.Definition.Enums.ReservStatus.Saled
                                : Reservation.Definition.Enums.ReservStatus.Reserved, user.Id))
                    {
                        throw new Exception("خطایی درانتخاب صندلی وجود دارد");
                    }
                }

                if (chairId.HasValue && user1.ChairId != chairId)
                {
                    if (
                        !chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)chairId,
                            Reservation.Definition.Enums.ReservStatus.Free))
                    {
                        throw new Exception("خطایی در ثبت تراکنش وجود دارد");
                    }
                }
                if (!Update(connectionHandler, user1))
                {
                    throw new Exception("خطایی در ویرایش  کاربر وجود دارد");
                }

                if (modifyWithInform.InformList.All(x => x.obj.Id != user1.Id))
                {
                    modifyWithInform.AddInform(user1, Resources.Congress.UserChangeStatusEmail, Resources.Congress.UserChangeStatusSMS);
                  
                }

                if (!user1.ParentId.HasValue || modifyWithInform.InformList.Any(x => x.obj.Id == user1.ParentId))
                {
                    continue;
                }

                User key = Get(connectionHandler, user1.ParentId);
                if (key != null)
                {
                    modifyWithInform.AddInform(key, Resources.Congress.UserChangeStatusEmail, Resources.Congress.UserChangeStatusSMS);
                }
            }
            modifyWithInform.Result = true;
            modifyWithInform.SendInform = true;
            return modifyWithInform;
        }

        public bool Delete(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, IConnectionHandler reservationconnectionHandler, params object[] keys)
        {
            User obj = Get(connectionHandler, keys);
            bool byFilter = new ArticleBO().Any(connectionHandler, article => article.UserId == obj.Id);
            if (byFilter)
            {
                throw new Exception("این کاربر دارای مقاله/ایده/اثر است آن را نمیتوانید حذف کنید");
            }

            bool users = Any(connectionHandler, article => article.ParentId == obj.Id);
            if (users)
            {
                throw new Exception("این کاربر دارای اشخاص زیر دستی است آن را نمیتوانید حذف کنید");
            }

            bool shopUsers = new WorkShopUserBO().Any(connectionHandler, article => article.UserId == obj.Id);
            if (shopUsers)
            {
                throw new Exception("این کاربر دارای رزرو کارگاه است آن را نمیتوانید حذف کنید");
            }

            bool hotelUsers = new HotelUserBO().Any(connectionHandler, article => article.UserId == obj.Id);
            if (hotelUsers)
            {
                throw new Exception("این کاربر دارای رزرو هتل است است آن را نمیتوانید حذف کنید");
            }

            bool chipsFoodUsers = new ChipsFoodUserBO().Any(connectionHandler, article => article.UserId == obj.Id);
            if (chipsFoodUsers)
            {
                throw new Exception("این کاربر دارای رزرو زتون است است آن را نمیتوانید حذف کنید");
            }

            bool userBooths = new UserBoothBO().Any(connectionHandler, article => article.UserId == obj.Id);
            if (userBooths)
            {
                throw new Exception("این کاربر دارای رزرو غرفه است است آن را نمیتوانید حذف کنید");
            }

            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            if (obj == null)
            {
                return true;
            }

            if (obj.TempId.HasValue)
            {
                if (!tempTransactionalFacade.Delete(obj.TempId))
                {
                    throw new Exception(Resources.Congress.ErrorInDeleteUser);
                }
            }
            if (obj.ChairId.HasValue)
            {
                Reservation.Facade.Interface.IChairFacade chairTransactionalFacade = ReservationComponent.Instance.ChairTransactionalFacade(reservationconnectionHandler);
                if (
                    !chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)obj.ChairId,
                        Reservation.Definition.Enums.ReservStatus.Free))
                {
                    return false;
                }
            }
            if (!base.Delete(connectionHandler, keys))
            {
                throw new Exception(Resources.Congress.ErrorInDeleteUser);
            }

            return true;
        }


        public ModelView.ModifyResult<User> UpdateStatusAfterTransaction(IConnectionHandler connectionHandler, IConnectionHandler paymentconnection, IConnectionHandler reservationconnectionHandler, Guid userId, Guid tempId)
        {
            
            ModelView.ModifyResult<User> result = new ModelView.ModifyResult<User>();
            bool informUser = false;
            List<User> list = Where(connectionHandler, x => x.TempId == tempId);
            if (!list.Any())
            {
                return result;
            }

            Transaction tr = PaymentComponenets.Instance.TempTransactionalFacade(paymentconnection).RemoveTempAndReturnTransaction(tempId);
            if (tr == null)
            {
                return result;
            }
            //اگر پرداخت موفق نبوده باشد یا حضوری نباشد یوزر را اپدیت نمیکنیم
            if (tr.Done == false && tr.PayTypeId == 2)
            {
                return result;
            }

            Reservation.Facade.Interface.IChairFacade chairTransactionalFacade = ReservationComponent.Instance.ChairTransactionalFacade(reservationconnectionHandler);
            User firstOrDefault = list.FirstOrDefault();
            if (firstOrDefault != null)
            {

                if (firstOrDefault.PaymentTypeId == null)
                {
                    return result;
                }

                foreach (User user in list)
                {
                    user.TransactionId = tr.Id;
                    user.TempId = null;
                    bool Payed = false;
                    if (tr.PreDone)
                    {
                        user.Status = (byte)Enums.UserStatus.RegisterPay;
                        firstOrDefault.PaymentType.Capacity--;
                        Payed = true;
                        result.AddInform( user, Resources.Congress.UserPaymentEmail, Resources.Congress.UserPaymentSMS);

                    }
                    if (user.ChairId.HasValue)
                    {
                        if (Payed)
                        {
                            if (!chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)user.ChairId, Reservation.Definition.Enums.ReservStatus.Reserved, user.Id))
                            {
                                return result;
                            }

                            User olduser = Get(connectionHandler, user.Id);
                            if (olduser.ChairId.HasValue && user.ChairId != olduser.ChairId)
                            {
                                if (!chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)olduser.ChairId, Reservation.Definition.Enums.ReservStatus.Free))
                                {
                                    throw new Exception("خطایی درانتخاب صندلی وجود دارد");
                                }
                            }
                        }
                        else
                        {
                            if (
                                !chairTransactionalFacade.ChangeStatusAndSetOwner((Guid)user.ChairId,
                                    Reservation.Definition.Enums.ReservStatus.Free))
                            {
                                return result;
                            }

                            user.ChairId = null;
                        }
                    }
                    if (!Update(connectionHandler, user))
                    {
                        throw new Exception("خطایی در ویرایش  کاربر وجود دارد");
                    }
                }
                if (!new UserRegisterPaymentTypeBO().Update(connectionHandler, firstOrDefault.PaymentType))
                {
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                }

                
            }
            result.TransactionId = tr.Id;
            result.Result =true;
            result.SendInform =true;
            return result;
        }
        public KeyValuePair<bool, Guid> GroupPayment(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, Guid congressId, User parentUser, Guid paymentype, List<User> users, List<DiscountType> discountTypes, string callbackurl, Dictionary<int, decimal> paymentamounts)
        {
            int newlistCount = 0;
            CongressDiscountTypeBO congressDiscountTypeBo = new CongressDiscountTypeBO();
            string additionalData = congressDiscountTypeBo.FillTempAdditionalData(connectionHandler, congressId);
            List<User> validlist = new List<User>();
            List<User> ulist = new List<User>();
            if (users.Any())
            {
                ulist = Where(connectionHandler, x => x.Id.In(users.Select(i => i.Id)), true);
            }

            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            foreach (User user in ulist)
            {
                if (user.PaymentTypeId == null)
                {
                    newlistCount++;
                }

                user.ChairId = user.ChairId;
                validlist.Add(user);
            }
            UserRegisterPaymentTypeBO userRegisterPaymentTypeBo = new UserRegisterPaymentTypeBO();
            UserRegisterPaymentType userRegisterPaymentType = userRegisterPaymentTypeBo.Get(connectionHandler, paymentype);
            if (newlistCount > userRegisterPaymentType.Capacity)
            {
                throw new Exception(Resources.Congress.RegistertypeCapacityislessthanthenumberofusersisselected);
            }

            List<User> childs = new UserBO().Where(connectionHandler, x => x.ParentId == parentUser.Id && x.TransactionId == null && x.PaymentTypeId == null, true);
            foreach (User child in childs)
            {
                if (validlist.Any(x => x.Id == child.Id))
                {
                    continue;
                }

                if (child.TempId.HasValue)
                {
                    Temp temp = tempTransactionalFacade.Get(child.TempId);
                    if (temp != null)
                    {
                        if (!tempTransactionalFacade.Delete(child.TempId))
                        {
                            throw new Exception(Resources.Congress.ErrorInReservWorkShop);
                        }
                    }
                }
                child.PaymentTypeId = null;
                child.TempId = null;
                if (!Update(connectionHandler, child))
                {
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                }
            }
            Guid Id = Guid.Empty;
            GroupRegisterDiscountBO groupRegisterDiscountBo = new GroupRegisterDiscountBO();
            GroupRegisterDiscount groupDiscountAmount = groupRegisterDiscountBo.GetGroupDiscount(connectionHandler, congressId, validlist.Count);
            decimal amount = 0;
            decimal regiteramount = 0;
            string paymentTypes = "";
            foreach (KeyValuePair<int, decimal> keyValuePair in paymentamounts)
            {
                regiteramount += keyValuePair.Value;
                if (!string.IsNullOrEmpty(paymentTypes))
                {
                    paymentTypes += "-";
                }

                if (keyValuePair.Key > 0)
                {
                    paymentTypes += keyValuePair.Key;
                }
            }

            decimal amountwithcount = regiteramount * validlist.Count;
            if (groupDiscountAmount != null)
            {
                if (groupDiscountAmount.IsPrecent)
                {
                    amount = (amountwithcount * groupDiscountAmount.ValidAmount.ToDecimal()) / 100;
                }
                else
                {
                    amount = validlist.Count * groupDiscountAmount.ValidAmount.ToDecimal();
                }

                parentUser.GroupRegisterDiscountId = groupDiscountAmount.Id;
                if (!Update(connectionHandler, parentUser))
                {
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                }
            }

            decimal calulateAmountNew = congressDiscountTypeBo.CalulateAmountNew(connectionHandler, amountwithcount, discountTypes) - amount;
            if (regiteramount > 0)
            {
                foreach (User user in validlist)
                {
                    if (user.TempId == null)
                    {
                        continue;
                    }

                    Temp tr = tempTransactionalFacade.Get(Id);
                    if (tr == null)
                    {
                        continue;
                    }

                    Id = (Guid)user.TempId;
                    break;
                }

                if (Id == Guid.Empty)
                {
                    Id = Guid.NewGuid();
                    Temp temp = new Temp
                    {
                        Id = Id,
                        PayerId = parentUser.Id,
                        CallBackUrl = callbackurl + Id,
                        PayerTitle = parentUser.DescriptionField,
                        Description = Resources.Congress.PaymentUserRegister + " " + parentUser.FullName,
                        Amount = calulateAmountNew,
                        CurrencyType = (byte)userRegisterPaymentType.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                        AdditionalData = additionalData
                    };

                    if (!tempTransactionalFacade.Insert(temp, discountTypes))
                    {
                        return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    }
                }
                else
                {
                    Temp tr = tempTransactionalFacade.Get(Id);
                    tr.Amount = calulateAmountNew;
                    tr.AdditionalData = additionalData;
                    tr.CurrencyType =
                        (byte)userRegisterPaymentType.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>();
                    if (!tempTransactionalFacade.Update(tr, discountTypes))
                    {
                        return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    }
                }
            }
            foreach (User user in validlist)
            {
                user.PaymentTypeId = paymentype;
                user.PaymentTypeDaysInfo = paymentTypes;
                if (Id != Guid.Empty)
                {
                    user.TempId = Id;
                }
                else
                {
                    user.TempId = null;
                    user.Status = (byte)Enums.UserStatus.PayConfirm;
                    if (user.TempId.HasValue)
                    {
                        if (
                            !tempTransactionalFacade
                                .Delete(user.TempId))
                        {
                            return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                        }
                    }
                }
                if (!Update(connectionHandler, user))
                {
                    throw new Exception(Resources.Congress.ErrorInEditUser);
                }
            }
            return new KeyValuePair<bool, Guid>(true, Id);
        }
        public bool Paymnet(IConnectionHandler connectionHandler, IConnectionHandler paymentConnection, User user, List<DiscountType> discountAttaches, string callbackurl, Dictionary<int, decimal> paymentamounts)
        {

            UserRegisterPaymentType userRegisterPaymentType = new UserRegisterPaymentTypeBO().Get(connectionHandler,
                user.PaymentTypeId);
            decimal regiteramount = 0;
            string paymentTypes = "";
            string additionalData = new CongressDiscountTypeBO().FillTempAdditionalData(connectionHandler, user.CongressId);
            foreach (KeyValuePair<int, decimal> keyValuePair in paymentamounts)
            {
                regiteramount += keyValuePair.Value;
                if (!string.IsNullOrEmpty(paymentTypes))
                {
                    paymentTypes += "-";
                }

                if (keyValuePair.Key > 0)
                {
                    paymentTypes += keyValuePair.Key;
                }
            }
            decimal calulateAmountNew = new CongressDiscountTypeBO().CalulateAmountNew(paymentConnection,
                regiteramount, discountAttaches);
            Payment.Facade.Interface.ITempFacade tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(paymentConnection);
            if (regiteramount > 0)
            {
                Temp temp = new Temp
                {
                    PayerId = user.Id,
                    CallBackUrl = callbackurl,
                    PayerTitle = user.DescriptionField,
                    Description = Resources.Congress.PaymentUserRegister + " " + user.FullName,
                    Amount = calulateAmountNew,
                    CurrencyType =
                        (byte)
                            userRegisterPaymentType.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                    AdditionalData = additionalData,
                };

                if (
                    !tempTransactionalFacade
                        .Insert(temp, discountAttaches))
                {
                    return false;
                }

                user.TempId = temp.Id;
            }
            else
            {
                user.TempId = null;
                if (user.TempId.HasValue)
                {
                    if (
                        !tempTransactionalFacade
                            .Delete(user.TempId))
                    {
                        return false;
                    }
                }
            }
            user.PaymentTypeDaysInfo = paymentTypes;
            if (user.TempId == null)
            {
                user.Status = (byte)Enums.UserStatus.PayConfirm;
            }

            if (!Update(connectionHandler, user))
            {
                throw new Exception(Resources.Congress.ErrorInEditUser);
            }

            return true;


        }


        public IEnumerable<ModelView.ReportChartModel> ChartUserStatusCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler, new Expression<Func<User, object>>[] { x => x.Status },
                new[]
                {
                    new GroupByModel<User>()
                    {
                        Expression = x => x.Status,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    }
                }, x => x.CongressId == congressId);
            IEnumerable<KeyValuePair<byte, string>> enums = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.UserStatus>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.UserStatus>(),
                        keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in enums)
            {
                dynamic first = list.FirstOrDefault(x => (x.Status is byte) && (byte)x.Status == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountStatus ?? 0,
                    Value = ((Enums.UserStatus)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }

        public IEnumerable<ModelView.ReportChartModel> ChartUserMothCount(IConnectionHandler connectionHandler, Guid congressId, string year)
        {


            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler, new Expression<Func<User, object>>[] { x => x.RegisterDate.Substring(5, 2) },
                new[]
                {
                    new GroupByModel<User>()
                    {
                        Expression = x => x.RegisterDate.Substring(5,2),
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    }
                }, x => x.CongressId == congressId && x.RegisterDate.Substring(0, 4) == year);
            IEnumerable<KeyValuePair<byte, string>> months = EnumUtils.ConvertEnumToIEnumerableInLocalization<Common.Definition.Enums.PersianMonth>().Select(
                keyValuePair =>
                    new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Common.Definition.Enums.PersianMonth>(),
                        keyValuePair.Value));
            foreach (KeyValuePair<byte, string> item in months)
            {
                dynamic first = list.FirstOrDefault(x => (x.RegisterDate is string) && ((string)x.RegisterDate).ToByte() == item.Key);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountRegisterDate ?? 0,
                    Value = ((Common.Definition.Enums.PersianMonth)item.Key).GetDescriptionInLocalization()
                });
            }
            return listout;
        }
        public IEnumerable<ModelView.ReportChartModel> ChartUserDayCount(IConnectionHandler connectionHandler, Guid congressId, string moth, string year)
        {
            List<ModelView.ReportChartModel> listout = new List<ModelView.ReportChartModel>();
            List<dynamic> list = GroupBy(connectionHandler, new Expression<Func<User, object>>[] { x => x.RegisterDate.Substring(8, 2) },
                new[]
                {
                    new GroupByModel<User>()
                    {
                        Expression = x => x.RegisterDate.Substring(8,2),
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    }
                }, x => x.CongressId == congressId && x.RegisterDate.Substring(0, 4) == year && x.RegisterDate.Substring(5, 2) == moth);
            for (int x = 1; x <= (moth.CompareTo("06") <= 0 ? 31 : 30); x++)
            {
                string number = x > 10 ? x.ToString() : "0" + x;
                dynamic first = list.FirstOrDefault(i => (i.RegisterDate is string) && (string)i.RegisterDate == number);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountRegisterDate ?? 0,
                    Value = number
                });
            }
            return listout;
        }
        public IEnumerable<EnterpriseNode.DataStructure.EnterpriseNode> SearchUserWithInformType(IConnectionHandler connectionHandler, string informType, Guid congressId)
        {
            List<Guid> userlist = new List<Guid>();
            List<EnterpriseNode.DataStructure.EnterpriseNode> outlist = new List<EnterpriseNode.DataStructure.EnterpriseNode>();
            string[] id = informType.Split('/');

            if (id.Length == 2 && id[1].ToGuid() != Guid.Empty)
            {
                if (id[0] == typeof(Pivot).Name)
                {
                    userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                        x => x.PivotId == id[1].ToGuid() && x.CongressId == congressId, new OrderByModel<Article>()
                        {
                            Expression = x => x.User.RegisterDate,
                            OrderType = OrderType.DESC
                        });
                }

                if (id[0] == typeof(ArticleType).Name)
                {
                    userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                        x => x.TypeId == id[1].ToGuid() && x.CongressId == congressId, new OrderByModel<Article>()
                        {
                            Expression = x => x.User.RegisterDate,
                            OrderType = OrderType.DESC
                        });
                }
            }
            else
            {



                SentUserTypes sentUserTypes = informType.ToEnum<Enums.SentUserTypes>();
                switch (sentUserTypes)
                {
                    case Enums.SentUserTypes.SendForAllMan:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.EnterpriseNode.RealEnterpriseNode.Gender == true,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForAllUser:
                        userlist = Select(connectionHandler, x => x.Id, x => x.CongressId == congressId,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });

                        break;
                    case Enums.SentUserTypes.SendForAllWomen:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.EnterpriseNode.RealEnterpriseNode.Gender == false,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForUserAbstarctConfirm:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId &&
                                 x.FinalState == (byte)Enums.FinalState.AbstractConfirm,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserAbstarctConfirmAndNotSendOrginal:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId &&
                                 x.FinalState == (byte)Enums.FinalState.AbstractConfirm && x.OrginalFileId == null,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserArticleDenial:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId && x.FinalState == (byte)Enums.FinalState.Denial,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendToMembersOfTheAbstractsWillBeConfirmedAfterCorrection:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId &&
                                 x.FinalState == (byte)Enums.FinalState.ConfirmandEdit,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserOrginalConfirm:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId && x.FinalState == (byte)Enums.FinalState.Confirm,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SentForUserSendAbstractArticle:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.ArticleState.AbstractSended,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SentForUserSendOrginalArticle:
                        userlist = new ArticleBO().Select(connectionHandler, x => x.UserId,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.ArticleState.OrginalSended,
                            new OrderByModel<Article>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoReservedHotelButNotPay:
                        userlist = new HotelUserBO().Select(connectionHandler, x => x.UserId,
                            x => x.User.CongressId == congressId &&
                                 x.Status == (byte)Enums.RezervState.RegisterRequest,
                            new OrderByModel<HotelUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoReservedHotelConfirm:
                        userlist = new HotelUserBO().Select(connectionHandler, x => x.UserId,
                            x => x.User.CongressId == congressId &&
                                 x.Status == (byte)Enums.RezervState.Finalconfirm,
                            new OrderByModel<HotelUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoReservedHotelDenial:
                        userlist = new HotelUserBO().Select(connectionHandler, x => x.UserId,
                            x => x.User.CongressId == congressId && x.Status == (byte)Enums.RezervState.Denial,
                            new OrderByModel<HotelUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoReservedWorkShopButNotPay:
                        userlist = new WorkShopUserBO().Select(connectionHandler, x => x.UserId,
                            x =>
                                x.User.CongressId == congressId &&
                                x.Status == (byte)Enums.WorkShopRezervState.RegisterRequest,
                            new OrderByModel<WorkShopUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoReservedWorkShopConfirm:
                        userlist = new WorkShopUserBO().Select(connectionHandler, x => x.UserId,
                            x =>
                                x.User.CongressId == congressId &&
                                x.Status == (byte)Enums.WorkShopRezervState.Finalconfirm,
                            new OrderByModel<WorkShopUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;

                    case Enums.SentUserTypes.SendForUserWhoReservedWorkShopDenial:
                        userlist = new WorkShopUserBO().Select(connectionHandler, x => x.UserId,
                            x => x.User.CongressId == congressId &&
                                 x.Status == (byte)Enums.WorkShopRezervState.Denial,
                            new OrderByModel<WorkShopUser>()
                            {
                                Expression = x => x.User.RegisterDate,
                                OrderType = OrderType.DESC
                            });

                        break;
                    case Enums.SentUserTypes.SendForUserWhoConfirmInHoma:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId &&
                                 x.Status == (byte)Enums.UserStatus.ConfirmPresentInHoma);
                        break;
                    case Enums.SentUserTypes.SendForUserWhoRegisterAndnotPay:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.UserStatus.Register,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });

                        break;
                    case Enums.SentUserTypes.SendForAllUserInPreRegisterStatus:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.UserStatus.PreRegister,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForAllUserPayConfirmAndNotHaveChair:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.UserStatus.PreRegister &&
                                 x.ChairId == null,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForUserWhoRegisterPayConfirm:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.UserStatus.PayConfirm,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForUserWhoRegisterPayDenial:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.Status == (byte)Enums.UserStatus.PayDenial,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForUserWhoDenialInHoma:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId &&
                                 x.Status == (byte)Enums.UserStatus.DenialPresentInHoma,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });


                        break;
                    case Enums.SentUserTypes.SendForRefrereeHasWaitForAnswerArticle:
                        userlist = new RefereeCartableBO().Select(connectionHandler, x => x.RefereeId,
                            x => x.Referee.CongressId == congressId && x.Status == 0);
                        break;
                    case Enums.SentUserTypes.SendForRefrereeNotHasWaitForAnswerArticle:
                        userlist = new RefereeCartableBO().Select(connectionHandler, x => x.RefereeId,
                            x => x.Referee.CongressId == congressId && x.Status != 0);

                        break;
                    case Enums.SentUserTypes.SendForAllUserHaveReserveChair:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.ChairId.HasValue && x.Chair.Status !=
                                 (byte)Reservation.Definition.Enums.ReservStatus.Reserved,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });

                        break;
                    case Enums.SentUserTypes.SendForAllUserHaveSaledChair:
                        userlist = Select(connectionHandler, x => x.Id,
                            x => x.CongressId == congressId && x.ChairId.HasValue && x.Chair.Status !=
                                 (byte)Reservation.Definition.Enums.ReservStatus.Saled,
                            new OrderByModel<User>()
                            { Expression = x => x.RegisterDate, OrderType = OrderType.DESC });

                        break;
                }
            }


            if (!userlist.Any())
            {
                return outlist;
            }

            return EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Where(x => x.Id.In(userlist));


        }




        public Dictionary<User, List<string>> ImportFromXml(IConnectionHandler connectionHandler, HttpPostedFileBase file)
        {
            Dictionary<User, List<string>> keyValuePairs = new Dictionary<User, List<string>>();
            XmlDocument doc = new XmlDocument();
            doc.Load(file.InputStream);
            AttendanceXml deserialize = Serialize.XmlDeserialize<AttendanceXml>(doc.InnerXml);
            if (deserialize != null)
            {

                if (deserialize.CongressAbsentList == null)
                {
                    return keyValuePairs;
                }

                foreach (CongressAbsentXml congressAbsentXml in deserialize.CongressAbsentList)
                {
                    foreach (AbsentItemXml absentItem in congressAbsentXml.AbsentItem)
                    {
                        List<string> resultStatus = new List<string>();

                        User firstOrDefault = FirstOrDefault(connectionHandler, x => x.Number == absentItem.Value.ToLong() & x.CongressId == congressAbsentXml.CongressId);
                        if (firstOrDefault == null)
                        {
                            firstOrDefault = new User { Id = Guid.NewGuid(), Number = absentItem.Value.ToLong(), EnterpriseNode = new EnterpriseNode.DataStructure.EnterpriseNode() { RealEnterpriseNode = new RealEnterpriseNode() } };
                            resultStatus.Add(string.Format("کاربری با شماره {0} یافت نشد", absentItem.Value));

                        }
                        else
                        {
                            firstOrDefault.Status = (byte)Enums.UserStatus.ConfirmPresentInHoma;
                        }
                        keyValuePairs.Add(firstOrDefault, resultStatus);



                    }




                }


            }

            return keyValuePairs;


        }



        public Dictionary<User, List<string>> ImportFromExcel(IConnectionHandler connectionHandler, HttpPostedFileBase fileBase, Guid congressId, Guid? parentId)
        {
            try
            {
                Dictionary<User, List<string>> keyValuePairs = new Dictionary<User, List<string>>();
                if (fileBase == null)
                {
                    return keyValuePairs;
                }

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileBase.InputStream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                if (result == null)
                {
                    return keyValuePairs;
                }

                UserBO userBo = new UserBO();
                foreach (DataTable table in result.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        List<string> resultStatus = new List<string>();
                        User user = new User()
                        {
                            Id = Guid.NewGuid(),
                            CongressId = congressId,
                            Status = (byte)Enums.UserStatus.Register,
                            EnterpriseNode =
                                new EnterpriseNode.DataStructure.EnterpriseNode()
                                {
                                    RealEnterpriseNode = new RealEnterpriseNode()
                                }
                        };

                        if (string.IsNullOrEmpty(row[0].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.PleaseEnterYourEmail);
                        }
                        else
                        {
                            user.EnterpriseNode.Email = row[0].ToString();
                            user.Username = row[0].ToString();
                            if (!Utility.Utils.IsEmail(user.EnterpriseNode.Email))
                            {
                                resultStatus.Add(Resources.Congress.UnValid_Enter_Email);
                            }

                            User byUserName = userBo.FirstOrDefault(connectionHandler, x => x.EnterpriseNode.Email == user.EnterpriseNode.Email & x.CongressId == congressId);
                            if (byUserName != null)
                            {
                                resultStatus.Add(Resources.Congress.UserNameIsRepeate);
                                user = byUserName;
                                user.State = Framework.ObjectState.Dirty;
                                if (parentId != null && user.ParentId != parentId)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(user.Username))
                                {
                                    user.Password = StringUtils.HashPassword(user.Username.Substring(0, 5));
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourName);
                            user.EnterpriseNode.RealEnterpriseNode.FirstName = string.Empty;
                        }
                        else
                        {
                            user.EnterpriseNode.RealEnterpriseNode.FirstName = row[1].ToString();
                        }

                        if (string.IsNullOrEmpty(row[2].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourLastName);
                            user.EnterpriseNode.RealEnterpriseNode.LastName = string.Empty;
                        }
                        else
                        {
                            user.EnterpriseNode.RealEnterpriseNode.LastName = row[2].ToString();
                        }

                        if (string.IsNullOrEmpty(row[3].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourGender);
                            user.EnterpriseNode.RealEnterpriseNode.Gender = null;
                        }
                        else
                        {
                            switch (row[3].ToString().ToLower())
                            {
                                case "men":
                                case "مرد":
                                    user.EnterpriseNode.RealEnterpriseNode.Gender = true;
                                    break;
                                case "women":
                                case "زن":
                                    user.EnterpriseNode.RealEnterpriseNode.Gender = false;
                                    break;
                                default:
                                    user.EnterpriseNode.RealEnterpriseNode.Gender = null;
                                    break;
                            }
                        }
                        if (string.IsNullOrEmpty(row[4].ToString()))
                        {
                            resultStatus.Add(Resources.Congress.Please_Enter_YourMobile);
                            user.EnterpriseNode.Cellphone = string.Empty;
                        }
                        else
                        {
                            user.EnterpriseNode.Cellphone = row[4].ToString();
                        }

                        if (!string.IsNullOrEmpty(row[5].ToString()))
                        {
                            long NationalCode = row[5].ToString().ToLong();
                            string national = string.Format("{0:D10}", NationalCode);
                            if (!Radyn.Utility.Utils.ValidNationalID(national))
                            {
                                resultStatus.Add("کد ملی صحیح نمیباشد");
                            }
                            else
                            {
                                user.EnterpriseNode.RealEnterpriseNode.NationalCode = national;
                            }
                        }
                        else
                        {
                            user.EnterpriseNode.RealEnterpriseNode.NationalCode = string.Empty;
                        }

                        user.EnterpriseNode.Address = !string.IsNullOrEmpty(row[6].ToString()) ? row[6].ToString() : string.Empty;

                        keyValuePairs.Add(user, resultStatus);
                    }
                }
                return keyValuePairs;
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace); throw new KnownException(ex.Message, ex);
            }
        }

        protected override void CheckConstraint(IConnectionHandler connectionHandler, User item)
        {
            base.CheckConstraint(connectionHandler, item);
            if (item.EnterpriseNode != null)
            {
                if (item.EnterpriseNode.Email != null && !string.IsNullOrEmpty(item.EnterpriseNode.Email.Trim()) &&
                    Any(connectionHandler, x => x.EnterpriseNode.Email == item.EnterpriseNode.Email && x.CongressId == item.CongressId && x.Id != item.Id))
                {
                    throw new KnownException(string.Format("ایمیل {0} قبلا در سامانه ثبت شده است", item.EnterpriseNode.Email));
                }

                if (item.EnterpriseNode.RealEnterpriseNode != null && item.EnterpriseNode.RealEnterpriseNode.NationalCode != null && !string.IsNullOrEmpty(item.EnterpriseNode.RealEnterpriseNode.NationalCode.Trim()) &&
                    Any(connectionHandler, x => x.EnterpriseNode.RealEnterpriseNode.NationalCode == item.EnterpriseNode.RealEnterpriseNode.NationalCode && x.CongressId == item.CongressId && x.Id != item.Id))
                {
                    throw new KnownException(string.Format("کد ملی {0} قبلا در سامانه ثبت شده است", item.EnterpriseNode.RealEnterpriseNode.NationalCode));
                }
            }
            if (item.Username != null && !string.IsNullOrEmpty(item.Username.Trim()) &&
                 Any(connectionHandler, x => x.Username.ToLower() == item.Username.ToLower() && x.CongressId == item.CongressId && x.Id != item.Id))
            {
                throw new KnownException(string.Format("نام کاربری {0} قبلا در سامانه ثبت شده است", item.Username));
            }
        }

        public override bool Insert(IConnectionHandler connectionHandler, User user)
        {
            user.RegisterDate = DateTime.Now.ShamsiDate();
            if (!string.IsNullOrEmpty(user.Password)) user.Password = StringUtils.HashPassword(user.Password);
            return base.Insert(connectionHandler, user);

        }

        internal bool Insert(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, User user,  HttpPostedFileBase fileBase)
        {
            Guid id = user.Id;
            BOUtility.GetGuidForId(ref id);
            user.Id = id;
            user.EnterpriseNode.Id = user.Id;
            if (!EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeconnection).Insert(user.EnterpriseNode, fileBase))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }
            
            return this.Insert(connectionHandler1,user);
        }
        internal bool InsertGuestUser(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, User user, System.Web.HttpPostedFileBase fileBase)
        {
            Guid id = user.Id;
            BOUtility.GetGuidForId(ref id);
            user.Id = id;
            user.EnterpriseNode.Id = user.Id;
            if (!EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeconnection).Insert(user.EnterpriseNode, fileBase))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }
            user.Status = (byte)Enums.UserStatus.Guest;
            if (!this.Insert(connectionHandler1, user))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            return true;
        }

        public override bool Update(IConnectionHandler connectionHandler, User user)
        {

            var oldpass = new UserBO().SelectFirstOrDefault(connectionHandler, x => x.Password, x => x.Id == user.Id);
            user.Password =
                (user.Password != null && !string.IsNullOrEmpty(user.Password.Trim()) &&
                 (oldpass == null || (oldpass.ToLower() != user.Password.ToLower())))
                    ? StringUtils.HashPassword(user.Password)
                    : oldpass;

            return base.Update(connectionHandler, user);
        }

        internal bool Update(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, User user, System.Web.HttpPostedFileBase fileBase = null)
        {

            if (!EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(enterpriseNodeconnection).Update(user.EnterpriseNode, fileBase))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            if (!this.Update(connectionHandler1, user))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            return true;
        }



        internal bool Insert(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, IConnectionHandler formgeneratorconnection, User user,  FormGenerator.DataStructure.FormStructure formModel, System.Web.HttpPostedFileBase fileBase)
        {
            if (!Insert(connectionHandler1, enterpriseNodeconnection, user, fileBase))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            formModel.RefId = user.Id.ToString();
            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            return true;
        }
        internal bool Update(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, IConnectionHandler formgeneratorconnection, User user,  FormGenerator.DataStructure.FormStructure formModel, System.Web.HttpPostedFileBase fileBase)
        {
            if (!this.Update(connectionHandler1, enterpriseNodeconnection, user, fileBase))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            return true;
        }

        internal bool Update(IConnectionHandler connectionHandler1, IConnectionHandler enterpriseNodeconnection, IConnectionHandler formgeneratorconnection, User user, FormGenerator.DataStructure.FormStructure formModel)
        {
            if (!this.Update(connectionHandler1, enterpriseNodeconnection, user, null))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            if (!FormGeneratorComponent.Instance.FormDataTransactionalFacade(formgeneratorconnection).ModifyFormData(formModel))
            {
                throw new Exception(Resources.Congress.ErrorInSaveUser);
            }

            return true;
        }
        public void InformUserRegister(IConnectionHandler connectionHandler, Guid congressId, ModelView.InFormEntitiyList<User> valuePairs)
        {
            if (!valuePairs.Any())
            {
                return;
            }


            Homa homa1 = new HomaBO().Get(connectionHandler, congressId);
            Configuration config = homa1.Configuration;
            if (config.UserRegisterInformType == null) return;
            CustomMessage custommessage = new CustomMessageBO().FirstOrDefault(connectionHandler, x => x.CongressId == congressId && x.Type == MessageInformType.User);
            foreach (var  user in valuePairs)
            {
                var status = ((UserStatus)user.obj.Status).GetDescriptionInLocalization();
                var homaCompleteUrl = homa1.GetHomaCompleteUrl();
                 string sms = string.Format(user.SmsBody, homa1.CongressTitle, user.obj.EnterpriseNode.DescriptionFieldWithGender, status);
                string email = string.Format(user.EmailBody, homa1.CongressTitle, user.obj.EnterpriseNode.DescriptionFieldWithGender, homaCompleteUrl, status);
                if (custommessage != null)
                {

                    if (!string.IsNullOrEmpty(custommessage.EmailText))
                    {
                        email = custommessage.EmailText.Replace($"[{UserMessageKey.Username.ToString()}]", user.obj.Username);
                        email = email.Replace($"[{UserMessageKey.Email.ToString()}]", user.obj.Email);
                        email = email.Replace($"[{UserMessageKey.FullName.ToString()}]", user.obj.EnterpriseNode.DescriptionFieldWithGender);
                        email = email.Replace($"[{UserMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        email = email.Replace($"[{UserMessageKey.Status.ToString()}]", status);
                        email = email.Replace($"[{UserMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);


                    }
                    if (!string.IsNullOrEmpty(custommessage.SmsText))
                    {
                        sms = custommessage.EmailText.Replace($"[{UserMessageKey.Username.ToString()}]", user.obj.Username);
                        sms = sms.Replace($"[{UserMessageKey.Email.ToString()}]", user.obj.Email);
                        sms = sms.Replace($"[{UserMessageKey.FullName.ToString()}]", user.obj.EnterpriseNode.DescriptionFieldWithGender);
                        sms = sms.Replace($"[{UserMessageKey.CongressTitle.ToString()}]", homa1.CongressTitle);
                        sms = sms.Replace($"[{UserMessageKey.Status.ToString()}]", status);
                        sms = sms.Replace($"[{UserMessageKey.CongressAddress.ToString()}]", homaCompleteUrl);
                    }

                }


                Message.Tools.ModelView.MessageModel inform = new Message.Tools.ModelView.MessageModel()
                {

                    Email = user.obj.EnterpriseNode.Email,
                    Mobile = user.obj.EnterpriseNode.Cellphone,
                    EmailTitle = homa1.DescriptionField,
                    EmailBody = email,
                    SMSBody = sms

                };
                new HomaBO().SendInform((byte)config.UserRegisterInformType, inform, config, homa1.CongressTitle);
                MessageComponenet.SentInternalMessageInstance.MailBoxFacade.SendInternalMail(homa1.OwnerId, config.CongressId,
                    new[] { user.obj.Id.ToString() }, homa1.CongressTitle, inform.SMSBody);
            }
        }
        public decimal GetTransactionId(IConnectionHandler connectionHandler, Guid congressId, string year = "", string moth = "")
        {

            PredicateBuilder<User> predicateBuilder = new PredicateBuilder<User>();
            predicateBuilder.And(x => x.CongressId == congressId && x.TransactionId.HasValue && x.Transaction.Done);
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




        public IEnumerable<User> GetSimilarUser(IConnectionHandler connectionHandler, Guid congresId, string name, string lastname)
        {
            UserDA userDa = new UserDA(connectionHandler);
            if (string.IsNullOrEmpty(name.Trim()) && string.IsNullOrEmpty(lastname.Trim()))
            {
                return null;
            }

            return userDa.GetSimilarUser(congresId, name, lastname);
        }

        public IEnumerable<User> GetSimilarUser(IConnectionHandler connectionHandler, Guid congresId)
        {

            List<User> users = new List<User>();
            List<User> byFilter = Where(connectionHandler, x => x.CongressId == congresId);
            if (byFilter == null)
            {
                return users;
            }

            foreach (User sourceuser in byFilter)
            {

                if (sourceuser.EnterpriseNode == null || sourceuser.EnterpriseNode.RealEnterpriseNode == null)
                {
                    continue;
                }

                IEnumerable<User> list = GetSimilarUser(connectionHandler, congresId, sourceuser.EnterpriseNode.RealEnterpriseNode.FirstName, sourceuser.EnterpriseNode.RealEnterpriseNode.LastName);
                if (list == null)
                {
                    continue;
                }

                List<User> inneruser = new List<User>();
                foreach (User user in list)
                {
                    if (user == null || user.Id == sourceuser.Id)
                    {
                        continue;
                    }

                    inneruser.AddRange(new[] { sourceuser, user });
                }
                if (!inneruser.Any())
                {
                    continue;
                }

                IOrderedEnumerable<User> enumerable = inneruser.OrderByDescending(x => x.RegisterDate);
                foreach (User user in enumerable)
                {
                    if (users.Any(x => x.Id == user.Id))
                    {
                        continue;
                    }

                    users.Add(user);
                }
            }


            return users;


        }

        public bool MergUsers(IConnectionHandler connectionHandler, IConnectionHandler enterpriseNodeConnection, IConnectionHandler formGeneratorConnection, IConnectionHandler paymentConnection, IConnectionHandler reservationConnection, Guid sourceUserId, List<Guid> list)
        {

            foreach (Guid guid in list)
            {
                User user1 = Get(connectionHandler, guid);
                if (user1 == null)
                {
                    continue;
                }

                if (!Delete(connectionHandler, paymentConnection, reservationConnection, user1.Id))
                {
                    throw new Exception(Resources.Congress.ErrorInDeleteUser);
                }
            }
            return true;
        }


    }
}
