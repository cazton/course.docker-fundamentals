using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MovieCatalog.Web.Models.Movies;

namespace MovieCatalog.Web.Models
{
    public static class MappingConfig
    {
        /// <summary>
        /// Adds the model automapper mappings
        /// </summary>
        /// <param name="services">The services collection</param>
        public static void AddModelMappings(this IServiceCollection services)
        {

            var config = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile(new MovieModel.MappingProfile());
                 cfg.AddProfile(new MovieCreateModel.MappingProfile());
                 cfg.AddProfile(new MovieUpdateModel.MappingProfile());
             });

            services.AddSingleton<IMapper>(sp => config.CreateMapper());
        }
    }
}
