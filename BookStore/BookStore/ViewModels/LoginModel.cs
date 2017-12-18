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
        public string userName { get ; set; }

        [Required(ErrorMessage = "Mời nhập password")]
        public string password { get ; set ; }
    }
}