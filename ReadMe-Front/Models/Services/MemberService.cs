using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ReadMe_Front.Models.Repositories.MemberEFRepo;

namespace ReadMe_Front.Models.Services
{
    public class MemberService
    {
        private MemberEFRepository _repo;

        public MemberService()
        {
            _repo = new MemberEFRepository();
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
    }
}