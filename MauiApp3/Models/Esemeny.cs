using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public partial class Esemeny
    {
        public int Id { get; set; }

        public string Cime { get; set; } = null!;

        public string Helyszin { get; set; } = null!;

        public DateTime Datum { get; set; }

        public string? Leiras { get; set; }

        private string _kepurl = "";

        public string Kepurl
        {
            get
            {
                if (string.IsNullOrEmpty(_kepurl))
                {
                    return "https://images-0prm.onrender.com/default.jpg";
                }

                return _kepurl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? _kepurl
                    : $"https://images-0prm.onrender.com/{_kepurl}";
            }
            set => _kepurl = value ?? "";
        }

    }
}
