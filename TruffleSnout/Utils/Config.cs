using System;
using System.Collections.Generic;

namespace TruffleSnout.Utils
{
    static class Config
    {
        public static class Forest{

            public static string ForestName = String.Empty;
            public static bool Domains = false;
            public static bool Trusts = false;
            public static bool GCs = false;
            public static bool Sites = false;
        }
        public static class Domain{

            public static string DomainName = String.Empty;
            public static bool Controllers = false;
            public static bool Trusts = false;
        }
        public static class Directory{

            public static string LDAPPath = String.Empty;
            public static string Filter = String.Empty;
            public static List<String> Attrs = new List<String>();
            public static int Count = 0;
            public static bool NonSecure = false;
            public static bool SecureSocket = false;
        }
        public static class Auth
        {
            public static string User = String.Empty;
            public static string Password = String.Empty;
        }

    }
}
