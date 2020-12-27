using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IVIPFacade : IBaseFacade<VIP>
    {
        bool Insert(VIP vip, HttpPostedFileBase fileResume, HttpPostedFileBase file);
        bool Update(VIP vip, HttpPostedFileBase fileResume, HttpPostedFileBase file);

    }
}
