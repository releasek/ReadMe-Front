using Microsoft.AspNetCore.Authorization;

namespace ReadMe_Back.Models.Security
{
    public class FunctionRequirement : IAuthorizationRequirement
    {
        public FunctionRequirement(string functionId)
        {
            FunctionId = functionId.ToLower();
        }

        public string FunctionId { get; }
    }

    public class FunctionAuthorizeHandler : AuthorizationHandler<FunctionRequirement>
    {
		// 覆寫授權處理器的方法，用於自訂授權邏輯
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionRequirement requirement)
		{
			// 檢查目前使用者是否具有指定功能的 Claims（功能名稱會轉成小寫來比對）
			if (context.User.Claims.Any(c => c.Type.ToLower() == "function" && c.Value.ToLower() == requirement.FunctionId))
			{
				// 若使用者有此功能的權限，則授權通過
				context.Succeed(requirement);
			}

			// 授權處理結束（不論成功與否都需回傳已完成的 Task）
			return Task.CompletedTask;
		}

	}
}
