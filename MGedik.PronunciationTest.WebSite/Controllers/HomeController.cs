using System.Web.Mvc;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult TestResult()
        {
            ViewBag.Message = "Congratulations.";
            return View();
        }
    }
}