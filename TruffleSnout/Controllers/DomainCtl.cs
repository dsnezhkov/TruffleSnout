using TruffleSnout.Utils;
using System;

namespace TruffleSnout
{
    public static class DomainCtl
    {
        public static void Run(DomainOptions opts)
        {
            BBDomain bbd;

            Config.Domain.DomainName = opts.DomainName;


            if (opts.All)
            {
                Config.Domain.Controllers = true;
                Config.Domain.Trusts = true;
            }
            else
            {
                if (opts.Controllers)
                    Config.Domain.Controllers = true;
                if (opts.Trusts)
                    Config.Domain.Trusts = true;
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

            // No need for username/password. Discovery for a situational context
            if (Config.Domain.DomainName.Equals("current"))
            {
                Console.WriteLine("\n[*] Discover current domain");
                bbd = new BBDomain();
                bbd.DiscoverDomain();
            }
            else
            {
                // Discover forest by name.
                // May be a need for username/password. 
                Console.WriteLine("\n[*] Discovering domain {0}", Config.Domain.DomainName);

                // If credentials are submitted, use them
                if (!Config.Auth.User.Equals(String.Empty))
                {
                    if (!Config.Auth.Password.Equals(String.Empty))
                    {
                        bbd = new BBDomain(Config.Domain.DomainName, Config.Auth.User, Config.Auth.Password);
                        bbd.DiscoverDomain();
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Need both user and password");
                    }
                }
                else
                {
                    // If credentials are not provided, use current ones.
                    bbd = new BBDomain(Config.Domain.DomainName);
                    bbd.DiscoverDomain();
                }
            }
        }
    }
}
