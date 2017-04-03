using System.Web.Mvc;

namespace CurrencyRateChange.Controllers
{
    public class CurrencyRatesController : Controller
    {                
        public ActionResult Index()
        {
            return View("CurrencyRates");
        }       
    }
}