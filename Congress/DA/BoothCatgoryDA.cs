using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
namespace Radyn.Congress.DA
{
    public sealed class BoothCatgoryDA : DALBase<BoothCatgory>
    {
        public BoothCatgoryDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    }
    internal class BoothCatgoryCommandBuilder
    {
    }
}
