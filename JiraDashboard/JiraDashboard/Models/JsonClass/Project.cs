﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HürriyetJiraDashboard.Models.JsonClasses
{
    public class Project
    {
        public string expand { get; set; }
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string projectTypeKey { get; set; }
    }
}