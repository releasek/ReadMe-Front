﻿using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using ReadMe_Front.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.UI.WebControls;
using static ReadMe_Front.Models.Repositories.MemberEFRepo;

namespace ReadMe_Front.Models.Services
{
    public class MemberService
    {
        private MemberEFRepo _repo;

        public MemberService()
        {
            _repo = new MemberEFRepo();
        }

        public void Register(RegisterDto dto)
        {

            //判斷帳號是否存在
            if (_repo.IsExists(dto.Account))
                throw new Exception("帳號已存在");
            //密碼加密
            dto.PasswordHash = HashUtility.ToSHA256(dto.Password, HashUtility.GetSalt());

            _repo.Create(dto);

        }
		public void ActivateMember(string memberAccount, bool isbanned)
		{
			// 從資料庫取得會員資料
			var member = _repo.GetMemberForActivation(memberAccount, isbanned);

			if (member == null)
			{
				// 如果找不到符合條件的會員，不執行任何動作
				return;
			}
		}

		public void Login ( LoginDto dto)
		{
			//判斷帳號是否存在
			if (_repo.IsExists(dto.Account))
			{
                //檢查帳號是否被停權
                if (_repo.IsBanned(dto.Account))
                {
					throw new Exception("此帳號已被停權");
				}
				else
                {
					Console.WriteLine("登入成功");
				}
			}
			else
			{
				throw new Exception("帳號不存在請重新註冊");
			}
		}
		public void ValidateLogin(string account, string password)
		{
			var member = _repo.GetUserByAccount(account); // 取得完整的 User 資料

			if (member == null)
				throw new Exception("帳號密碼錯誤");

			if (!HashUtility.VerifySHA256(password, member.PasswordHash)) // 驗證密碼
				throw new Exception("帳號密碼錯誤");

			if (!member.IsBanned==false) // 檢查是否已開通 (若 IsBanned 為 false，則尚未封禁)
				throw new Exception("帳號已停用");
		}

		public (string returnUrl, HttpCookie cookie) ProcessLogin(string account)
		{
			var roles = string.Empty; // 本範例中不使用角色權限 

			// 建立一張認證
			var ticket = new FormsAuthenticationTicket(
				1, // 版本別
				account, // 使用者名稱
				DateTime.Now, // 發行日
				DateTime.Now.AddDays(2), // 到期日
				false, // 是否續存
				roles, // 使用者資料
				"/" // Cookie 存放路徑
			);

			// 將認證票加密
			string encryptedTicket = FormsAuthentication.Encrypt(ticket);

			// 建立 Cookie
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

			// 取得 return URL
			var returnUrl = FormsAuthentication.GetRedirectUrl(account, true);

			return (returnUrl, cookie);
		}
	}
}