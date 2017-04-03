using System.Threading.Tasks;

namespace CurrencyRateChange.Service.Currency.RestClients
{
    public interface ILietuvosBankasRestClient
    {
        Task<string> GetExchangeRatesByDate(string date);
    }
}
