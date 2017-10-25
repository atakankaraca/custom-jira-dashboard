using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraDashboard.Models.DTO
{
    public class TableModel
    {
        public string Assignee_Name { get; set; }
        public string Creator { get; set; }
        public double Story_Point { get; set; }
        public string Issue_ID { get; set; }
        public string Issue_Link { get; set; }
        public string Issue_Type { get; set; }
        public string Issue_Icon { get; set; }
        public string Resolution_Date { get; set; }
        public string Created_Date { get; set; }
        public int Efficiency_Point { get; set; }
        public string Status { get; set; }
        public string Status_Color_Code { get; set; }
    }
}