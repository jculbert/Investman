using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investman.Entities
{
    public class Upload
    {
        public int id { get; set; }
        public string date { get; set; }
        public string? file_name { get; set; }
        public int? num_transactions { get; set; }
        public string? result { get; set; }
        public string? content { get; set; }
    }
}
