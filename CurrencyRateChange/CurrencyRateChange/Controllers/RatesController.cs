using CurrencyRateChange.Service.Currency;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace CurrencyRateChange.Controllers
{
    [RoutePrefix("rates")]
    public class RatesController : ApiController
    {
        private readonly ICurrencyService currencyService;

        public RatesController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [HttpGet]
        [Route("{date}")]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public async Task<IHttpActionResult> Rates(string date)
        {
            DateTime dateTime;

            if (DateTime.TryParse(date, out dateTime))
            {
                return BadRequest("Invalid date format");
            }

            var results = await currencyService.GetCurrencyRateChangesByDate(dateTime);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}