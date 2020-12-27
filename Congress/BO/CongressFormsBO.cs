using System;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;


namespace Radyn.Congress.BO
{
    internal class CongressFormsBO : BusinessBase<CongressForms>
    {
        public bool AssgineForm(IConnectionHandler connectionHandler, IConnectionHandler formgeneratorconnectionHandler, Guid congressId,FormStructure formStructure, string url, bool forUser)
        {
            var formAssigmentTransactionalFacade = FormGeneratorComponent.Instance.FormAssigmentTransactionalFacade(formgeneratorconnectionHandler);
            var formStructureTransactionalFacade = FormGeneratorComponent.Instance.FormStructureTransactionalFacade(formgeneratorconnectionHandler);
            var userFormsTransactionalFacade = new UserFormsBO();
            var formAssigments = formAssigmentTransactionalFacade.Where(x => x.FormStructureId == formStructure.Id);
            var userform = userFormsTransactionalFacade.Get(connectionHandler,congressId, formStructure.Id);
            if (forUser)
            {
                if (userform == null)
                {
                    var userformdata = new UserForms() { CongressId = congressId, FormId = formStructure.Id };
                    if (!userFormsTransactionalFacade.Insert(connectionHandler, userformdata))
                        throw new Exception("خطایی در ویرایش فرم وجود دارد");
                }
               
            }
            else
            {
                if (userform != null)
                {
                        if (!userFormsTransactionalFacade.Delete(connectionHandler, userform))
                            throw new Exception("خطایی در ویرایش فرم وجود دارد");
                }
            }
           
            foreach (var formAssigment in formAssigments)
            {
                if (!string.IsNullOrEmpty(url) && url.Equals(formAssigment.Url.ToLower())) continue;
                if (!formAssigmentTransactionalFacade.Delete(formAssigment.FormStructureId, formAssigment.Url))
                    throw new Exception("خطایی در ویرایش فرم وجود دارد");
            }
            if (!string.IsNullOrEmpty(url))
            {
                var assigments = formAssigmentTransactionalFacade.GetByUrl(url);
                if (assigments == null)
                {
                    assigments = new FormAssigment { FormStructureId = formStructure.Id, Url = url };
                    if (!formAssigmentTransactionalFacade.Insert(assigments))
                        throw new Exception("خطایی در ویرایش فرم وجود دارد");
                }
                else
                {
                    if (assigments.FormStructureId != formStructure.Id)
                    {
                        if (!formAssigmentTransactionalFacade.Delete(assigments.FormStructureId, assigments.Url))
                            throw new Exception("خطایی در ویرایش فرم وجود دارد");
                        assigments = new FormAssigment { FormStructureId = formStructure.Id, Url = url };
                        if (!formAssigmentTransactionalFacade.Insert(assigments))
                            throw new Exception("خطایی در ویرایش فرم وجود دارد");
                    }

                }
            }


            if (!formStructureTransactionalFacade.Update(formStructure))
                throw new Exception("خطایی در ویرایش فرم وجود دارد");
            return true;
        }
    }
}
