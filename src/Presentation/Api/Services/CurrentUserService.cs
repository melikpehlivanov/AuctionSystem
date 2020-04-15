namespace Api.Services
{
    using System.Security.Claims;
    using Application;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly bool? hasAdminClaim;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            this.IsAuthenticated = this.UserId != null;
            this.hasAdminClaim = httpContextAccessor.HttpContext?.User?.HasClaim(ClaimTypes.Role, AppConstants.AdministratorRole);
        }

        public string UserId { get; }

        public bool IsAuthenticated { get; }

        public bool IsAdmin => this.hasAdminClaim ?? false;
    }
}
