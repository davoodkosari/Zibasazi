using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Evaluation.Models
{
    public class TopSisModel
    {
        public TopSisModel()
        {
            Scoreses = new Dictionary<string, double>();
        }
        public string RefId { get; set; }

        public Dictionary<string, double> Scoreses { get; set; }

    }

    public class TopSisReturnModel
    {

        public string RefId { get; set; }
        public int? Rank { get; set; }
        public double? Score { get; set; }



    }
}
