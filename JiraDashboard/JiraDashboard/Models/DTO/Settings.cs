using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraDashboard.Models.DTO
{
    public class Settings
    {
        public int UserSearchLenght { get; set; }
        public int MaxResults { get; set; }
        public string StoryPointField { get; set; }
        public long RepeatInterval { get; set; }
    }
}