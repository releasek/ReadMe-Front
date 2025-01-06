using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.DTOs
{
	public class LoginDto
	{
		public string Account { get; set; }
		public string Password { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public bool IsBanned { get; set; }
	}
}