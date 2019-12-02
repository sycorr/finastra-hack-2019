using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace Finastra.Hackathon.ML
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class RatioAnalysis
    {
        //[FieldConverter(typeof(ConvertDate))]
        [FieldConverter(ConverterKind.Date, "M/d/yyyy")]
        public DateTime Date { get; set; }

        [FieldConverter(ConverterKind.Single, ".")]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float CostOfGoods { get; set; }

        [FieldConverter(ConverterKind.Single, ".")]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float Inventory { get; set; }

        [FieldConverter(ConverterKind.Single, ".")]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float Turnover { get; set; }

        [FieldConverter(ConverterKind.Single, ".")]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float TurnDays { get; set; }

        [FieldHidden]
        public float CostOfGoodsDelta { get; set; }
        [FieldHidden]
        public float InventoryDelta { get; set; }
        [FieldHidden]
        public float TurnoverDelta { get; set; }
    }

    internal class ConvertDate : ConverterBase
    {

        /// <summary>
        /// different forms for date separator : . or / or space
        /// </summary>
        /// <param name="from">the string format of date - first the day</param>
        /// <returns></returns>

        public override object StringToField(string from)
        {
            return DateTime.Parse(from);
        }
    }
}
