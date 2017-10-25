using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HürriyetJiraDashboard.Models.JsonClasses
{
    public class Fields
    {
        public Assignee assignee { get; set; }
        public string resolutiondate { get; set; }
        public string created { get; set; }
        public Creator creator { get; set; }
        public Customfield StoryPoint { get; set; }
        public Issuetype issuetype { get; set; }
        public Resolution resolution { get; set; }
        public Status status { get; set; }
    }
}