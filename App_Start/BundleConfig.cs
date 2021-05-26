using System.Web;
using System.Web.Optimization;

namespace pfi
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/imageUploader").Include(
                        "~/Scripts/imageUploader.js"));

            bundles.Add(new ScriptBundle("~/bundles/imageUploader").Include(
                       "~/Scripts/RatingBar.js"));

            bundles.Add(new ScriptBundle("~/bundles/partialRefresh").Include(
                        "~/Scripts/partialRefresh.js"));

            bundles.Add(new ScriptBundle("~/bundles/selections").Include(
                        "~/Scripts/selections.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/flashButtons.css",
                      "~/Content/selections.css",
                      "~/Content/RatingBar.css",
                      "~/Content/styles.css",
                      "~/Content/tooltip.css",
                      "~/Content/site.css"));
        }
    }
}
