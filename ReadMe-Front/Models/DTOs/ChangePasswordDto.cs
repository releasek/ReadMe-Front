using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.DTOs
{
	public class ChangePasswordDto
	{
		public string OrginalPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
