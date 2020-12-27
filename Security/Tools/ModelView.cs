using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radyn.Framework;

namespace Radyn.Security.Tools
{
    [Schema("dbo")]
    public class Access
    {

        public Guid? Id { get; set; }
        public Guid? HelpId { get; set; }
        public string Title { get; set; }
    }
}
