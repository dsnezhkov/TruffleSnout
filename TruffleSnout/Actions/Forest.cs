using System;
using System.DirectoryServices.ActiveDirectory;

namespace TruffleSnout
{
    class BBForest
    {
        private Forest forest { get; set; }
        private DirectoryContext dCtx { get; set; }


        public BBForest()
        {

            try
            {
                forest = Forest.GetCurrentForest();
            }
            catch (Exception se)
            {

                Console.WriteLine($"Error getting Directory Context: {se.Message}");
            }
        }
        public BBForest(string ForestName)
        {
            try
            {
                dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName);
                forest = Forest.GetForest(dCtx);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting Directory Context: {e.Message}");
                return;
            }
        }
        public BBForest(string ForestName, string fUser, string fPass)
        {
            try
            {
                dCtx = new DirectoryContext(DirectoryContextType.Forest, ForestName, fUser, fPass);
                forest = Forest.GetForest(dCtx);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting Directory Context: {e.Message}");
                return;
            }
        }

        public Forest GetWorkingForest() { return forest; }
        public string GetWorkingForestName() { return forest.Name; }

        public void SetWorkingForest(Forest f)
        {
            forest = f;
        }
        private void showForestCatalogs(ref GlobalCatalogCollection gcc)
        {
            foreach (GlobalCatalog gc in gcc)
            {
                try
                {
                    Console.WriteLine("\t\tGlobalCatalog: {0} {1}, {2}", gc.Name, gc.IPAddress, gc.ToString());
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    Console.WriteLine("\t\tERROR: Getting IP: Host not reachable? {0}", se.Message);
                }
            }
        }
        private string GetFSMOMaster()
        {
            return forest.NamingRoleOwner.ToString();
        }
        private string GetForestMode()
        {
            string fMode;
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
            }
            catch (Exception e)
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
                Console.WriteLine("\t\tAD Site: {0} {1}", ads.Name, ads.Location);

                foreach (ActiveDirectorySiteLink asl in ads.SiteLinks)
                {
                    Console.WriteLine($"\t\tSite Link: {asl.Name}");
                }

                foreach (ActiveDirectorySubnet asn in ads.Subnets)
                {
                    Console.WriteLine($"\t\tSubnet: {asn.Name} ${asn.Location}");
                }

                foreach (var x in ads.AdjacentSites)
                {
                    Console.WriteLine($"\t\tAdjacent Site: {x}");
                }
                foreach (var x in ads.BridgeheadServers)
                {
                    Console.WriteLine($"\t\tBridgehead Server: {x}");
                }

            }
        }
        public void DiscoverForest()
        {
            if (forest != null)
            {
                Console.WriteLine("Forest Name: {0}", forest.Name);
                Console.WriteLine("\tForest Mode: {0}", GetForestMode());
                Console.WriteLine("\tForest FSMO Master DC: {0}", GetFSMOMaster());

                if (Utils.Config.Forest.GCs)
                {
                    GlobalCatalogCollection gcc = forest.FindAllDiscoverableGlobalCatalogs();
                    Console.WriteLine("\tForest GCs (Discoverable): ");
                    showForestCatalogs(ref gcc);

                    gcc = forest.FindAllGlobalCatalogs();
                    Console.WriteLine("\tForest GCs (All): ");
                    showForestCatalogs(ref gcc);
                }
                if (Utils.Config.Forest.Sites)
                {
                    Console.WriteLine("\tForest Sites: "); GetForestSites();
                }
                if (Utils.Config.Forest.Trusts)
                {
                    Console.WriteLine("\tForest Trust: "); GetTrustRelationships();
                }
                if (Utils.Config.Forest.Domains)
                {
                    Console.WriteLine("\tForest Root Domain: {0}", GetRootDomain());
                    Console.WriteLine("\tDomains in Forest:"); GetDomainsInForest();
                }
            }
            else
            {
                Console.WriteLine("Forest object not set");
            }
        }

        public void GetDomainsInForest()
        {
            Domain dRoot = forest.RootDomain;
            try
            {
                DomainCollection dc = forest.Domains;
                foreach (Domain d in dc)
                {
                    Console.WriteLine("\t\t{0} {1}", d.Name.Equals(dRoot.Name) ? "*" : "-", d.Name);
                }
            }
            catch (ActiveDirectoryOperationException e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            catch (ActiveDirectoryServerDownException e)
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
                Console.WriteLine("\t\tSource: {0}, Target: {1}, Direction: {2}, Trust Type: {3}",
                    tri.SourceName, tri.TargetName, tri.TrustDirection, tri.TrustType);
            }
        }

    }
}

