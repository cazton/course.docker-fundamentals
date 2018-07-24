using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MovieCatalog.Web.Infrastructure.Documentation
{
    /// <summary>
    /// Swagger configuration helpers
    /// </summary>
    /// <remarks>
    /// https://swagger.io
    /// https://github.com/domaindrivendev/Swashbuckle
    /// </remarks>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Adds swagger generation to the application
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="provider">The api version provider</param>
        /// <param name="includeXmlComments">Indicates if xml comments are generated for assembly</param>
        public static void AddSwaggerGenVersions(this IServiceCollection services, IApiVersionDescriptionProvider provider, bool includeXmlComments = false)
        {
            services.AddSwaggerGen(options =>
            {
                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultsFilter>();

                // lowercase ri convention
                options.DocumentFilter<LowercaseDocumentFilter>();

                // model examples
                options.OperationFilter<ExamplesOperationFilter>();

                // Adds an Authorization input box to every endpoint
                // options.OperationFilter<SecurityRequirementsOperationFilter>();

                // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                options.OperationFilter<AddFileParamTypesOperationFilter>();

                // [SwaggerResponseHeader]
                options.OperationFilter<AddResponseHeadersFilter>();

                // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // integrate xml comments
                if (includeXmlComments)
                    options.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        /// <summary>
        /// Adds swagger UI to the application
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="provider">The api version provider</param>
        public static void UseSwaggerUIVersions(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        #region [ Helpers ]
        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = System.AppContext.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Contact = new Contact() { Name = "Contact", Email = "chris@cdinc.net" },
                Description = "API for the platform",
                License = new License() { Name = "Restricted" },
                TermsOfService = "None",
                Title = "Movies Catalog API",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
        #endregion

    }


}
