using TruffleSnout.Utils;
using System;
using System.DirectoryServices;
using System.Linq;

namespace TruffleSnout
{
    public static class DirectoryCtl
    {
        public static void Run(DirectoryOptions opts)
        {

            BBDirectory directoryEntry = null;
            AuthenticationTypes authType = AuthenticationTypes.Secure;

            // Required
            Config.Directory.LDAPPath = opts.LdapPath;
            Config.Directory.Filter = opts.Filter;

            if (opts.Count != 0)
            {
                Config.Directory.Count = opts.Count;
            }

            Config.Directory.Attrs = opts.Attrs.ToList();

            if (opts.NonSecure)
            {
                Config.Directory.NonSecure = true;
                authType = AuthenticationTypes.None;
            }

            if (opts.SecureSocket)
            {
                Config.Directory.SecureSocket = true;
                authType |= AuthenticationTypes.SecureSocketsLayer;
            }

            // Set auth options
            // Only check when user is defined.
            if (!opts.UserName.Equals(String.Empty))
                Config.Auth.User = opts.UserName;

            if (!opts.Password.Equals(String.Empty))
            {
                if (opts.Password.Equals("interactive"))
                {
                    Console.Write("Enter <masked> credentials for `{0}` : ", Config.Auth.User);
                    Config.Auth.Password = Utils.Utils.GetPassword();

                }
                else
                    Config.Auth.Password = opts.Password;
            }


            Console.WriteLine("\n[*] Directory {0}", Config.Directory.LDAPPath);

            // If credentials are submitted, use them
            if (!Config.Auth.User.Equals(String.Empty))
            {
                if (!Config.Auth.Password.Equals(String.Empty))
                {

                    Console.WriteLine("Auth type: {0}", authType);
                    directoryEntry = new BBDirectory(
                        Config.Directory.LDAPPath, Config.Auth.User, Config.Auth.Password, authType);
                }
                else
                {
                    Console.WriteLine("ERROR: Need both user and password");
                    return;
                }
            }
            else
            {
                    Console.WriteLine("Auth type: {0}", authType);
                    directoryEntry = new BBDirectory(
                        Config.Directory.LDAPPath, null, null, authType);
            }

            if (directoryEntry != null)
            {
                directoryEntry.SearchByFiter(Config.Directory.Filter, Config.Directory.Count);
            }
        }
    }
}
