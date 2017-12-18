using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
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