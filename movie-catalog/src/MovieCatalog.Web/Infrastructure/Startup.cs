using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edutacity.Lovano.DataAccess.Seed;
using Envoice.MongoRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieCatalog.Web.DataAccess.Entities;
using MovieCatalog.Web.Infrastructure.Documentation;
using MovieCatalog.Web.Infrastructure.Middleware;
using MovieCatalog.Web.Models;

namespace MovieCatalog.Web.Infrastructure
{
    public class Startup
    {
        protected IConfiguration Configuration { get; }
        protected IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the whole configuration object here.
            services.AddSingleton<IConfiguration>(Configuration);

            // Sevices
            var connectionString = this.Configuration.GetConnectionString("Entities");
            var repositoryConfig = new MongoRepositoryConfig(connectionString);
            services.TryAddSingleton<MongoRepositoryConfig>(repositoryConfig);
            services.TryAddScoped<IRepository<Movie>, MongoRepository<Movie>>();

            // Mvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Versioning
            services.AddMvcCore().AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Versioned swagger generator
            var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGenVersions(apiVersionDescriptionProvider);

            // Model mappings (automapper)
            services.AddModelMappings();
        }

        /// <summary>
        /// Configures the application runtime
        /// </summary>
        public void Configure(
            IApplicationBuilder app,
            IApiVersionDescriptionProvider versionDescriptionProvider,
            IApplicationLifetime applicationLifetime,
            IRepository<Movie> movieRepository,
            IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionMiddleware(hostingEnvironment, loggerFactory).Invoke
            });

            app.UseMvc();
            // app.UseAuthentication();movie
            app.UseSwagger();
            app.UseSwaggerUIVersions(versionDescriptionProvider);

            // seed entities
            SeedMovieData.Load(movieRepository);
        }
    }
}
