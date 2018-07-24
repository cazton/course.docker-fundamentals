using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Envoice.Conditions;
using Envoice.MongoRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using MovieCatalog.Web.DataAccess.Entities;
using MovieCatalog.Web.Models.Movies;
using MovieCatalog.Web.Models.Movies.Examples;

namespace MovieCatalog.Web.Controllers
{
    /// <summary>
    /// Controller for managing Movies.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private readonly IRepository<Movie> _MovieRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public MoviesController(
            IRepository<Movie> MovieRepository,
            ILoggerFactory loggerFactory,
            IMapper mapper
            )
        {
            Condition.Requires(MovieRepository, "MovieRepository").IsNotNull();
            Condition.Requires(loggerFactory, "loggerFactory").IsNotNull();
            Condition.Requires(mapper, "mapper").IsNotNull();

            _MovieRepository = MovieRepository;
            _logger = loggerFactory.CreateLogger<MoviesController>();
            _mapper = mapper;
        }

        /// <summary>
        ///  Returns list of movies
        /// </summary>
        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<MovieModel>))]
        [SwaggerOperation(operationId: "GetAllMovies")]
        [SwaggerResponseExample(200, typeof(MovieListModelExample), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> GetAll(int? skip = 0, int? take = 10)
        {
            var query = _MovieRepository.Collection.AsQueryable();

            if (skip > 0)
                query = query.Skip(skip.GetValueOrDefault());
            if (take > 0)
                query = query.Take(take.GetValueOrDefault());

            var Movies = await query.ToListAsync();
            var models = _mapper.Map<IEnumerable<MovieModel>>(Movies);
            return Ok(models);
        }

        /// <summary>
        /// Returns single movie by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(MovieModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerOperation(operationId: "FindMovie")]
        [SwaggerResponseExample(200, typeof(MovieModelExample), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> Find(string id)
        {
            var Movie = await _MovieRepository.GetByIdAsync(id);
            if (null == Movie)
            {
                return NotFound();
            }

            var model = _mapper.Map<MovieModel>(Movie);
            return Ok(model);
        }

        /// <summary>
        /// Creates a new movie
        /// </summary>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(MovieModel))]
        [SwaggerOperation(operationId: "CreateMovie")]
        [SwaggerResponseExample(200, typeof(MovieCreateModelExample), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> Create([FromBody] MovieCreateModel model)
        {
            var Movie = _mapper.Map<Movie>(model);
            await _MovieRepository.AddAsync(Movie);

            var result = _mapper.Map<MovieModel>(Movie);
            return Ok(result);
        }

        /// <summary>
        /// Updates a movie
        /// </summary>
        [HttpPost("{id}")]
        [ProducesResponseType(200, Type = typeof(MovieModel))]
        [SwaggerOperation(operationId: "UpdateMovie")]
        [SwaggerResponseExample(200, typeof(MovieUpdateModelExample), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> Update(string 
        id, [FromBody] MovieUpdateModel model)
        {
            Movie Movie = await _MovieRepository.GetByIdAsync(id);

            if (null == Movie)
            {
                return NotFound();
            }

            Movie = _mapper.Map<Movie>(model);
            await _MovieRepository.UpdateAsync(Movie);

            var result = _mapper.Map<MovieModel>(Movie);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a movie
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [SwaggerOperation(operationId: "DeleteMovie")]
        public async Task<IActionResult> Delete(string id)
        {
            await _MovieRepository.DeleteAsync(id); // idempotent
            return Ok();
        }
    }
}
