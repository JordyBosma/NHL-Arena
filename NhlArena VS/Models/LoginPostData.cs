using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NhlArena_VS.Models
{
    public class LoginPostData
    {

        [Display(Name = "Username")]
        public string username { get; set; }

        [Display(Name = "Password")]
        public string password { get; set; }
    }
}
