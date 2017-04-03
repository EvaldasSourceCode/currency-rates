namespace CurrencyRateChange.Service.Currency.RestClients.Configuration
{
    public interface IRestClientConfiguration
    {
        string Url { get; }
        string Username { get; }
        string Password { get; }
    }
}
