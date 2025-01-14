using System.ComponentModel.DataAnnotations;

namespace ReadMe_Back.Models.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "帳號必填")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼必填")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Id { get; set; }

    }
}
