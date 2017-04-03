using System;
using System.Collections.Generic;

namespace CurrencyRateChange.Service.Currency.Comparers
{
    public class CurrencyEqualityComparer : IEqualityComparer<Models.Currency>
    {
        public bool Equals(Models.Currency x, Models.Currency y)
        {
            return x.CurrencyCode.Equals(y.CurrencyCode, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Models.Currency obj)
        {
            return obj.CurrencyCode.GetHashCode();
        }
    }
}
