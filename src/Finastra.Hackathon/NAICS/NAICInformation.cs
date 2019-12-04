using System;
using FileHelpers;

namespace Finastra.Hackathon.NAICS
{
    [DelimitedRecord(",")]
    public class NAICInformation
    {
        [FieldHidden]
        public Guid Id { get; set; }
        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string Code { get; set; }
        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string Name { get; set; }
        
    }
}