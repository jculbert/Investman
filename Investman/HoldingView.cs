using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investman
{
    internal class HoldingView
    {
        public string? symbol_name { get; set; }
        public string? symbol_description { get; set; }
        public string? symbol_reviewed_date { get; set; }
        public string? symbol_review_result { get; set; }
        public float? quantity { get; set; }
        public float? amount { get; set; }
        public float? us_amount { get; set; }

        public HoldingView(Holding h)
        {
            symbol_name = h.symbol.name;
            symbol_description = h.symbol.description;
            symbol_reviewed_date = h.symbol.reviewed_date;
            symbol_review_result = h.symbol.review_result;
            quantity = h.quantity;
            amount = h.amount;
            us_amount = h.us_amount;
        }
    }
}
