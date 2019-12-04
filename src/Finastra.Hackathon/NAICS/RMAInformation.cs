using System;

namespace Finastra.Hackathon.NAICS
{
    public class RMAInformation
    {
        public Guid NAICId { get; set; }
        public DateTime Date { get; set; }
        public float? InventoryTurnover { get; set; }
        public float? InventoryTurnDays { get; set; }
    }
}