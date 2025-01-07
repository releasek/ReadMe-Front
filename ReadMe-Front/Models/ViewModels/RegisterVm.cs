using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
    public class RegisterVm
    {
            [Display(Name = "帳號")]
            [Required(ErrorMessage = "{0}必填")]//制定錯誤訊息,{0}代表dispalyname以利維護errormessage
            [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]//{1}代表長度
            public string Account { get; set; }

		    [Display(Name = "姓名")]
		    [Required(ErrorMessage = "{0}必填")]//制定錯誤訊息,{0}代表dispalyname以利維護errormessage
		    [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]//{1}代表長度
		    public string Name { get; set; }

            [Display(Name = "密碼")]
            [Required(ErrorMessage = DAHealper.Require)]//Way2 DAHealper
            [StringLength(15, MinimumLength = 5, ErrorMessage = "{0}長度必須介於{2}~{1}")]
            [DataType(DataType.Password)]//不顯示密碼
            public string Password { get; set; }

            [Display(Name = "確認密碼")]
            [Required(ErrorMessage = "請輸入密碼")]
            [StringLength(15, ErrorMessage = "{0}長度不可超過{1}")]
            [DataType(DataType.Password)]//不顯示密碼
            [Compare("Password")]//驗證密碼
            public string ConfirmPassword { get; set; }

            [Display(Name = "電子郵件")]
            [Required(ErrorMessage = DAHealper.Require)]
            [StringLength(256, ErrorMessage = "{0}長度不可超過{1}")]
            [EmailAddress(ErrorMessage = "{0}格式有誤")]
            public string Email { get; set; }



    }
}