using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.api
{
    public class ApiResponse
    {
        public decimal ApiVersion { get; set; }
        public string HomeAddress { get; set; }
        public string LoginAddress { get; set; }
        public string ChatAddress { get; set; }
        public string DownloadPath { get; set; }
    }
}
