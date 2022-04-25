using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocoaDI
{
    public class MyConfig
    {
        public string SqliteConnectionString { get; set; } = default!;
        public string CaminhoChromeDriver { get; set; } = default!;
        public string UrlTangerino { get; set; } = default!;
    }
}
