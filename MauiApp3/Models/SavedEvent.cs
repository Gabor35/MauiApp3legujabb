using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class SavedEvent
    {
        public int Id { get; set; }
        public string Cime { get; set; }
        public string Helyszin { get; set; }
        public string KepUrl { get; set; }
        public DateTime Datum { get; set; }
        public string Leiras { get; set; }
    }
}
