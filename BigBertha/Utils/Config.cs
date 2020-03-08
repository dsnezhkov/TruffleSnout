using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBertha.Utils
{
    static class Config
    {
        public static class Forest{

            public static string ForestName = String.Empty;
            public static bool Domains = false;
            public static bool Trusts = false;
            public static bool GCs = false;
        }
        public static class Auth
        {
            public static string User = String.Empty;
            public static string Password = String.Empty;
        }

    }
}
