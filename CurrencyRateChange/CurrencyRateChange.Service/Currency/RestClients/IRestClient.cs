using System.Threading.Tasks;

namespace CurrencyRateChange.Service.Currency.RestClients
{
    public interface IRestClient
    {
        Task<TResponse> GetAsync<TResponse>(string relativeAddress, System.Threading.CancellationToken cancellationToken, bool allowRetries);
    }
}
