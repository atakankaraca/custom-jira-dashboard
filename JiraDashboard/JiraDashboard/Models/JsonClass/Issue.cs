using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HürriyetJiraDashboard.Models.JsonClasses
{
    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Changelog changelog { get; set; }
        public Fields fields { get; set; }
    }
}