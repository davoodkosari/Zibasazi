using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
namespace Radyn.Congress.DA
{
    public sealed class TeacherDA : DALBase<Teacher>
    {
        public TeacherDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class TeacherCommandBuilder
    {
    }
}
