using System;
using System.Collections.Generic;
using System.Linq;

namespace TruffleSnout
{
    static class UtilCtl
    {
        public static void Run(UtilitiesOptions opts)
        {

            if (opts.Ticks != 0)
                Console.WriteLine(Utils.Utils.TicksToTime(opts.Ticks));
            if (opts.Expires != 0)
                Console.WriteLine("Expires? {0}", Utils.Utils.AcctExpires(opts.Ticks));

            try
            {
                List<string> bytes2SID = opts.Bytes2SID.ToList<string>();
                int bytes2SidCount = bytes2SID.Count();

                if ( bytes2SidCount !=0)
                {

                Console.WriteLine("SID: {0}", Utils.Utils.Bytes2SID(bytes2SID));
                }
            }catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            try
            {
                List<string> bytes2GUID = opts.Bytes2GUID.ToList<string>();
                int bytes2GuidCount = bytes2GUID.Count();

                if ( bytes2GuidCount !=0)
                {

                Console.WriteLine("GUID: {0}", Utils.Utils.Bytes2GUID(bytes2GUID));
                }
            }catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
 
        }
    }
}
