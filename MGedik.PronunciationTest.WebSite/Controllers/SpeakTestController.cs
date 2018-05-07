using System.Web.Mvc;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class SpeakTestController : Controller
    {
        public ActionResult Index()
        {
            var selectedTestItem = SpeakTestContainer.GetNextSpeakTestItem();
            if (selectedTestItem == null)
                return RedirectToAction("TestResult", "Home");

            Session["selectedSpeakTestItem"] = selectedTestItem;
            return View(selectedTestItem);
        }
    }
}