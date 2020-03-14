using System;
using System.DirectoryServices.ActiveDirectory;

namespace TruffleSnout
{
    class BBDomain
    {
        private Domain domain { get; set; }
        private DirectoryContext dCtx { get; set; }


        public BBDomain()
        {
            try
            {
                domain = Domain.GetCurrentDomain();
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error getting Directory Context: {e.Message}");
                return;
            }
        }
        public BBDomain(string DomainName)
        {
            try
            {
                dCtx = new DirectoryContext(DirectoryContextType.Domain, DomainName);
                domain = Domain.GetDomain(dCtx);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error getting Directory Context: {e.Message}");
                return;
            }
        }
        public BBDomain(string DomainName, string dUser, string dPass)
        {
            try
            {
                dCtx = new DirectoryContext(DirectoryContextType.Domain, DomainName, dUser, dPass);
                domain = Domain.GetDomain(dCtx);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error getting Directory Context: {e.Message}");
                return;
            }
        }

        public Domain GetWorkingDomain() { return domain; }
        public string GetWorkingDomainName() { return domain.Name; }

        public void SetWorkingDomain(Domain d)
        {
            domain = d;
        }
        public void DiscoverDomain()
        {
            if (domain != null)
            {
                Console.WriteLine("Domain Name: {0}", domain.Name);
                Console.WriteLine("Domain Forest: {0}", domain.Forest.Name);
                Console.WriteLine("Domain Mode: {0}", domain.DomainMode);
                // 4.8
                // Console.WriteLine("Domain Mode Level: {0}", domain.DomainModeLevel);
                if (domain.Parent != null)
                {
                    Console.WriteLine("Domain Parent: {0}", domain.Parent);
                }
                else
                {
                    Console.WriteLine("Domain Parent: {0}", "None");
                }
                Console.WriteLine("Domain PDC: {0}", domain.PdcRoleOwner.ToString());
                Console.WriteLine("Domain RID Master: {0}", domain.RidRoleOwner.ToString());

                DomainControllerCollection dcDis = domain.FindAllDiscoverableDomainControllers();
                DomainControllerCollection dcAll = domain.FindAllDomainControllers();


                Console.WriteLine("All DCs: ");
                foreach (DomainController dc in dcAll)
                {

                    try
                    {
                        Console.WriteLine("\tDomainController : {0}, {1}, {2}, {3}",
                            dc.Name,
                            dc.OSVersion,
                            dc.SiteName, dc.IPAddress);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }

                }


                Console.WriteLine("All Discoverable DCs: ");
                foreach (DomainController dc in dcDis)
                {
                    try
                    {

                        Console.WriteLine("\tDomainController :\n" +
                            $"\t\tName: {dc.Name}\n" +
                            $"\t\tDomain: {dc.Domain}\n" +
                            $"\t\tForest: {dc.Forest}\n" +
                            $"\t\tTime: {dc.CurrentTime}\n" +
                            $"\t\tIP Address: {dc.IPAddress}\n" +
                            $"\t\tOS Version: {dc.OSVersion}\n" +
                            $"\t\tSite Name: {dc.SiteName}\n" +
                            $"\t\tIs GC?: {dc.IsGlobalCatalog()}"

                            );
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }
                    foreach (ReplicationConnection ircc in dc.InboundConnections)
                    {
                        try
                        {
                            Console.WriteLine("Inbound Replication: {0} {1} {2} {3} {4} {5}",
                                ircc.GetDirectoryEntry().Name,
                                ircc.SourceServer, ircc.TransportType, ircc.ReplicationSpan,
                                ircc.ReciprocalReplicationEnabled, ircc.DestinationServer
                                );
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR: {0}", e.Message);
                        }
                    }
                    foreach (ReplicationConnection orcc in dc.OutboundConnections)
                    {
                        try
                        {
                            Console.WriteLine("Outbound Replication: {0} {1} from: {2} {3} reciprocal: {4} to: {5}",
                                orcc.GetDirectoryEntry().Name,
                                orcc.SourceServer, orcc.TransportType, orcc.ReplicationSpan,
                                orcc.ReciprocalReplicationEnabled, orcc.DestinationServer
                                );
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR: {0}", e.Message);
                        }
                    }
                }

                if (domain.Children.Count != 0)
                {
                    Console.WriteLine("Domain Children: ");
                    foreach (Domain d in domain.Children)
                    {
                        Console.WriteLine("{0}", d.Name);
                    }
                }
                else
                {
                    Console.WriteLine("Domain Children: {0}", "None");
                }

                if (domain.GetAllTrustRelationships().Count != 0)
                {
                    Console.WriteLine("Domain Trusts: ");
                    foreach (TrustRelationshipInformation tri in domain.GetAllTrustRelationships())
                    {
                        Console.WriteLine("\t\tSource: {0}, Target: {1}, Direction: {2}, Trust Type: {3}",
                            tri.SourceName, tri.TargetName, tri.TrustDirection, tri.TrustType);
                    }
                }
                else
                {
                    Console.WriteLine("Domain Trusts: {0}", "None");

                }

            }
            else
            {
                Console.WriteLine("Domain object not set");
            }
        }
    }
}

