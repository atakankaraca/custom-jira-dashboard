using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HürriyetJiraDashboard.Models.JsonClasses
{
    public class History
    {
        public string id { get; set; }
        public Author author { get; set; }
        public string created { get; set; }
        public List<Item> items { get; set; }
    }
}