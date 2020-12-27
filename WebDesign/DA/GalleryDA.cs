using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.DA
{
    public sealed class GalleryDA : DALBase<Radyn.WebDesign.DataStructure.Gallery>
    {
        public GalleryDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }
    }
    internal class GalleryCommandBuilder
    {
    }
}
