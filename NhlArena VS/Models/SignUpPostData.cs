using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NhlArena_VS.Models
{
    public class SignUpPostData
    {
        [Display(Name = "Username")]
        public string username { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Repeat password")]
        public string password2 { get; set; }
    }
}
