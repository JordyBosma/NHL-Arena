using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NhlArena_VS.Models
{
    public class SignUpPostData : ValidationAttribute
    {
        [Required]
        [MinLength(3)]
        [StringLength(16)]
        [Display(Name = "Username")]
        public string username { get; set; }

        /*
        [Required]
        [EmailAddress]
        [MinLength(5)]
        [StringLength(40)]
        [Display(Name = "Email")]
        public string email { get; set; }
        */

        [Required]
        [StringLength(40)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [StringLength(40)]
        [Compare("password")]
        [Display(Name = "Repeat password")]
        public string password2 { get; set; }
    }
}
