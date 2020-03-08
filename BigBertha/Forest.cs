using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using BigBertha.Utils;

namespace BigBertha
{
    class BBForest
    {
        private Forest forest { get; set; }
        private DirectoryContext dCtx { get; set; }

        
        public BBForest()
        {
            forest = Forest.GetCurrentForest();
        }
        public BBForest(string ForestName)
        {
            dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName);
            forest = Forest.GetForest(dCtx);
        }
        public BBForest(string ForestName, string fUser, string fPass)
        {
            dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName, fUser, fPass);
            forest = Forest.GetForest(dCtx);
        }

        public Forest GetWorkingForest()
        {
            return forest; 
        }
        public void SetWorkingForest(Forest f)
        {
            forest = f;
        }
        private void showForestCatalogs(ref GlobalCatalogCollection gcc)
        {
            foreach (GlobalCatalog gc in gcc)
            {
                Console.WriteLine("\tGlobalCatalog: {0} {1}, {2}", gc.Name, gc.IPAddress, gc.ToString());
            }
        }
        private string GetFSMOMaster()
        {
            return forest.NamingRoleOwner.ToString();
        }
        private string GetForestMode()
        {
            string fMode = "unknown";
            ForestMode fm;

            try
            {
                fm = forest.ForestMode;
                switch (forest.ForestMode)
                {
                    case ForestMode.Windows2000Forest:
                        {
                            fMode = "w2k";
                            break;
                        }
                    case ForestMode.Windows2003InterimForest:
                        {
                            fMode = "w2k3Interim";
                            break;
                        }
                    case ForestMode.Windows2003Forest:
                        {
                            fMode = "w2k3";
                            break;
                        }
                    case ForestMode.Windows2008Forest:
                        {
                            fMode = "w2k8";
                            break;
                        }
                    case ForestMode.Windows2008R2Forest:
                        {
                            fMode = "w2k8r2";
                            break;
                        }
                    case ForestMode.Windows8Forest:
                        {
                            fMode = "w8";
                            break;
                        }
                    default:
                        {
                            fMode = "unknown";
                            break;
                        }
                }
            }catch(Exception e)
            {
                Console.WriteLine("!!! Forest mode Exception: {0}", e.Message);
                return "N/A";
            }
            return fMode;
        }

        private string GetRootDomain()
        {
            return forest.RootDomain.Name;
        }

        private void GetForestSites()
        {
            foreach (ActiveDirectorySite ads in forest.Sites)
            {
                Console.WriteLine("\tAD Site: {0} {1}", ads.Name, ads.Location);
            }
        }
        public void DiscoverForest()
        {
            Console.WriteLine("Forest: {0}", forest.Name);

            GlobalCatalogCollection gcc = forest.FindAllDiscoverableGlobalCatalogs();
            Console.WriteLine("Forest GCs (Discoverable): ");
            showForestCatalogs(ref gcc);

            gcc = forest.FindAllGlobalCatalogs();
            Console.WriteLine("Forest GCs (All): ");
            showForestCatalogs(ref gcc);

            Console.WriteLine("Forest mode: {0}",GetForestMode());
            Console.WriteLine("Forest FSMO Master DC: {0}", GetFSMOMaster());
            Console.WriteLine("Forest Root Domain: {0}", GetRootDomain());
            Console.WriteLine("Forest Sites: "); GetForestSites();
            Console.WriteLine("Forest Trust: "); GetTrustRelationships();

        }
        /*public void DiscoverCurrentForest()
        {
            forest = Forest.GetCurrentForest();
            DiscoverForest(forest);
        }


        public void DiscoverForestByName(string ForestName)
        {
            DirectoryContext dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName);
            DiscoverForest(dCtx);
        }
        public void DiscoverForestByNameCreds(string ForestName, string fUser, string fPass)
        {
            DirectoryContext dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName, fUser, fPass);
            DiscoverForest(dCtx);
        }*/


        public void DiscoverDomainsInForest()
        {
            Domain dRoot = forest.RootDomain;
            try
            {
                DomainCollection dc = forest.Domains;
                foreach ( Domain d in dc)
                {
                    Console.WriteLine("{0} {1}", d.Name.Equals(dRoot.Name) ? "*": "-" , d.Name);
                }
            }catch(ActiveDirectoryOperationException e)
            {
                Console.WriteLine("Error: {0}", e);
            }catch (ActiveDirectoryServerDownException e)
            {
                Console.WriteLine("Server Down Error: {0}", e);
            }
        }

        public void GetTrustRelationships()
        {
            /*ForestTrustRelationshipInformation ftri =  forest.GetTrustRelationship("other.int");
                Console.WriteLine("\t Source: {0}, Target: {1}, Direction: {2}, Trust Type: {3}", 
                    ftri.SourceName, ftri.TargetName, ftri.TrustDirection, ftri.TrustType);  */
            foreach (TrustRelationshipInformation tri in forest.GetAllTrustRelationships())
            {
                Console.WriteLine("\t Source: {0}, Target: {1}, Direction: {2}, Trust Type: {3}", 
                    tri.SourceName, tri.TargetName, tri.TrustDirection, tri.TrustType);
            }
        }

    }
}
