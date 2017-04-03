using System;

namespace CurrencyRateChange.Service.Currency.RestClients.Configuration
{
    public class LietuvosBankasRestClientConfiguration : IRestClientConfiguration
    {
        public string Url => "http://lb.lt/webservices/ExchangeRates/ExchangeRates.asmx/getExchangeRatesByDate";

        public string Username => String.Empty;

        public string Password => String.Empty;
    }
}
