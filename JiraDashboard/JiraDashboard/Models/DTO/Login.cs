using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JiraDashboard.Models.DTO
{
    public class Login
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Remember Me")]
        public bool rememberme { get; set; }
    }
}