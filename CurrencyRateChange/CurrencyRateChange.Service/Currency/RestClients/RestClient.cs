using CurrencyRateChange.Service.Currency.RestClients.Configuration;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyRateChange.Service.Currency.RestClients
{
    public class RestClient : HttpClient, IRestClient
    {
        private readonly IRestClientConfiguration configuration;
        
        private const int MaxRetryAttempts = 3;

        public RestClient(IRestClientConfiguration configuration)
        {
            DefaultRequestHeaders.ConnectionClose = false;
            DefaultRequestHeaders.Connection.Clear();
            DefaultRequestHeaders.Connection.Add("Keep-Alive");
            DefaultRequestHeaders.Accept.TryParseAdd("text/xml");           

            this.configuration = configuration;

            if (!String.IsNullOrEmpty(this.configuration.Username) && !String.IsNullOrEmpty(this.configuration.Password))
                UseAuthorization(this.configuration.Username, this.configuration.Password);

            SetBaseAddress(configuration.Url);
        }

        protected void UseAuthorization(string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var credentials =
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            String.Format(CultureInfo.InvariantCulture, "{0}:{1}", username, password)));

                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            }
        }       

        protected void SetBaseAddress(string baseAddress)
        {
            BaseAddress = new Uri(baseAddress);
        }        

        public async Task<TResponse> GetAsync<TResponse>(string relativeAddress, CancellationToken cancellationToken, bool allowRetries)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var address = new Uri(GetFullAddress(relativeAddress), UriKind.Absolute);

            using (var request = new HttpRequestMessage(HttpMethod.Get, address))
            {
                response =
                    await
                        SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                            .ConfigureAwait(false);
            }

            return await GetResponseContentAsync<TResponse>(response).ConfigureAwait(false);
        }      

        private string GetFullAddress(string relativeAddress)
        {
            var remoteAddress = configuration.Url ?? String.Empty;
            var address = new StringBuilder(remoteAddress);
            relativeAddress = relativeAddress ?? String.Empty;          

            // remove any leading slash characters in the relative address
            while (relativeAddress.Length > 1 && relativeAddress.FirstOrDefault() == '/')
                relativeAddress = relativeAddress.Substring(1);

            address.Append(relativeAddress);

            return address.ToString();
        }        

        private static async Task<TResponse> GetResponseContentAsync<TResponse>(HttpResponseMessage response)
        {
            if (response == null || response.Content == null)
            {
                return default(TResponse);
            }

            string result;
            try
            {
                await response.Content.LoadIntoBufferAsync().ConfigureAwait(false);

                using (response)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default(TResponse);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ReasonPhrase);
                    }

                    if (response.Content == null)
                    {
                        return default(TResponse);
                    }

                    result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException)
            {
                result = null;
            }

            return ReturnContent<TResponse>(result);
        }

        private static TDto ReturnContent<TDto>(string result)
        {
            if (String.IsNullOrEmpty(result))
                return default(TDto);            

            return (TDto)Convert.ChangeType(result, typeof(TDto));            
        }
    }   
}
