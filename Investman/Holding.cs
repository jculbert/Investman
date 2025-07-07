using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investman
{
    internal class Holding
    {
        public Symbol? symbol { get; set; }
        public float? quantity { get; set; }
        public float? amount { get; set; }
        public float? us_amount { get; set; }
    }
}
