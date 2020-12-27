using System;
using System.Collections.Generic;
using System.Data;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class FormsFacade : WebDesignBaseFacade<Forms>, IFormsFacade
    {
        internal FormsFacade() { }

        internal FormsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressFormsBo = new FormsBO();
                var obj = congressFormsBo.Get(this.ConnectionHandler, keys);
                if (!congressFormsBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف فرم وجود دارد");
                if (!FormGeneratorComponent.Instance.FormStructureTransactionalFacade(this.FormGeneratorConnection).Delete(obj.FormId))
                    throw new Exception("خطایی در حذف فرم وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }



        public bool Insert(Guid websiteId, FormStructure formStructure)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                formStructure.IsExternal = true;
                var formStructureTransactionalFacade = FormGeneratorComponent.Instance.FormStructureTransactionalFacade(this.FormGeneratorConnection);
                if (!formStructureTransactionalFacade.Insert(formStructure))
                    throw new Exception("خطایی در ذخیره فرم وجود دارد");
                var congressContent = new Forms { FormId = formStructure.Id, WebId = websiteId };
                if (!new FormsBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception("خطایی در ذخیره فرم وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
