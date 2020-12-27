using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.Congress.DataStructure;
using Radyn.Congress.DA;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class RefereePivotBO : BusinessBase<RefereePivot>
    {

        public bool Insert(IConnectionHandler connectionHandler, Guid refreeId, List<Guid> pivots)
        {
            if (pivots == null) return true;
            foreach (var pivot in pivots)
            {
                var refereePivot = new RefereePivot { RefereeId = refreeId, PivotId = pivot };
                if (!this.Insert(connectionHandler, refereePivot))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
            }
            return true;
        }

        public bool Update(IConnectionHandler connectionHandler, Guid refreeId, List<Guid> pivots)
        {
            if (pivots == null) return true;
            foreach (var pivot in pivots)
            {
               
                var refereePivot1 = this.GetByPivotAndRefereeId(connectionHandler, refreeId, pivot);
                if (refereePivot1 != null) continue;
                var refereePivot = new RefereePivot { RefereeId = refreeId, PivotId = pivot };
                if (!this.Insert(connectionHandler, refereePivot))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
            }
          
            var list = this.GetAllByRefereeId(connectionHandler, refreeId);

            foreach (var refereePivot in list)
            {
                if (pivots.All(x => x != refereePivot.PivotId))
                {
                    if (!this.Delete(connectionHandler, refereePivot.RefereeId, refereePivot.PivotId))
                        throw new Exception("خطایی در حذف محور های داور وجود دارد");
                }
            }
            return true;
        }
        public bool UpdatePivotReferee(IConnectionHandler connectionHandler, Guid pivotId, List<Guid> refreeIds)
        {
            foreach (var guid in refreeIds)
            {
                var refereePivot1 = this.Get(connectionHandler, guid, pivotId);
                if (refereePivot1 != null) continue;
                var refereePivot = new RefereePivot { RefereeId = guid, PivotId = pivotId };
                if (!this.Insert(connectionHandler, refereePivot))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
            }
            var list = this.Select(connectionHandler,x=>x.RefereeId, x => x.PivotId == pivotId);
            foreach (var refereePivot in list)
            {
                if (refreeIds.All(x => x != refereePivot))
                {
                    if (!this.Delete(connectionHandler, refereePivot, pivotId))
                        throw new Exception("خطایی در حذف محور های داور وجود دارد");
                }
            }
            return true;
        }


        public Dictionary<Referee, bool> Search(IConnectionHandler connectionHandler, string txt, Guid pivotId, Guid congressId)
        {
          
            var predicateBuilder = new PredicateBuilder<Referee>();
            predicateBuilder.And(x => x.CongressId == congressId);
            if (!string.IsNullOrEmpty(txt))
            {
                var txtSearch = txt.ToLower();
                predicateBuilder.And((x => x.Username.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.FirstName.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.LastName.Contains(txtSearch)
                || x.EnterpriseNode.RealEnterpriseNode.NationalCode.Contains(txtSearch) || x.EnterpriseNode.RealEnterpriseNode.IDNumber.Contains(txtSearch) || x.EnterpriseNode.Address.Contains(txtSearch)
                || x.EnterpriseNode.Website.Contains(txtSearch) || x.EnterpriseNode.Email.Contains(txtSearch) || x.EnterpriseNode.Tel.Contains(txtSearch)));

            }
            var dictionary = new Dictionary<Referee, bool>();
            var byFilter = new RefereePivotBO().Select(connectionHandler, x=>x.RefereeId,x => x.PivotId == pivotId);
            var list=new RefereeBO().Where(connectionHandler,predicateBuilder.GetExpression());
            foreach (var referee in list)
            {
                if (dictionary.Any(x => x.Key.Id == referee.Id)) continue;
                var added = byFilter.Any(x => x.Equals(referee.Id));
                dictionary.Add(referee, added);

            }
            return dictionary;
        }

        public RefereePivot GetByPivotAndRefereeId(IConnectionHandler connectionHandler, Guid? Id, Guid pivotId)
        {
            RefereePivotDA dal = new RefereePivotDA(connectionHandler);
            return dal.GetByPivotAndRefereeId(Id, pivotId);

        }

        public List<RefereePivot> GetAllByRefereeId(IConnectionHandler connectionHandler, Guid Id)
        {
            RefereePivotDA dal = new RefereePivotDA(connectionHandler);
            return dal.GetAllByRefereeId(Id);
        }
    }
}
