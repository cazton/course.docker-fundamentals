using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;

namespace MoviesCatalog.Web.Infrastructure.Config
{
    /// <summary>
    /// Configuration for applicatoin health checks (/health) endpoint
    /// </summary>
    public static class HealthCheckConfig
    {

        /// <summary>
        /// Adds application health checks
        /// </summary>
        /// <param name="services">The services collection</param>
        public static void AddHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                checks
                    // Basic check for running application
                    .AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });
        }
    }
}
