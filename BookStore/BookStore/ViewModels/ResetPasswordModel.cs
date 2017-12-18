using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email không được trống")]
        public string email { get; set; }
    }
}