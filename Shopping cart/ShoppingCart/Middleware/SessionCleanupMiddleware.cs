using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Middleware
{
    public class SessionCleanupMiddleware : IMiddleware
    {

        private readonly ILogger<SessionCleanupMiddleware> _logger;

        public SessionCleanupMiddleware(ILogger<SessionCleanupMiddleware> logger)
        {
            _logger = logger; //creates a logger, information can be seen in the VS debug console
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? currentSessionId = context.Session.GetString("sessionid");
            if (currentSessionId == null)
            {
                currentSessionId = Guid.NewGuid().ToString();
                context.Session.SetString("sessionid", currentSessionId);
            }
            // Check if the session ID has changed
            string? previousSessionId = context.Request.Cookies["PreviousSessionId"];
            string? username = context.Session.GetString("username");

            bool c2 = string.IsNullOrEmpty(previousSessionId);
            bool c3 = previousSessionId == currentSessionId;

            if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(previousSessionId) && !previousSessionId.Equals(currentSessionId))
            {
                //session id has changed
                _logger.LogInformation($"Session ID changed: PreviousSessionId={previousSessionId}, CurrentSessionId={currentSessionId}");
				// Delete the information in the SQL database associated with the previous session ID
				CartData.CleanupCart(previousSessionId);
            }

            // Set the current session ID as the previous session ID for the next request
            if (!string.IsNullOrEmpty(currentSessionId))
                context.Response.Cookies.Append("PreviousSessionId", currentSessionId);

            // Call the next middleware in the pipeline
            await next(context);
        }
    }
}
