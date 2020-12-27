using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class WorkShopTeacherFacade : CongressBaseFacade<WorkShopTeacher>, IWorkShopTeacherFacade
    {
        internal WorkShopTeacherFacade() { }

        internal WorkShopTeacherFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }

      
    }
}
