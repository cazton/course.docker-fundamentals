using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MovieCatalog.Web.Infrastructure.Middleware
{
    public class JsonExceptionMiddleware
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<JsonExceptionMiddleware> _logger;

        /// <summary>
        /// Models a global error response
        /// </summary>
        public class ErrorResponse
        {
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }

        public JsonExceptionMiddleware(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = loggerFactory?.CreateLogger<JsonExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception == null) return;

            var response = new ErrorResponse()
            {
                Message = exception.Message,
                StackTrace = (!this._hostingEnvironment.IsProduction()) ? exception.StackTrace : null
            };

            context.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                new JsonSerializer().Serialize(writer, response);
                await writer.FlushAsync().ConfigureAwait(false);
            }

            _logger.LogError(exception, exception.Message);
        }
    }
}
