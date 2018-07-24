using System.Collections.Generic;
using AutoMapper;
using MovieCatalog.Web.DataAccess.Entities;

namespace MovieCatalog.Web.Models.Movies
{
    /// <summary>
    /// Modesl movie metadata
    /// </summary>
    public class MovieUpdateModel
    {
        public MovieUpdateModel()
        {
        }

        /// <summary>
        /// The movie title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The movie description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The mapping profile
        /// </summary>
        internal class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Movie, MovieCreateModel>();
                CreateMap<MovieUpdateModel, Movie>();
            }
        }
    }
}
