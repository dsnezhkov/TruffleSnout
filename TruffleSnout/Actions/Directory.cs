using TruffleSnout.Utils;
using System;
using System.Collections;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;

namespace TruffleSnout
{
    class BBDirectory
    {
        private DirectoryEntry directoryEntry { get; set; }


        public BBDirectory(String ldapPath, String user, String password, AuthenticationTypes authType)
        {
            try
            {
                directoryEntry = new DirectoryEntry(ldapPath, user, password, authType);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error Creeating Directory Entry: {e.Message}");
                return;
            }
        }
        private void printAttribute(string key, object attr)
        {



            Type t = attr.GetType();
            if (t.IsArray)
            {
                Console.Write($"\t{key}:Bytes : ");
                foreach (var e in (Array)attr)
                {
                    Console.Write($"{e} ");
                }
                Console.WriteLine();
            }
            else if (t.Equals(typeof(String)))
                Console.WriteLine($"\t{key}:String: {attr}");
            else if (t.Equals(typeof(DateTime)))
                Console.WriteLine($"\t{key}:DateTime: {attr}");
            else if ( (t.Equals(typeof(int)) || t.Equals(typeof(Int32)) || t.Equals(typeof(Int64)) ))
                Console.WriteLine($"\t{key}:Int: {attr}");
            else
                Console.WriteLine($"\t{key}:{t}: {attr}");
        }

        public void SearchByFiter(string qFilter, int sizeLimit)
        {  
            DirectorySearcher searcher = new DirectorySearcher(this.directoryEntry)
            {
                PageSize =  int.MaxValue,
                Filter = qFilter,
                SizeLimit = sizeLimit
            };

            Console.WriteLine($"[*] Query: {searcher.Filter}\n");

            try
            {
                SearchResultCollection result = null;

                if ( Config.Directory.Attrs.Count !=0 && Config.Directory.Attrs.First().Equals("meta",StringComparison.OrdinalIgnoreCase))
                {
                    var resultOne = searcher.FindOne();
                    if (resultOne != null)
                    {
                        foreach (var p in resultOne.Properties.PropertyNames)
                        {
                            Console.WriteLine($"{p}");
                        }
                    }

                    return;
                }

                result = searcher.FindAll();
                if (result != null)
                {
                    foreach (SearchResult sr in result)
                    {
                        Console.WriteLine($"{sr.Path}");
   
                        foreach (DictionaryEntry p in sr.Properties)
                        {
                            foreach( var d in p.Value as ResultPropertyValueCollection)
                            {
                                if (Config.Directory.Attrs.Count != 0)
                                {
                                    if (Config.Directory.Attrs.Any(s =>
                                        s.Equals(p.Key.ToString(), StringComparison.OrdinalIgnoreCase)))
                                    {
                                        printAttribute(p.Key.ToString(), d);
                                    }
                                }
                                else
                                {

                                    printAttribute(p.Key.ToString(), d);
                                }
                            }
                        }
                    }
                }

            }catch(Exception e)
            {
                Console.WriteLine($"Filter failed: {e.Message}");
            }

        }
    }
}

