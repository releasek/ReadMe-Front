﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.DTOs
{
	public class RegisterDto
	{
		
		public string Account { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
		public string Name { get; set; }
			
		public string PasswordHash { get; set; }

	}
}