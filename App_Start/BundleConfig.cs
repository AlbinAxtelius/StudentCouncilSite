﻿using System.Web;
using System.Web.Optimization;

namespace ElevkårSida
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862  
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            // Jquery validator & unobstrusive ajax  
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.unobtrusive-ajax.js", "~/Scripts/jquery.unobtrusive-ajax.min.js", "~/Scripts/jquery.validate*", "~/Scripts/jquery.validate.unobtrusive.js", "~/Scripts/jquery.validate.unobtrusive.min.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're  
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.  
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
            // Custom Form  
            bundles.Add(new ScriptBundle("~/scripts/custom-form").Include("~/Scripts/custom-form.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));
        }
    }
}
