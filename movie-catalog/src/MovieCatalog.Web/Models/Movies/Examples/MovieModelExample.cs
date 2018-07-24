using MovieCatalog.Web.Models.Movies;
using Swashbuckle.AspNetCore.Examples;

namespace MovieCatalog.Web.Models.Movies.Examples
{
    public class MovieModelExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new MovieModel()
            {
                Id = "507f1f77bcf86cd799439011",
                Description = "Young Han Solo finds adventure when he joins a gang of galactic smugglers, including a 196-year-old Wookie named Chewbacca. ",
                Title = "Solo: A Star Wars Story"
            };
        }
    }
}
