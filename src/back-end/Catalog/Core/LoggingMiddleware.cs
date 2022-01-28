using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Catalog.Core
{
    /// <summary>
    /// Inserts configured context keys in ILogger service scope.
    /// Includes its behavior in netcore pipelines before request execution.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Initialize a new instance of <see cref="LoggingMiddleware"/>
        /// </summary>
        /// <param name="next">Invoked request.</param>
        /// <param name="logger">Logger service instance.</param>
        /// <param name="options">Context keys set.</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Obtains the value of configured keys from HttpContext and inserts them in ILogger service scope.
        /// Includes its behavior in netcore pipelines before request execution.
        /// </summary>
        /// <param name="context">HTTP-specific information about an individual HTTP request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var scope = new List<KeyValuePair<string, object>>();

            using (_logger.BeginScope(scope.ToArray()))
            {
                await _next(context);
            }
        }
    }
}