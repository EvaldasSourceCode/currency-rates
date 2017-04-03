using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyRateChange.Service.Currency
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Models.CurrencyRateChange>> GetCurrencyRateChangesByDate(DateTime date);
    }
}
