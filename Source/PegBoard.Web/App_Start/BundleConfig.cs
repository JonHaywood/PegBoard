using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PegBoard.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/lib/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/lib/angular.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/lib/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-ui").Include(
                        "~/Scripts/lib/ui-bootstrap-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/services.js",
                        "~/Scripts/app/controllers.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                        "~/Content/bootstrap*"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css"));
        }        
    }
}