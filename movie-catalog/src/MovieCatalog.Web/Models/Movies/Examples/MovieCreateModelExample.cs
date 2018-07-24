using MovieCatalog.Web.Models.Movies;
using Swashbuckle.AspNetCore.Examples;

namespace MovieCatalog.Web.Models.Movies.Examples
{
    public class MovieCreateModelExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new MovieCreateModel()
            {
                Description = "The Lord of the Rings is a film series consisting of three high fantasy adventure films directed by Peter Jackson. ",
                Title = "Lord of the Rings"
            };
        }
    }
}
