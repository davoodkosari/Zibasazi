using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.DA
{
    public sealed class CongressLanguageDA : DALBase<CongressLanguage>
    {
        public CongressLanguageDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
       
    }
    internal class CongressLanguageCommandBuilder
    {
       
    }
}
