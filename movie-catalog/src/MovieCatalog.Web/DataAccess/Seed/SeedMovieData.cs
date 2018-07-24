using System.Collections.Generic;
using System.Linq;
using MovieCatalog.Web.DataAccess.Entities;
using Envoice.MongoRepository;

namespace Edutacity.Lovano.DataAccess.Seed
{
    public class SeedMovieData
    {
        /// <summary>
        /// Seeds movie entities
        /// </summary>
        /// <param name="movieRepository">Instance of the movie repository</param>
        public static void Load(IRepository<Movie> movieRepository)
        {
            AddOrUpdateMovie(movieRepository, new Movie("The Big Lebowski")
            {
                Description = "Demo course one"
            });
        }

        /// <summary>
        /// Upserts a movie
        /// </summary>
        private static Movie AddOrUpdateMovie(IRepository<Movie> movieRepository, Movie movie)
        {
            var existing = movieRepository.SingleOrDefault(o => o.Title == movie.Title);
            if (existing != null)
            {
                existing.Description = movie.Description;
                movieRepository.Update(existing);
                return existing;
            }
            else
            {
                movieRepository.Add(movie);
                return movie;
            }
        }
    }
}
