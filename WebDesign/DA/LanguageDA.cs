using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class LanguageDA : DALBase<Language>
    {
        public LanguageDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class LanguageCommandBuilder
    {
    }
}
