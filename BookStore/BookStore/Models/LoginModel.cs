using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời nhập user name")]
        public string UserName { get ; set; }

        [Required(ErrorMessage = "Mời nhập password")]
        public string Password { get ; set ; }


    }
}