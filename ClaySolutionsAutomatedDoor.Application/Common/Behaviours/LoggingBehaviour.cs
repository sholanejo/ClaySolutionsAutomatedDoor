using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace ClaySolutionsAutomatedDoor.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingBehaviour(ILogger<TRequest> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            string userName = string.Empty;

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null && httpContext.User.Identity.IsAuthenticated)
            {
                var claimIdentity = (ClaimsIdentity)httpContext.User.Identity;
                var claim = claimIdentity.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email);
                userName = claim?.Value ?? string.Empty;
            }

            var headers = httpContext?.Request.Headers.ToDictionary(k => k.Key, v => v.Value.ToString());

            _logger.LogInformation("Request: {Name} {@Request} {TraceId} {Headers}", requestName, request, Activity.Current?.Id, headers);
            return await next();
        }


    }
}
