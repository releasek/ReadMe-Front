using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadMe_Front.Models.ViewModels;
using System.Web.Security;
using ReadMe_Front.Models.Infra;
using System.Web.UI.WebControls;

namespace ReadMe_Front.Controllers
{
	public class MembersController : Controller
	{
		private readonly MemberService _memberService;

		public MembersController()
		{
			_memberService = new MemberService();
		}

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
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
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
		/// <summary>
		/// 啟用帳號
		/// /Members/ActiveRegister?memberId=4&confirmCode=6a9024f67d314ed49464ff20a71da072
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="confirmCode"></param>
		/// <returns></returns>
		public ActionResult ActiveRegister(string memberAccount, bool isbanned)
		{
			_memberService.ActivateMember(memberAccount, isbanned);
			return View();
		}

		public ActionResult Login(LoginVm model)
		{
			if (!ModelState.IsValid) return View(model);

			try
			{
				// 驗證登入
				_memberService.ValidateLogin(model.Account, model.Password);

				// 處理登入成功後的 Cookie 和 URL
				(string returnUrl, HttpCookie cookie) = _memberService.ProcessLogin(model.Account);

				// 設定 Cookie
				Response.Cookies.Add(cookie);

				// 導向 returnUrl
				return Redirect(returnUrl);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}

		[Authorize]

		public ActionResult EditProfile()
		{
			// 取得目前登入會員的帳號
			string account = User.Identity.Name;

			// 從 Service 層取得會員資料
			var model = _memberService.GetProfile(account);

			return View(model);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(ProfileVm model)
		{
			if (!ModelState.IsValid)
			{
				return View(model); // 若驗證失敗，返回 View 顯示錯誤
			}

			// 取得目前登入會員的帳號
			string account = User.Identity.Name;

			// 更新會員資料
			_memberService.UpdateProfile(account, model);

			TempData["Message"] = "個人資料已更新";
			return RedirectToAction("Index");
		}
	}
}