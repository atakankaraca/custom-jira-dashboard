using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JiraDashboard.Models.Helpers
{
    public class GenericHelper
    {
        public static string username => ConfigurationManager.AppSettings["username"].ToString();
        public static string password => ConfigurationManager.AppSettings["password"].ToString();
        public static string domain => ConfigurationManager.AppSettings["domain"].ToString();
        public static int maxresults => Convert.ToInt32(ConfigurationManager.AppSettings["maxresult"].ToString());
        public static int userquerylength => Convert.ToInt32(ConfigurationManager.AppSettings["searchuser"].ToString());
        public static long repeatinterval => Convert.ToInt32(ConfigurationManager.AppSettings["repeatinterval"].ToString());
        public static string storypointfield => ConfigurationManager.AppSettings["storypointfield"].ToString();
        public static string Done => ConfigurationManager.AppSettings["Done"].ToString();
        public static string RDF => ConfigurationManager.AppSettings["RDF"].ToString();
        public static string InProgress => ConfigurationManager.AppSettings["InProgress"].ToString();
        public static string ToDo => ConfigurationManager.AppSettings["ToDo"].ToString();
        public static string Live => ConfigurationManager.AppSettings["Live"].ToString();
        public static string ToBeDone => ConfigurationManager.AppSettings["ToBeDone"].ToString();
        public static string CodeReview => ConfigurationManager.AppSettings["CodeReview"].ToString();
    }
}