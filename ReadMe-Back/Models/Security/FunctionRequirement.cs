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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionRequirement requirement)
        {
            if (context.User.Claims.Any(c => c.Type.ToLower() == "function" && c.Value.ToLower() == requirement.FunctionId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
