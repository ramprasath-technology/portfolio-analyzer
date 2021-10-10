using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ExtensionMethods
{
    internal static class DecimalExtension
    {
        static int roundingFactor = 2;
        public static decimal RoundDecimal(this decimal input)
        {
            return decimal.Round(input, roundingFactor, MidpointRounding.AwayFromZero);
        }
    }
}
