using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investman.Entities
{
    internal class Transaction
    {
        public required uint id { get; set; }
        public required string date { get; set; }
        public required string type { get; set; }
        public float? quantity { get; set; }
        public float? price { get; set; }
        public float? amount { get; set; }
        public float? fee { get; set; }
        public float? capital_return { get; set; }
        public float? capital_gain { get; set; }
        public float? acb { get; set; }
        public string? symbol { get; set; }
        public string? account { get; set; }
        public uint? upload_id { get; set; }
        public string? note { get; set; }
    }
}
