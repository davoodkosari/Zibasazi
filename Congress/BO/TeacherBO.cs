using Radyn.Congress.DataStructure;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class TeacherBO : BusinessBase<Teacher>
    {
        public override bool Update(IConnectionHandler connectionHandler, Teacher obj)
        {
            if(obj.ResumeAttachId != null)  return base.Update(connectionHandler, obj);
            if (!base.Update(connectionHandler, obj)) return false;
            var oldobj = this.Get(connectionHandler, obj.Id);
            return !oldobj.ResumeAttachId.HasValue || FileManagerComponent.Instance.FileFacade.Delete(oldobj.ResumeAttachId);
        }
    }
}
