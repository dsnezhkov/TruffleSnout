using System;
using System.Collections.Generic;
using CommandLine;

namespace BigBertha
{

    interface IForestOptions
    {

        [Option('f', "forest", Required = true, HelpText = "The name of a forest or `current`")]
        string ForestName { get; set; }

        [Option('d', "domains", Required = false,  HelpText = "Get forest domains")]
        bool Domains { get; set; }

        [Option('t', "trusts", Required = false,  HelpText = "Get forest trusts")]
        bool Trusts { get; set; }

        [Option('s', "sites", Required = false,  HelpText = "Get forest sites")]
        bool Sites { get; set; }

        [Option('g', "gcs", Required = false, HelpText = "Get Global Catalogs")]
        bool GCs { get; set; }

        [Option('a', "all", Required = false, HelpText = "Get All forest info")]
        bool All { get; set; }

        [Option('u', "username", Required = false, HelpText = "(Auth) User Name")]
        string UserName { get; set; }
        
        [Option('p', "password", Required = false, HelpText = "(Auth) Password or `interactive`")]
        string Password { get; set; }
    }

    interface IDomainOptions
    {

        [Option('d', "domain", Required = true, HelpText = "The name of a domain or `current`")]
        string DomainName { get; set; }

        [Option('c', "controller", Required = false,  HelpText = "Get domain controllers")]
        bool Controllers { get; set; }

        [Option('t', "trusts", Required = false,  HelpText = "Get domain trusts")]
        bool Trusts { get; set; }

        [Option('a', "all", Required = false, HelpText = "Get All domain info")]
        bool All { get; set; }

        [Option('u', "username", Required = false, HelpText = "(Auth) User Name")]
        string UserName { get; set; }
        
        [Option('p', "password", Required = false, HelpText = "(Auth) Password or `interactive`")]
        string Password { get; set; }
    }

    interface IDirectoryOptions
    {

        [Option('l', "ldap", Required = true, HelpText = "LDAP Path")]
        string LdapPath { get; set; }

        [Option('f', "filter", Required = true, HelpText = "Query Filter")]
        string Filter { get; set; }

        [Option('a', "attr", Required = false, Separator = ',', HelpText = 
            @"Attributes to query from LDAP response collection.
                `meta` - shows the structure")]
        IEnumerable<string> Attrs { get; set; }

        [Option('c', "count", Required = true, HelpText = "Limit of entries to return in a query")]
        int Count { get; set; }


        /// <summary>
        /// Conection Type 
        /// </summary>
        [Option('n', "nonsecure", Required = false, HelpText = "Do basic auth, not (KRB/NTLM) connection. This is falback")]
        bool NonSecure { get; set; }

        [Option('s', "SSL", Required = false, HelpText = "Do LDAPS (SS)")]
        bool SecureSocket { get; set; }

        /// <summary>
        /// Authentication
        /// </summary>
        [Option('u', "username", Required = false, HelpText = "(Auth) User Name")]
        string UserName { get; set; }

        [Option('p', "password", Required = false, HelpText = "(Auth) Password or `interactive`")]
        string Password { get; set; }
    }

    [Verb("forest", HelpText = "Forest discovery workflow")]
    public class ForestOptions:  IForestOptions
    {

        //Implement commands in forest set
        public string ForestName { get; set; } = String.Empty;
        public bool Domains { get; set; } = false; 
        public bool Trusts { get; set; } = false;
        public bool GCs { get; set; } = false;
        public bool Sites { get; set; } = false;
        public bool All { get; set; } = false;

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

    [Verb("domain", HelpText = "Domain discovery workflow")]
    public class DomainOptions:  IDomainOptions
    {

        //Implement commands in forest set
        public string DomainName { get; set; } = String.Empty;
        public bool Controllers { get; set; } = false; 
        public bool Trusts { get; set; } = false;
        public bool All { get; set; } = false;

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

    [Verb("directory", HelpText = "Directory discovery workflow")]
    public class DirectoryOptions:  IDirectoryOptions
    {

        //Implement commands in forest set
        public string LdapPath { get; set; } = String.Empty;
        public string Filter { get; set; } = String.Empty;
        public IEnumerable<string> Attrs { get; set; } = new string[] { };
        public int Count { get; set; } = 0; // TODO: Opsec

        public bool NonSecure { get; set; } = false;
        public bool SecureSocket { get; set; } = false;

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

    [Verb("util", HelpText = "Utilities")]
    public class UtilitiesOptions
    {
        [Option('t', "ticks2date", Required = false, HelpText = "Ticks to Date")]
        public Int64 Ticks { get; set; } = 0;
        [Option('e', "expires", Required = false, HelpText = "Expires?")]
        public Int64 Expires { get; set; } = 0;

        [Option('s', "bytes2SID", Required = false, Separator = ' ',  HelpText = "Bytes to SID")]
        public IEnumerable<string> Bytes2SID { get; set; } = new string[] { };
        [Option('g', "bytes2GUID", Required = false, Separator = ' ',  HelpText = "Bytes to GUID")]
        public IEnumerable<string> Bytes2GUID { get; set; } = new string[] { };

    }

    class CommonOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Verbose output.")]
        public bool verbose { get; set; } = false;

    }
}
