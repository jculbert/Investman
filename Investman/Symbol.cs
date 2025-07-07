using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Investman
{
    internal class Symbol
    {
        public string name { get; set; }
        public string? description { get; set; }
        public float? last_price { get; set; }
        public string? last_price_date { get; set; }
        public string? reviewed_date { get; set; }
        public string? review_result { get; set; }
    }
}
