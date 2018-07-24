using System.Collections.Generic;
using AutoMapper;
using MovieCatalog.Web.DataAccess.Entities;

namespace MovieCatalog.Web.Models.Movies
{
    /// <summary>
    /// A course
    /// </summary>
    public class MovieCreateModel
    {
        public MovieCreateModel()
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
                CreateMap<MovieCreateModel, Movie>();
            }
        }
    }
}
