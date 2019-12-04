using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FileHelpers;
using Finastra.Hackathon.Accounting;
using Finastra.Hackathon.NAICS;

namespace Finastra.Hackathon
{
    public static class StaticData
    {
        static StaticData()
        {
            //--Bankers
            Lenders = new List<Identity>()
            {
                new Identity()
                {
                    Id = "12",
                    Username = "nsanders",
                    Name = "Nicky Sanders",
                    IsLender = true
                },
                new Identity()
                {
                    Id = "2b64c178-52ec-4c64-a776-a863c2223702",
                    Username = "asmith",
                    Name = "Arday Smith",
                    IsLender = true
                },
                new Identity()
                {
                    Id = "4dd87e63-ea30-4155-942a-59192cc5daff",
                    Username = "vpi",
                    Name = "Venkat Pi",
                    IsLender = true
                },
                new Identity()
                {
                    Id = "359cb3fb-be1a-4e06-ab05-68992df1917f",
                    Username = "dog",
                    Name = "Banker Dog",
                    IsLender = true
                }
            };

            BusinessOwner = new Identity()
            {
                Id = "4e2c50da-ca50-4bbe-8163-902c1859f92a",
                Username = "rrogerson",
                Name = "Ryan Rogerson",
                IsLender = false
            };

            //--AccountingPrinciples
            var adata =
                "Date,Cost of Goods,Inventory,Turn Over,Turn Days\r\n7/1/2016,\"75,000\",\"20,000\",3.75,96.00\r\n8/1/2016,\"75,000\",\"24,000\",3.13,115.20\r\n9/1/2016,\"75,000\",\"13,400\",5.60,64.32\r\n10/1/2016,\"75,000\",\"7,500\",10.00,36.00\r\n11/1/2016,\"75,000\",\"7,150\",10.49,34.32\r\n12/1/2016,\"75,000\",\"7,050\",10.64,33.84\r\n1/1/2017,\"75,000\",\"12,500\",6.00,60.00\r\n2/1/2017,\"75,000\",\"21,000\",3.57,100.80\r\n3/1/2017,\"75,000\",\"19,700\",3.81,94.56\r\n4/1/2017,\"75,000\",\"22,300\",3.36,107.04\r\n5/1/2017,\"75,000\",\"18,900\",3.97,90.72\r\n6/1/2017,\"75,000\",\"18,000\",4.17,86.40\r\n7/1/2017,\"75,000\",\"19,800\",3.79,95.04\r\n8/1/2017,\"75,000\",\"22,900\",3.28,109.92\r\n9/1/2017,\"75,000\",\"13,000\",5.77,62.40\r\n10/1/2017,\"75,000\",\"7,000\",10.71,33.60\r\n11/1/2017,\"75,000\",\"7,200\",10.42,34.56\r\n12/1/2017,\"75,000\",\"6,950\",10.79,33.36\r\n1/1/2018,\"75,000\",\"11,900\",6.30,57.12\r\n2/1/2018,\"75,000\",\"20,500\",3.66,98.40\r\n3/1/2018,\"75,000\",\"19,200\",3.91,92.16\r\n4/1/2018,\"75,000\",\"23,550\",3.18,113.04\r\n5/1/2018,\"75,000\",\"18,100\",4.14,86.88\r\n6/1/2018,\"75,000\",\"18,900\",3.97,90.72\r\n7/1/2018,\"75,000\",\"19,000\",3.95,91.20\r\n8/1/2018,\"75,000\",\"26,000\",2.88,124.80\r\n9/1/2018,\"75,000\",\"15,600\",4.81,74.88\r\n10/1/2018,\"75,000\",\"8,100\",9.26,38.88\r\n11/1/2018,\"75,000\",\"7,650\",9.80,36.72\r\n12/1/2018,\"75,000\",\"6,850\",10.95,32.88\r\n1/1/2019,\"75,000\",\"10,200\",7.35,48.96\r\n2/1/2019,\"75,000\",\"24,500\",3.06,117.60\r\n3/1/2019,\"75,000\",\"18,750\",4.00,90.00\r\n4/1/2019,\"75,000\",\"21,900\",3.42,105.12\r\n5/1/2019,\"75,000\",\"17,500\",4.29,84.00\r\n6/1/2019,\"75,000\",\"21,200\",3.54,101.76\r\n7/1/2019,\"75,000\",\"17,400\",4.31,83.52\r\n8/1/2019,\"75,000\",\"24,000\",3.13,115.20\r\n";

            AccountingPrinciples = new FileHelperEngine<RatioAnalysis>().ReadStringAsList(adata).ToList();

            //--NAICs and RMA
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finastra.Hackathon.NAICS.2017_NAICS.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var file = reader.ReadToEnd();
                    var data = new FileHelperEngine<NAICInformation>().ReadStringAsList(file);
                    var rma = new List<RMAInformation>();

                    var r = new Random();
                    foreach (var d in data)
                    {
                        d.Id = Guid.NewGuid();

                        var now = new DateTime(2019, 12, 1);
                        var date = new DateTime(2016, 7, 1);

                        while (date <= now)
                        {
                            var inventoryTurnover = ((float)r.Next(3, 6)) + (float)r.NextDouble();
                            var inventoryTurnDays = ((float)r.Next(50, 90)) + (float)r.NextDouble();

                            if (d.Code == "424490")
                            {
                                if (date.Year == 2019)
                                {
                                    inventoryTurnover = 4.2f;
                                    inventoryTurnDays = 74f;
                                }
                                else
                                {
                                    inventoryTurnover = 5.4f;
                                    inventoryTurnDays = 82f;
                                }
                            }

                            rma.Add(new RMAInformation()
                            {
                                Date = date,
                                InventoryTurnDays = inventoryTurnDays,
                                InventoryTurnover = inventoryTurnover,
                                NAICId = d.Id
                            });

                            date = date.AddMonths(1);
                        }
                    }

                    RMA = rma;
                    NAIC = data;
                }
            };
        }

        public static IEnumerable<NAICInformation> NAIC { get; private set; }
        public static IEnumerable<RMAInformation> RMA { get; private set; }
        public static IEnumerable<RatioAnalysis> AccountingPrinciples { get; private set; }
        public static IEnumerable<Identity> Lenders { get; private set; }
        public static Identity BusinessOwner { get; private set; }
    }
}
