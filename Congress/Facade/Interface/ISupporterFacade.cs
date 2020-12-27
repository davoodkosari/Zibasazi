using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ISupporterFacade : IBaseFacade<Supporter>
    {
        bool Insert(Supporter supporter, HttpPostedFileBase file);
        bool Update(Supporter supporter, HttpPostedFileBase file);

    }
}
