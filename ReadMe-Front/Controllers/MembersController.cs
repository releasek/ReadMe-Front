using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadMe_Front.Models.ViewModels;
using System.Web.Security;
using ReadMe_Front.Models.Infra;

namespace ReadMe_Front.Controllers
{
    public class MembersController : Controller
    {

        [Authorize]
        // Get: Index 會員中心頁
        public ActionResult Index()
        {
            return View();
        }
        // GET: Members 會員註冊頁
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVm model)
        {
            //todo
            if (!ModelState.IsValid) return View(model);
            try
            {
                ProcessRegister(model);

                //建檔成功導向確證頁
                return View("RegisterConfirm");
                //return RedirectToAction("Registerfirm")
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("此帳號已註冊過請重新登入", ex.Message);
                return View(model);
            }

        }
        /// <summary>
        /// 在此直接叫用EF
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="Exception"></exception>
        private void ProcessRegister(RegisterVm model)
        {
            RegisterDto dto = new RegisterDto
            {
                Account = model.Account,
                Email = model.Email,
                Password = model.Password,
   
            };
            new MemberService().Register(dto);
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                ProcessLogin(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("登入失敗", ex.Message);
                return View(model);
            }
        }

        private void ProcessLogin(LoginVm model)
        {
            throw new NotImplementedException();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}