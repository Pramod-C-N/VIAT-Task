using Microsoft.AspNetCore.Mvc;
using vita.Web.Controllers;

namespace vita.Web.Public.Controllers
{
    public class HomeController : vitaControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}