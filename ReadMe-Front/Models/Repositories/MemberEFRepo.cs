using Microsoft.Ajax.Utilities;
using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class MemberEFRepo
    {
            public void Create(RegisterDto dto)
            {
                using (var db = new AppDbContext())
                {
                    if (db.Users.Any(m => m.Account == dto.Account))
                        throw new Exception("帳號已存在");

                    var member = new User
                    {
                        Account = dto.Account,
                        Email = dto.Email,
                        PasswordHash = dto.PasswordHash, // 確保對應資料表的欄位名稱
                        IsBanned = false // 初始化必填欄位
                    };
                    db.Users.Add(member);
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

	}
}