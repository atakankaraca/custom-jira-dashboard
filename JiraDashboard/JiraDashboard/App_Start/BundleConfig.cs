using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace JiraDashboard.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/home/js").Include(
               "~/assets/js/jquery-3.2.1.min.js",
               "~/assets/js/tether.js",
               "~/assets/js/bootstrap.min.js",
               "~/assets/js/highcharts.js",
               "~/assets/js/moment.min.js",
               "~/assets/js/daterangepicker.js",
               "~/assets/js/jquery.dataTables.min.js",
               "~/assets/js/dataTables.bootstrap4.min.js",
               "~/assets/js/jquery.table2excel.js",
               "~/assets/js/loop.js"
               ));

            bundles.Add(new StyleBundle("~/bundles/home/css").Include(
               "~/assets/css/bootstrap.css",
               "~/assets/css/dataTables.bootstrap4.min.css",
               "~/assets/css/main.css",
               "~/assets/css/daterangepicker.css"
               ));

            bundles.Add(new StyleBundle("~/bundles/login/css").Include(
                "~/assets/login_css/bootstrap.css",
                "~/assets/login_css/custom.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                "~/assets/login_js/jquery.min.js",
                "~/assets/login_js/bootstrap.min.js"
                )
                );
        }
    }
}