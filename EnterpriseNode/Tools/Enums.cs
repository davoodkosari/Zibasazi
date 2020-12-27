using Radyn.Framework;

namespace Radyn.EnterpriseNode.Tools
{
    public class Enums
    {
        public enum EnterpriseNodeType : byte
        {
            None=0,
            [Description("حقیقی")]
            RealEnterPriseNode = 1,
            [Description("حقوقی")]
            LegalEnterPriseNode = 2,

        }
        public enum Gender : byte
        {

            None = 0,
            [Description("Man", Type = typeof(Resources.EnterpriseNode))]
            Man = 1,
            [Description("Women", Type = typeof(Resources.EnterpriseNode))]
            Women = 2,

        }
    }
}
