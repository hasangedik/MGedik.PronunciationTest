using System.Web;
using System.Web.Optimization;

namespace MGedik.PronunciationTest.WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.min.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/audio").Include(
                "~/Scripts/audio/main.js",
                "~/Scripts/audio/recorderjs/recorder.js",
                "~/Scripts/audio/recorderjs/recorderWorker.js"));
        }
    }
}
