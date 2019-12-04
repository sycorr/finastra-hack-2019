using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FileHelpers;

namespace Finastra.Hackathon.NAICS
{
    public static class NAICSRepository
    {
        static NAICSRepository()
        {
            Reinitialize();
        }

        private static IEnumerable<NAICInformation> Data { get; set; }

        public static IQueryable<NAICInformation> All()
        {
            return Data.AsQueryable();
        }

        public static void Reinitialize()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finastra.Hackathon.NAICS.2017_NAICS.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var file = reader.ReadToEnd();
                    var data = new FileHelperEngine<NAICInformation>().ReadStringAsList(file);

                    var r = new Random();
                    foreach (var d in data)
                    {
                        d.InventoryTurnover = ((float) r.Next(3, 6)) + (float) r.NextDouble();
                        d.InventoryTurnDays = ((float) r.Next(50, 90)) + (float) r.NextDouble();

                        if (d.Code == "424490")
                        {
                            d.InventoryTurnover = 4.2f;
                            d.InventoryTurnDays = 74f;
                        }
                    }

                    Data = data;
                }
            };
        }
    }
}
