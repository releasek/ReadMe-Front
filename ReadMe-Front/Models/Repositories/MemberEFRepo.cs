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
        public class MemberEFRepository
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
        }
    }
}