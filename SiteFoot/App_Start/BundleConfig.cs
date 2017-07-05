﻿using System.Web;
using System.Web.Optimization;

namespace SiteFoot
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.1.1.js", "~/Scripts/materialize.js"));
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
            "~/Scripts/moment.js", "~/Scripts/fullcalendar.js", "~/Scripts/locale/fr.js"));
            bundles.Add(new StyleBundle("~/bundles/materialise").Include("~/Content/fullcalendar.css","~/Content/materialize.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/common.css"));
        }
    }
}