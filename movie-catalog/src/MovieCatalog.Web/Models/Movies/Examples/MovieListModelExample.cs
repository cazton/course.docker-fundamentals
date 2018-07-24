using MovieCatalog.Web.Models.Movies;
using Swashbuckle.AspNetCore.Examples;

namespace MovieCatalog.Web.Models.Movies.Examples
{
    public class MovieListModelExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new MovieModel[] {

                new MovieModel()
                {
                    Id = "507f1f77bcf86cd799439011",
                    Description = "Young Han Solo finds adventure when he joins a gang of galactic smugglers, including a 196-year-old Wookie named Chewbacca. ",
                    Title = "Solo: A Star Wars Story"
                },
                new MovieModel()
                {
                    Id = "508f1f77bcf86cd799439011",
                    Description = "Emmet (Chris Pratt), an ordinary LEGO figurine who always follows the rules, is mistakenly identified as the Special -- an extraordinary being and the key to saving the world. ",
                    Title = "The Lego Movie"
                },
                new MovieModel()
                {
                    Id = "509f1f77bcf86cd799439011",
                    Description = "The Lord of the Rings is a film series consisting of three high fantasy adventure films directed by Peter Jackson. ",
                    Title = "Lord of the Rings"
                }
            };

        }
    }
}
