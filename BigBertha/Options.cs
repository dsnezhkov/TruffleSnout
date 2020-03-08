using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace BigBertha
{

    //define commands in set 'web'
    interface IForestOptions
    {

        [Option('f', "forest", Required = true, HelpText = "The name of a forest or `current`")]
        string ForestName { get; set; }

        [Option('d', "domains",  HelpText = "Get forest domains")]
        bool Domains { get; set; }

        [Option('t', "trusts",  HelpText = "Get forest trusts")]
        bool Trusts { get; set; }

        [Option('g', "gcs",  HelpText = "Get Global Catalogs")]
        bool GCs { get; set; }

        [Option('u', "username", HelpText = "(Auth) User Name")]
        string UserName { get; set; }
        
        [Option('p', "password", HelpText = "(Auth) Password or `interactive`")]
        string Password { get; set; }
    }

    [Verb("forest", HelpText = "Forest options")]
    public class ForestOptions:  IForestOptions
    {

        //Implement commands in forest set
        public string ForestName { get; set; } = String.Empty;
        public bool Domains { get; set; } = false; 
        public bool Trusts { get; set; } = false;
        public bool GCs { get; set; } = false;

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
    class CommonOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Verbose output.")]
        public bool verbose { get; set; } = false;

    }
}
