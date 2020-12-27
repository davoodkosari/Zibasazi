using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class WorkShopTeacherDA : DALBase<WorkShopTeacher>
    {
        public WorkShopTeacherDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class WorkShopTeacherCommandBuilder
    {
    }
}
