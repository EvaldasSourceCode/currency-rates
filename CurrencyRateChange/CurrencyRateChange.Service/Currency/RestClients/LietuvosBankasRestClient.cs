using CurrencyRateChange.Service.Currency.RestClients.Configuration;
using System.Threading.Tasks;
using System.Threading;

namespace CurrencyRateChange.Service.Currency.RestClients
{
    public class LietuvosBankasRestClient : RestClient, ILietuvosBankasRestClient
    {
        public LietuvosBankasRestClient(IRestClientConfiguration configuration) : base(configuration)
        {
        }

        public async Task<string> GetExchangeRatesByDate(string date)
        {
            return await GetAsync<string>($"?Date={date}", CancellationToken.None, true);
        }
    }
}
