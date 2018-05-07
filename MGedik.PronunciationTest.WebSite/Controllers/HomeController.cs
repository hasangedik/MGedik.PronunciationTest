using System.Web.Mvc;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult TestResult()
        {
            ViewBag.Message = "Congratulations.";
            return View();
        }
    }
}