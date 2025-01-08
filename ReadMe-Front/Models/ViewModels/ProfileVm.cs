using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
	public class ProfileVm
	{
		[Display(Name = "帳號")]
		public string Account { get; set; }
		[Display(Name = "信箱")]
		public string Email { get; set; }
		[Display(Name = "姓名")]
		public string Name { get; set; }
	}
}