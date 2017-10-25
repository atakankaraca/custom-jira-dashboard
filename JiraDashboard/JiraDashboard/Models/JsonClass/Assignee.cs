using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HürriyetJiraDashboard.Models.JsonClasses
{
    public class Assignee
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
    }
}