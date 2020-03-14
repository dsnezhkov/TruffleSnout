using System;

namespace TruffleSnout
{
    static class CommonOptCtl
    {
        public static void Run(CommonOptions opts)
        {

            if (opts.verbose)
                Console.WriteLine("Verbose on!");

        }
    }
}
