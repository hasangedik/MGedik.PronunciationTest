using System.Web.Mvc;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            var selectedTestItem = TestContainer.GetNextTestItem();
            Session["selectedTestItem"] = selectedTestItem;
            return View(selectedTestItem);
        }
    }
}