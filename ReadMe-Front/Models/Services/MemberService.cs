
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic.ApplicationServices;
using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
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

        public MemberService(MemberEFRepo memberRepo)
		{
			_repo = memberRepo;
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

		/// <summary>
		/// 取得會員個人資料
		/// </summary>
		/// <param name="account">會員帳號</param>
		/// <returns>ProfileVm</returns>
		public ProfileVm GetProfile(string account)
		{
			// 從資料庫取得會員資料
			var member = _repo.GetMemberByAccount(account);

			if (member == null)
			{
				throw new Exception("會員不存在");
			}

			// 將資料轉換為 ViewModel
			return new ProfileVm
			{
				Account = member.Account,
				Name = member.Name,
				Email = member.Email
			};
		}

		/// <summary>
		/// 更新會員個人資料
		/// </summary>
		/// <param name="account">會員帳號</param>
		/// <param name="model">ProfileVm</param>
		public void UpdateProfile(string account, ProfileVm model)
		{
			// 從資料庫取得會員資料
			var memberInDb = _repo.GetMemberByAccount(account);

			if (memberInDb == null)
			{
				throw new Exception("會員不存在");
			}

			// 更新資料
			memberInDb.Email = model.Email;
			memberInDb.Name = model.Name;

			// 儲存變更
			_repo.UpdateMember(memberInDb);
		}

		/// <summary>
		/// 修改密碼
		/// </summary>
		/// <param name="dto">包含修改密碼所需資料的 ChangePasswordDto</param>
		/// <param name="account">目前登入的使用者帳號</param>
		public void ChangePassword(ChangePasswordDto dto, string account)
		{
			// 從資料庫取得使用者
			var memberInDb = _repo.GetMemberByAccount(account);

			if (memberInDb == null)
			{
				throw new Exception("會員不存在");
			}

			// 驗證原密碼是否正確
			if (!VerifyPassword(dto.OrginalPassword, memberInDb.PasswordHash)) // 修正：使用 memberInDb
			{
				throw new Exception("原密碼錯誤");
			}

			// 確認新密碼與確認密碼是否一致
			if (dto.NewPassword != dto.ConfirmPassword)
			{
				throw new Exception("新密碼與確認密碼不一致");
			}

			// 使用雜湊工具加密新密碼
			memberInDb.PasswordHash = HashUtility.ToSHA256(dto.NewPassword, HashUtility.GetSalt()); // 修正：使用 memberInDb

			// 更新密碼到資料庫
			_repo.UpdateMemberPassword(memberInDb); // 修正：傳入 memberInDb
		}
		private bool VerifyPassword(string inputPassword, string storedPasswordHash)
		{
			return HashUtility.VerifySHA256(inputPassword, storedPasswordHash);
		}


	}
}