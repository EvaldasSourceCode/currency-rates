using Ninject;
using CurrencyRateChange.Service.Currency.RestClients;
using CurrencyRateChange.Service.Currency.RestClients.Configuration;
using CurrencyRateChange.Service.Currency;

namespace CurrencyRateChange.Dependencies
{
    public class KernelSetup
    {
        public static void AddBindings(IKernel kernel)
        {
            kernel.Bind<IRestClientConfiguration>().To<LietuvosBankasRestClientConfiguration>();
            kernel.Bind<IRestClient>().To<RestClient>();
            kernel.Bind<ILietuvosBankasRestClient>().To<LietuvosBankasRestClient>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();
        }
    }
}