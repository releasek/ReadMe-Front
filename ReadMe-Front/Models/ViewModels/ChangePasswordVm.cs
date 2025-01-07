using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
	public class ChangePasswordVm
	{
		[Display(Name = "密碼")]
		[Required(ErrorMessage = DAHealper.Require)]//Way2 DAHealper
		[StringLength(15, MinimumLength = 5, ErrorMessage = "請輸入舊密碼")]
		[DataType(DataType.Password)]//不顯示密碼

		public string OrginalPassword { get; set; }

		[Display(Name = "新密碼")]
		[Required(ErrorMessage = DAHealper.Require)]
		[StringLength(15, ErrorMessage = "{0}長度不可超過{1}")]
		[DataType(DataType.Password)]//不顯示密碼
		public string NewPassword { get; set; }

		[Display(Name = "再次確認新密碼")]
		[Required(ErrorMessage = DAHealper.Require)]
		[StringLength(15, ErrorMessage = "{0}長度不可超過{1}")]
		[DataType(DataType.Password)]//不顯示密碼
		[Compare("NewPassword", ErrorMessage = "確認密碼與新密碼不一致")]//驗證密碼
		public string ConfirmPassword { get; set; }
	}
}