using Microsoft.Ajax.Utilities;
using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class MemberEFRepo
    {
		private readonly AppDbContext _context;
		public MemberEFRepo(AppDbContext context)
		{
			_context = context;
		}

		public void Create(RegisterDto dto)
         {
                using (var db = new AppDbContext())
                {
                    if (db.Users.Any(m => m.Account == dto.Account))
                        throw new Exception("帳號已存在");

                    var user = new User
                    {
                        Account = dto.Account,
                        Email = dto.Email,
						Name = dto.Name,
                        PasswordHash = dto.PasswordHash, // 確保對應資料表的欄位名稱
                        IsBanned = false // 初始化必填欄位
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
         }
            public bool IsExists(string account)
            {
                using (var db = new AppDbContext())
                {
                    return db.Users.Any(m => m.Account == account);
                }
            }

		    public bool IsBanned(string account)
		    {
			    using (var db = new AppDbContext())
			    {
				var user = db.Users.FirstOrDefault(u => u.Account == account);
				return user != null && user.IsBanned; // 如果使用者存在且 IsBanned 為 true，返回 true
			    }
		    }

		    public User GetUserByAccount(string account)
		    {
			    using (var db = new AppDbContext())
			    {
				    return db.Users.FirstOrDefault(m => m.Account == account);
			    }
		    }

			// 根據 memberId 和 confirmCode 查詢會員資料
			public User GetMemberForActivation(string memberAccount,bool isbanned)
			{
				using (var db = new AppDbContext())
				{
					return db.Users.FirstOrDefault(m =>
						m.Account == memberAccount &&
						m.IsBanned == true // 僅停權的帳號需要啟用
					);
				}
			}

		/// <summary>
		/// 根據帳號取得會員資料
		/// </summary>
		/// <param name="account">會員帳號</param>
		/// <returns>User</returns>
		public User GetMemberByAccount(string account)
		{
			using (var db = new AppDbContext())
			{
				return db.Users.FirstOrDefault(m => m.Account == account);
			}
		}

		/// <summary>
		/// 更新會員資料
		/// </summary>
		/// <param name="member">會員資料</param>
		public void UpdateMember(User user)
		{
			_context.Entry(user).State = EntityState.Modified;
			_context.SaveChanges();
		}

		/// <summary>
		/// 更新使用者密碼
		/// </summary>
		/// <param name="user">使用者實體</param>
		public void UpdateMemberPassword(User user)
		{
			_context.Entry(user).State = EntityState.Modified;
			_context.SaveChanges();
		}




	}
}