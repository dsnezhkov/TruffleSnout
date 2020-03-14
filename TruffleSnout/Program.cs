using System;
using System.Collections.Generic;
using CommandLine;

namespace TruffleSnout
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ForestOptions, 
                DomainOptions, DirectoryOptions, UtilitiesOptions, CommonOptions>(args)
                .WithParsed<ForestOptions>(ForestCtl.Run)
                .WithParsed<DirectoryOptions>(DirectoryCtl.Run)
                .WithParsed<DomainOptions>(DomainCtl.Run)
                .WithParsed<UtilitiesOptions>(UtilCtl.Run)
                .WithParsed<CommonOptions>(CommonOptCtl.Run)
                .WithNotParsed(HandleParseError);
        }
        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion()) { return; }
            if (errs.IsHelp()) { return; }

            foreach (var e in errs)
            {
                Console.WriteLine($"Error: {e.ToString()}");
            }

            return;
        }
    }
}
