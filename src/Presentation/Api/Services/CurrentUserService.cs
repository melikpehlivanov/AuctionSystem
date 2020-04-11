namespace Api.Services
{
    using System.Security.Claims;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            this.IsAuthenticated = this.UserId != null;
        }

        public string UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
