using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Auth
{
    public class JwtOptions
    {
#nullable disable
        public static string SectionName = "JwtOptions";
        public string Key { get; set; }
        public string issure { get; set; }
        public string audience { get; set; }
        public double Duration { get; set; }

    }
}
