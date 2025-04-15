using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public partial class Kijelentkeze
    {
        public int Id { get; set; }

        public int FelhasznaloId { get; set; }

        public DateTime? KijelentkezesDatuma { get; set; }

        public string? Ipaddress { get; set; }
    }
}
