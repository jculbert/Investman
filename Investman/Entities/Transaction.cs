using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investman.Entities
{
    public class Transaction
    {
        public uint id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
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

        public Transaction(string account, string symbol)
        {
            this.account = account;
            this.symbol = symbol;
            date = "2020-12-30";
            type = "DIST_D";
            quantity = 0;
            amount = 0;
            price = 0;
        }
    }
}
