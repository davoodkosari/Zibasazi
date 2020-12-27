using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressFormsFacade : CongressBaseFacade<CongressForms>, ICongressFormsFacade
    {
        internal CongressFormsFacade()
        {
        }

        internal CongressFormsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

     
        public bool UpdateAndAssgine(Guid congressId,FormStructure structure, string urls,bool forUser)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var formAssigmentBo = new CongressFormsBO();
                if (!formAssigmentBo.AssgineForm(this.ConnectionHandler, this.FormGeneratorConnection, congressId,structure, urls,forUser))
                    throw new Exception("خطایی در ویرایش فرم وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;

            }

            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressFormsBo = new CongressFormsBO();
                var obj = congressFormsBo.Get(this.ConnectionHandler, keys);
                if (!congressFormsBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressContainer);
                if (
                    !FormGeneratorComponent.Instance.FormStructureTransactionalFacade(this.FormGeneratorConnection)
                        .Delete(obj.FomId))
                    throw new Exception("خطایی در حذف فرم وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, FormStructure formStructure)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                formStructure.IsExternal = true;
                var formStructureTransactionalFacade =
                    FormGeneratorComponent.Instance.FormStructureTransactionalFacade(this.FormGeneratorConnection);
                if (!formStructureTransactionalFacade.Insert(formStructure))
                    throw new Exception("خطایی در ذخیره فرم وجود دارد");
                var congressContent = new CongressForms {FomId = formStructure.Id, CongressId = congressId};
                if (!new CongressFormsBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressContainer);
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }


    }
}
