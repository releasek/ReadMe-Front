using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
	public class LoginVm
	{
		[Display(Name = "帳號")]
		[Required(ErrorMessage = DAHealper.Require)]//制定錯誤訊息,{0}代表dispalyname以利維護errormessage
		public string Account { get; set; }

		[Display(Name = "密碼")]
		[Required(ErrorMessage = DAHealper.Require)]
        [DataType(DataType.Password)]//不顯示密碼
        public string Password { get; set; }
	}
}