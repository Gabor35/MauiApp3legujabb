using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class EventModel
    {
        public long EsemenyID { get; set; }
        public string Cime { get; set; } = string.Empty;
        public string Helyszin { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public string Leiras { get; set; } = string.Empty;
        public string Kepurl { get; set; } = string.Empty;
    }
}
