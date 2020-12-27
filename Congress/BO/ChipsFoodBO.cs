using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class ChipsFoodBO : BusinessBase<ChipsFood>
    {

        public override bool Insert(IConnectionHandler connectionHandler, ChipsFood obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            obj.BaseCapacity = obj.Capacity;
            return base.Insert(connectionHandler, obj);
        }
        public Dictionary<User, bool> SearchChipFood(IConnectionHandler connectionHandler, Guid congressId, Guid chipFoodId, string txtSearch, User user, EnterpriseNode.Tools.Enums.Gender gender, FormStructure formStructure)
        {

            var outlist = new Dictionary<User, bool>();
            var search = new UserBO().Search(connectionHandler, congressId, txtSearch, user, Enums.AscendingDescending.Descending, Enums.SortAccordingToUser.RegisterDate, gender, formStructure);

            var chipsFoodUserBo = new ChipsFoodUserBO();
            var @select = chipsFoodUserBo.Select(connectionHandler, x => x.UserId, x => x.ChipsFoodId == chipFoodId);
            foreach (var item in search)
            {
                var chipsFoodUsers = @select.Any(x => x.Equals(item.Id));
                outlist.Add(item, chipsFoodUsers);
            }
            return outlist;

        }
        protected override void CheckConstraint(IConnectionHandler connectionHandler, ChipsFood item)
        {
            base.CheckConstraint(connectionHandler, item);
            if (string.IsNullOrEmpty(item.DaysInfo))
                throw new Exception(Resources.Congress.PleaseEnterChipFoodDays);
        }
        public bool Insert(IConnectionHandler connectionHandler, ChipsFood chipsFood, List<int> days)
        {

            var chipsFoodBo = new ChipsFoodBO();
            var str = string.Empty;
            foreach (var day in days)
            {
                if (!string.IsNullOrEmpty(str)) str += "-";
                str += day;
            }
            chipsFood.DaysInfo = str;
            if (!chipsFoodBo.Insert(connectionHandler, chipsFood))
                throw new Exception(Resources.Congress.ErrorInSaveChipFood);

            return true;


        }

        public bool Update(IConnectionHandler connectionHandler, ChipsFood chipsFood, List<int> days)
        {

            var chipsFoodBo = new ChipsFoodBO();
            var str = string.Empty;
            foreach (var day in days)
            {
                if (!string.IsNullOrEmpty(str)) str += "-";
                str += day;
            }
            chipsFood.DaysInfo = str;
            if (!chipsFoodBo.Update(connectionHandler, chipsFood))
                throw new Exception(Resources.Congress.ErrorInUpdateChipFood);

            return true;

        }

        public bool JoinUser(IConnectionHandler connectionHandler, Guid chipsFoodid, List<Guid> list)
        {
            var chipsFoodBo = new ChipsFoodBO();
            var food = chipsFoodBo.Get(connectionHandler, chipsFoodid);
            if (food == null || food.Capacity == 0)
                throw new Exception(Resources.Congress.Capacityisnotenough);
            var chipsFoods = new ChipsFoodUserBO().Where(connectionHandler, x => x.ChipsFoodId == chipsFoodid);
            foreach (var guid in list)
            {
                var chipsFoodUser = new ChipsFoodUserBO().Get(connectionHandler, chipsFoodid, guid);
                if (chipsFoodUser == null)
                {
                    var foodUser = new ChipsFoodUser { UserId = guid, ChipsFoodId = chipsFoodid };
                    if (!new ChipsFoodUserBO().Insert(connectionHandler, foodUser))
                        throw new Exception(Resources.Congress.ErrorInSaveChipFood);
                    food.Capacity--;

                }

            }
            foreach (var chipsFood in chipsFoods)
            {
                if (list.All(x => x != chipsFood.UserId))
                {
                    if (!new ChipsFoodUserBO().Delete(connectionHandler, chipsFoodid, chipsFood.UserId))
                        throw new Exception(Resources.Congress.ErrorInSaveChipFood);
                    food.Capacity++;
                }
            }
            if (!chipsFoodBo.Update(connectionHandler, food))
                throw new Exception(Resources.Congress.ErrorInSaveChipFood);
            return true;

        }


    }
}
