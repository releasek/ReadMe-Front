
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web;

namespace ReadMe_Back.Models.Security
{
    public class FunctionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _functionId;

        public FunctionAuthorizeAttribute(string functionId)
        {
            this._functionId = functionId.ToLower();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isApi = context.HttpContext.Request.Path.Value.Contains("/api/");

            // 找出原本要去的那一頁url
            string returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            returnUrl = HttpUtility.UrlEncode(returnUrl);

            string loginUrl = "/AdminUsers/Login?returnUrl=" + returnUrl;

            // 如果沒有登入，就導到登入頁
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (isApi)
                {
                    context.Result = new UnauthorizedResult(); // 401
                }
                else
                {
                    context.Result = new RedirectResult(loginUrl);
                }
                return;
            }

            // 判斷是否有權限
            var authService = context.HttpContext.RequestServices.GetService<IAuthorizationService>();

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new FunctionRequirement(_functionId))
                .Build();

            var result = authService.AuthorizeAsync(context.HttpContext.User, null, policy).Result;

            if (!result.Succeeded)
            {
                if (isApi)
                {
                    // 授權失敗,會回傳 403
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
                else
                {
                    // 授權失敗,會導向 access/denied
                    // context.Result = new ForbidResult();
                    context.Result = new RedirectResult("/AdminUsers/AccessDenied/");
                }
            }

        }
    }
}
