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
                        "~/Scripts/jquery-{version}.js", "~/Scripts/materialize.js", "~/Scripts/jquery.datetimepicker.js", "~/Scripts/jQuery-autoComplete-master/jquery.auto-complete.js", "~/Scripts/BlockUI.js", "~/Scripts/Home/common.js"));
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
            "~/Scripts/moment.js", "~/Scripts/fullcalendar.js", "~/Scripts/locale/fr.js"));
            bundles.Add(new StyleBundle("~/bundles/materialise").Include("~/Content/fullcalendar.css", "~/Content/materialize.css", "~/Content/font-awesome-4.7.0/css/font-awesome.css", "~/Content/jquery.datetimepicker.css", "~/Scripts/jQuery-autoComplete-master/jquery.auto-complete.css"));

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