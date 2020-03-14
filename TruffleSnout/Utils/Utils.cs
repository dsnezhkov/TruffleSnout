using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace TruffleSnout.Utils
{
    static class Utils
    {

        public static string Bytes2SID(List<string> sidBytes)
        {
            byte[] bytes = new byte[sidBytes.Count];
            for (int i = 0; i < sidBytes.Count; i++)
            {
                bytes[i] = Byte.Parse(sidBytes[i]);
            }

            var securityIdentifier = new System.Security.Principal.SecurityIdentifier(bytes, 0);
            return securityIdentifier.ToString();
        }
        public static string Bytes2GUID(List<string> guidBytes)
        {
            byte[] bytes = new byte[guidBytes.Count];
            for (int i = 0; i < guidBytes.Count; i++)
            {
                bytes[i] = Byte.Parse(guidBytes[i]);
            }

            var GUID = new System.Guid(bytes);
            return GUID.ToString();
        }

        public static DateTime TicksToTime(Int64 ticks)
        {
            DateTime dt = new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                dt= new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddTicks(ticks);
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            return dt;
        }

        public static bool AcctExpires(Int64 value)
        {
            if (value == 9223372036854775807)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
        public static string GetPassword()
        {
            var pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            return SecureStringToString(pwd);
        }
    }
}
