using MovieCatalog.Web.Models.Movies;
using Swashbuckle.AspNetCore.Examples;

namespace MovieCatalog.Web.Models.Movies.Examples
{
    public class MovieUpdateModelExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new MovieUpdateModel()
            {
                Description = "Emmet (Chris Pratt), an ordinary LEGO figurine who always follows the rules, is mistakenly identified as the Special -- an extraordinary being and the key to saving the world. ",
                Title = "The Lego Movie"
            };
        }
    }
}
