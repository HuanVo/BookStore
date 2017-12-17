using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ChangePassMode
    {
        [Required(ErrorMessage = "Password cũ không thể trống")]
        public string oldPass { get; set; }

        [Required(ErrorMessage = "Password mới không thể trống")]
        public string newPass { get; set; }

        [Required(ErrorMessage = "Xác nhận password không thể trống")]
        public string cfmPass { get; set; }
    }
}