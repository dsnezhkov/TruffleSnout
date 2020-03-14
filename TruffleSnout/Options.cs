using System;
using System.Collections.Generic;
using CommandLine;

namespace TruffleSnout
{

    interface IForestOptions
    {

        [Option('n', "name", Required = true, 
            HelpText = 
            "The name of a forest or `current`\n" +
            "Example: -n top.int"
            )]
        string ForestName { get; set; }

        [Option('d', "domains", Required = false,  
            HelpText = 
            "Get forest domains\n" +
            "Example: -d"
            )]
        bool Domains { get; set; }

        [Option('t', "trusts", Required = false,  
            HelpText = 
            "Get forest trusts\n" +
            "Example: -t "
            )]
        bool Trusts { get; set; }

        [Option('s', "sites", Required = false,  
            HelpText = 
            "Get forest sites\n" +
            "Example: -s "
            )]
        bool Sites { get; set; }

        [Option('g', "gcs", Required = false, 
            HelpText = 
            "Get Global Catalogs\n"+
            "Example -g "
            )]
        bool GCs { get; set; }

        [Option('a', "all", Required = false, 
            HelpText = 
            "Get all forest info. This is a shortcut to -g -s -d -t \n" +
            "Example: -a "
            )]
        bool All { get; set; }

        [Option('U', "username", Required = false, 
            HelpText = "(Auth) User Name for connection to the service\n"+
            "Example: -U username"
            )]
        string UserName { get; set; }

        [Option('P', "password", Required = false, 
            HelpText = "(Auth) Password fot the user. \n" +
            "Provide: `interactive` to ask for a password. \n" +
            "OpSec: Clear text passwords on the command line if used without `interactive`\n" +
            "Example: -P <password> || -P interactive"
            )]
        string Password { get; set; }

    }

    interface IDomainOptions
    {

        [Option('n', "name", Required = true, 
            HelpText = 
            "The name of a domain or `current`"
            )]
        string DomainName { get; set; }

        [Option('c', "controller", Required = false,  
            HelpText = 
            "Get domain controllers"
            )]
        bool Controllers { get; set; }

        [Option('t', "trusts", Required = false,  
            HelpText = 
            "Get domain trusts"
            )]
        bool Trusts { get; set; }

        [Option('a', "all", Required = false, 
            HelpText = 
            "Get all domain info. This is a shortcut to -c -t \n" +
            "Example: -a "
            )]
        bool All { get; set; }

        [Option('U', "username", Required = false, 
            HelpText = "(Auth) User Name for connection to the service\n"+
            "Example: -U username"
            )]
        string UserName { get; set; }

        [Option('P', "password", Required = false, 
            HelpText = "(Auth) Password fot the user. \n" +
            "Provide: `interactive` to ask for a password. \n" +
            "OpSec: Clear text passwords on the command line if used without `interactive`\n" +
            "Example: -P <password> || -P interactive"
            )]
        string Password { get; set; }
    }

    interface IDirectoryOptions
    {

        [Option('l', "ldap", Required = true, 
            HelpText = 
            "LDAP Connection Path. Can include starting DN\n" +
            "Example:  LDAP://other.int/DC=other,DC=int"
            )]
        string LdapPath { get; set; }

        [Option('f', "filter", Required = true, 
            HelpText =
            "Query Filter, in relation to the starting DN\n" +
            "Syntax: https://docs.microsoft.com/en-us/windows/win32/adsi/search-filter-syntax \n"+
            "Example: '(&(objectClass=user))'"
            )]
        string Filter { get; set; }

        [Option('a', "attr", Required = false, Separator = ',', 
            HelpText =
            "Limit query to specific attribute(s). Attributes separated by ','. \n" + 
            "`meta` - shows the attribute keys.\n" +
            "Example: -a meta || -a  badpwdcount,cn,memberof"
            )]
        IEnumerable<string> Attrs { get; set; }

        [Option('c', "count", Required = true, 
            HelpText = 
            "Limit of entries returned in a query\n" + 
            "Example: -c 10 "
            )]
        int Count { get; set; }

        [Option('n', "nonsecure", Required = false, 
            HelpText = 
            "Do basic authentication. Do not use Kerberos/NTLM. This is a falback setting.\n" +
            "Opsec: clear text connectivity.\n" +
            "Example: -n "
            )]
        bool NonSecure { get; set; }

        [Option('s', "ssl", Required = false, 
            HelpText = 
            "Connect as LDAP/S (SSL). This assumes certificates are properly configured.\n" +
            "Note: to specify port use the format provided in `-l` option: LDAP://server.domain:PORT\n" +
            "Example: -s "
            )]
        bool SecureSocket { get; set; }

        [Option('U', "username", Required = false, 
            HelpText = "(Auth) User Name for connection to the service\n"+
            "Example: -U username"
            )]
        string UserName { get; set; }

        [Option('P', "password", Required = false, 
            HelpText = "(Auth) Password fot the user. \n" +
            "Provide: `interactive` to ask for a password. \n" +
            "OpSec: Clear text passwords on the command line if used without `interactive`\n" +
            "Example: -P <password> || -P interactive"
            )]
        string Password { get; set; }
    }

    [Verb("forest", HelpText = "Forest discovery workflow")]
    public class ForestOptions:  IForestOptions
    {
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
        [Option('t', "ticks2date", Required = false, 
            HelpText = 
            "Convert time ticks to DateTime.\n" +
            "Example: -t 132280170552471892. Output: 3/7/2020 1:10:55 AM"
            )]
        public Int64 Ticks { get; set; } = 0;
        [Option('e', "expires", Required = false, 
            HelpText = 
            "Check if a value of record expires\n" +
            "Example: -e 9223372036854775807. Output: Expires? Yes"
            )]
        public Int64 Expires { get; set; } = 0;
        [Option('s', "bytes2SID", Required = false, Separator = ' ',  
            HelpText =
            "Convert Bytes to SID. Used in `objectsid` attribute\n" +
            "Example: -s 1 5 0 0 0 0 0 5 21 0 0 0 1 94 67 225 228 177 51 26 1 202 233 128 80 4 0 0\n" +
            "Output: SID: S-1-5-21-3779288577-439595492-2162805249-1104"
            )]
        public IEnumerable<string> Bytes2SID { get; set; } = new string[] { };
        [Option('g', "bytes2GUID", Required = false, Separator = ' ',  
            HelpText =
            "Convert Bytes to GUID. Used in `objectguid` attribute\n" +
            "Example: -g 24 184 226 216 110 108 41 79 149 222 213 140 6 116 200 172\n" +
            "Output: GUID: d8e2b818-6c6e-4f29-95de-d58c0674c8ac"
            )]
        public IEnumerable<string> Bytes2GUID { get; set; } = new string[] { };

    }

    class CommonOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Verbose output.")]
        public bool verbose { get; set; } = false;

    }
}
