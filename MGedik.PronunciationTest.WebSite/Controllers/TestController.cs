﻿using System.Web.Mvc;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            var selectedTestItem = TestContainer.GetNextTestItem();
            if (selectedTestItem == null)
                return RedirectToAction("TestResult", "Home");

            Session["selectedTestItem"] = selectedTestItem;
            return View(selectedTestItem);
        }
    }
}