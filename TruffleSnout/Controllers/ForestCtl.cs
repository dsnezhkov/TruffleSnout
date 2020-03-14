using TruffleSnout.Utils;
using System;

namespace TruffleSnout
{
    public static class ForestCtl
    {
        public static void Run(ForestOptions opts)
        {
            BBForest bbf;

            Config.Forest.ForestName = opts.ForestName;

            if (opts.All)
            {
                Config.Forest.Domains = true;
                Config.Forest.Trusts = true;
                Config.Forest.GCs = true;
                Config.Forest.Sites = true;
            }
            else
            {
                if (opts.Domains)
                    Config.Forest.Domains = true;
                if (opts.Trusts)
                    Config.Forest.Trusts = true;
                if (opts.GCs)
                    Config.Forest.GCs = true;
                if (opts.Sites)
                    Config.Forest.Sites = true;
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
            if (Config.Forest.ForestName.Equals("current"))
            {
                Console.WriteLine("\n[*] Discover current forest");
                bbf = new BBForest();
                bbf.DiscoverForest();
            }
            else
            {
                // Discover forest by name.
                // May be a need for username/password. 
                Console.WriteLine("\n[*] Discovering forest {0}", Config.Forest.ForestName);

                // If credentials are submitted, use them
                if (!Config.Auth.User.Equals(String.Empty))
                {
                    if (!Config.Auth.Password.Equals(String.Empty))
                    {
                        bbf = new BBForest(Config.Forest.ForestName, Config.Auth.User, Config.Auth.Password);
                        bbf.DiscoverForest();
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Need both user and password");
                    }
                }
                else
                {
                    // If credentials are not provided, use current ones.
                    bbf = new BBForest(Config.Forest.ForestName);
                    bbf.DiscoverForest();
                }
            }
        }
    }
}
