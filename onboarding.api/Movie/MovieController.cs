using Microsoft.AspNetCore.Mvc;
using onboarding.api.Movie.DTO;
using onboarding.dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using onboarding.bll;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace onboarding.api.Movie
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private MovieService _movieService;

        private IMapper _mapper;

        public MovieController(MovieService movieService, IMapper mapper)
        {
            /*MapperConfiguration config = new MapperConfiguration(m =>
            {
                m.CreateMap<MovieDTO, MovieModel>();
                m.CreateMap<MovieModel, MovieDTO>();

                m.CreateMap<MovieWithNationalDTO, MovieModel>()
                    .ForMember(s => s.NationalId, d => d.MapFrom(t => t.Nation.Id))
                    .ForMember(s => s.Nation, opt => opt.Ignore());
                m.CreateMap<MovieModel, MovieWithNationalDTO>();
            });*/

            _movieService = movieService;
            _mapper = mapper;
            /*_mapper = config.CreateMapper();*/
        }



        // GET: api/<MovieController>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<MovieWithNationalDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult GetAllMovie()
        {
            var movie = _movieService.GetAll();
            var mapperResult = _mapper.Map<List<MovieWithNationalDTO>>(movie);
            return new OkObjectResult(mapperResult);
        }

        // GET api/<MovieController>/5
        [HttpGet]
        [Route("{imdbId}")]
        [ProducesResponseType(typeof(MovieDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult GetMovieById(int imdbId)
        {
            MovieModel movie = _movieService.GetMovieById(imdbId);
            var mapperResult = _mapper.Map<MovieDTO>(movie);
            return new OkObjectResult(mapperResult);
        }

        // POST api/<MovieController>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(MovieModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateMovie([FromBody] MovieModel value)
        {
            await _movieService.CreateMovie(value);
            /*var mapperResult = _mapper.Map<MovieDTO>(value);*/
            return new OkObjectResult(value);
        }

        // PUT api/<MovieController>/5
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(MovieModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> EditMovie([FromRoute] Guid id, [FromBody] MovieModel movie)
        {
            await _movieService.EditMovie(id, movie);
            
            return new OkObjectResult(movie);
        }

        // DELETE api/<MovieController>/5
        [HttpDelete]
        [Route("{title}")]
        [ProducesResponseType(typeof(MovieDTO), 200)]
        public async Task<ActionResult> Delete([FromRoute] string title)
        {
            await _movieService.DeleteMovie(title);
            return new OkResult();
        }
    }
}
