using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pfi.Models
{
    public class LoginView
    {
        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom d'usager")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}