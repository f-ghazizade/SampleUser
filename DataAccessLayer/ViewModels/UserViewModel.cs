using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "نام کاربری باید بین 6 تا 20 کاراکتر باشد.")]
        [RegularExpression("^[a-zA-Z0-9._-]+$", ErrorMessage = "نام کاربری فقط می ‌تواند شامل حروف، اعداد و کاراکترهای خاص باشد.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "رمز عبور باید بین 8 تا 20 کاراکتر باشد.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?""\:{}|<>]).*$", ErrorMessage = "رمز عبور باید شامل حروف بزرگ، حروف کوچک، اعداد و کاراکترهای خاص باشد.")]
        public required string Password { get; set; }
    }
}
