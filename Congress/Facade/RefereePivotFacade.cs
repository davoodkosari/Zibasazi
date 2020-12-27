using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class RefereePivotFacade : CongressBaseFacade<RefereePivot>, IRefereePivotFacade
    {
        internal RefereePivotFacade()
        {
        }

        internal RefereePivotFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        

        public bool Update(Guid refreeId, List<Guid> pivots)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new RefereePivotBO().Update(this.ConnectionHandler, refreeId, pivots))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
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

        public bool UpdatePivotReferee(Guid pivotId, List<Guid> refereeId)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new RefereePivotBO().UpdatePivotReferee(this.ConnectionHandler, pivotId, refereeId))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
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

        public Dictionary<Referee, bool> Search(string txt, Guid pivotId, Guid id)
        {
            try
            {
                return new RefereePivotBO().Search(this.ConnectionHandler, txt, pivotId, id);

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

        public bool Insert(Guid refreeId, List<Guid> pivots)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new RefereePivotBO().Insert(this.ConnectionHandler, refreeId, pivots))
                    throw new Exception("خطایی در ذخیره محور های داور وجود دارد");
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

        public Dictionary<Referee, bool> GetBypivotId(Guid pivotId)
        {
            try
            {
                var list = new Dictionary<Referee, bool>();
                var byFilter = new RefereePivotBO().OrderBy(this.ConnectionHandler,x=>x.Referee.EnterpriseNode.RealEnterpriseNode.FirstName+" "+x.Referee.EnterpriseNode.RealEnterpriseNode.LastName, x => x.PivotId == pivotId);
                foreach (var variable in byFilter)

                    list.Add(variable.Referee, true);
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

        public Dictionary<Pivot, bool> GetByRefereeId(Guid congressId, Guid? refreeId)
        {
            try
            {
                var list = new Dictionary<Pivot, bool>();
                var byFilter = new PivotBO().OrderBy(this.ConnectionHandler,x=>x.Order, x => x.CongressId == congressId);
                var refereePivotBo = new RefereePivotBO();
                var @select = refereePivotBo.Select(ConnectionHandler,
                    new Expression<Func<RefereePivot, object>>[] {x => x.RefereeId, x => x.PivotId},
                    x => x.Pivot.CongressId == congressId);
                foreach (var variable in byFilter)
                {
                    var added = false;
                    if (refreeId != null)
                        added = @select.Any(x => x.RefereeId == refreeId && x.PivotId == variable.Id);
                    list.Add(variable, added);
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
    }
}
