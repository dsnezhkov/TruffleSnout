using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ActiveDirectory.Management;
using System.DirectoryServices.ActiveDirectory;
using CommandLine;
using BigBertha.Utils;

namespace BigBertha
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ForestOptions, CommonOptions>(args)
                .WithParsed<ForestOptions>(Run)
                .WithParsed<CommonOptions>(Run)
                .WithNotParsed(HandleParseError);
        }
        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion()) { return; }
            if (errs.IsHelp()) { return; }

            foreach (var e in errs)
            {
                Console.WriteLine($"Error: {e.ToString()}" );
            }

            return;
        }

        private static void Run(CommonOptions opts)
        { 

            if (opts.verbose)
                Console.WriteLine("Verbose on!");
            
        }
        private static void Run(ForestOptions opts)
        {

            Config.Forest.ForestName = opts.ForestName;
            if (opts.Domains)
                Config.Forest.Domains = true;
            if (opts.Trusts)
                Config.Forest.Trusts = true;
            if (opts.GCs)
                Config.Forest.GCs = true;

            // Set auth options

            // Only check when user is defined.
            if (!opts.UserName.Equals(String.Empty) && !opts.Password.Equals(String.Empty))
            {
                Config.Auth.User = opts.UserName;

                    if (opts.Password.Equals("interactive"))
                    {
                        Console.Write("Enter <masked> credentials for `{0}` : ", Config.Auth.User);
                        Config.Auth.Password = Utils.Utils.GetPassword();

                    }
                    else
                        Config.Auth.Password = opts.Password;
            }
            else
            {
                Console.WriteLine("Asked for a user/password but not provided both?");
                return;
            }

            /*
            Domain domain =  
                Domain.GetCurrentDomain();
            Console.WriteLine( "Current Domain: {0}", domain.Name);

            DomainController dcc = domain.FindDomainController();
            Console.WriteLine( "Current DomainController : {0}, {1}, {2}, {3}", 
                    dcc.Name, 
                    dcc.OSVersion, 
                    dcc.SiteName, dcc.IPAddress);

            DomainControllerCollection dcAll = domain.FindAllDiscoverableDomainControllers();

            Console.WriteLine("All DCs: ");
            foreach (DomainController dc in dcAll)
            {
                Console.WriteLine( "\tDomainController : {0}, {1}, {2}, {3}", 
                    dc.Name, 
                    dc.OSVersion, 
                    dc.SiteName, dc.IPAddress);
            }
            */

            // Current forest

            if ( Config.Forest.ForestName.Equals("current"))
            {
                BBForest bbf = new BBForest();
                Console.WriteLine("\n[*] Discover current forest");
                bbf.DiscoverForest();
            }
            else
            {
                Console.WriteLine("\n[*] Discover forest by name");
                BBForest bbfN = new BBForest(Config.Forest.ForestName);
                bbfN.DiscoverForest();
            }


            if (!Config.Auth.User.Equals(String.Empty) && !Config.Auth.Password.Equals(String.Empty))
            {
                // By name and creds
                Console.WriteLine("\n[*] Discover forest by name and creds");
                string fPass = Utils.Utils.GetPassword();
                BBForest bbfNC = new BBForest("top.int", "user1", fPass);
                bbfNC.DiscoverForest();
            }

            /*
            // ... Domains
            Console.WriteLine("\n[*] \tDomains in current forest");
            // In a current forest
            bbf.DiscoverDomainsInForest();

            Console.WriteLine("\n[*] \tDomains in forest by object");
            bbf.DiscoverDomainsInForest(otherOne);
            */

            Console.WriteLine("Enter Any key to Continue...");
            Console.ReadKey();
            

        }
    }
}
