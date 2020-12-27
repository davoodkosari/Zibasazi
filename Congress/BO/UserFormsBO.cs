using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using Radyn.Congress.DataStructure;
using Radyn.FormGenerator;
using Radyn.FormGenerator.ControlFactory.Controls;
using Radyn.FormGenerator.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Control = Radyn.FormGenerator.ControlFactory.Base.Control;

namespace Radyn.Congress.BO
{
    internal class UserFormsBO : BusinessBase<UserForms>
    {
        internal DataTable ReportUserForms(IConnectionHandler connectionHandler, Guid formId, Guid homaId,string culture)
        {


            try
            {

                var table = new DataTable();
                var userList = new UserBO().Where(connectionHandler, x => x.CongressId == homaId);
                var list = FormGeneratorComponent.Instance.FormDataFacade.Where(x => x.StructureId == formId && x.ObjectName == typeof(UserForms).Name);
                var formStructure = FormGeneratorComponent.Instance.FormStructureFacade;
                var form = formStructure.Get(formId);

                table.Columns.Add(Resources.Congress.Username);
                foreach (Control control in form.Controls)
                {
                    if (control == null) continue;
                    if (control.GetType() == typeof(Label) || control.GetType() == typeof(FileUploader)) continue;
                    var columnName = control.GetCaption();

                    table.Columns.Add(columnName, control.DisplayValue != null ? control.DisplayValueType : typeof(string));
                }
                if (culture == "fa-IR")
                {
                    var ordinal = table.Columns.Count - 1;

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        table.Columns[0].SetOrdinal(ordinal);
                        ordinal--;
                    }
                }
              
                foreach (var user in userList)
                {

                    var firstOrDefualt = list.FirstOrDefault(c => c.RefId == user.Id.ToString());
                    var dictionary = firstOrDefualt != null ? Extentions.GetControlData(firstOrDefualt.Data) : null;
                    var row = table.NewRow();
                    var stringWriter = new StringWriter();
                    var writer = new Html32TextWriter(stringWriter);
                    row[Resources.Congress.Username] = user.Username;

                    foreach (Control control in form.Controls)
                    {
                        if (control.GetType() == typeof(Label) || control.GetType() == typeof(FileUploader)) continue;
                        control.Writer = writer;
                        control.FormState = FormState.DetailsMode;
                        control.Value = dictionary != null && dictionary.ContainsKey(control.Id) ? dictionary[control.Id] : string.Empty;
                        control.Generate();
                        var columnName = control.GetCaption();
                        row[columnName] = control.DisplayValue != null ? control.DisplayValue.ToString() : string.Empty;

                    }
                    table.Rows.Add(row);

                }

                return table;

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
