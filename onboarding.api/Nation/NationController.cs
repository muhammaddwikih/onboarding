using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using onboarding.api.Nation.DTO;
using onboarding.bll;
using onboarding.dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace onboarding.api.Nation
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationController : ControllerBase
    {
        private NationService _nationService;

        private IMapper _mapper;

        public NationController(NationService service, IMapper mapper)
        {
            _nationService = service;
            _mapper = mapper;
        }


        // GET: api/<NationController>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<NationWithMovieDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Get()
        {
            var nation = _nationService.GetAll();
            var mapperResult = _mapper.Map<List<NationWithMovieDTO>>(nation);
            return new OkObjectResult(mapperResult);
        }

        // GET api/<NationController>/5
        [HttpGet]
        [Route("{name}")]
        [ProducesResponseType(typeof(List<NationWithMovieDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            National nation = await _nationService.GetByName(name);
            if (nation != null)
            {
                var mapperResult = _mapper.Map<NationWithMovieDTO>(nation);
                return new OkObjectResult(mapperResult);
            }
            return new NotFoundResult();
        }

        // POST api/<NationController>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(National), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Post([FromBody] NationDTO value)
        {
            var mapperResult = _mapper.Map<National>(value);
            _nationService.CreateNation(mapperResult);
            return new OkObjectResult(mapperResult);
        }

        // PUT api/<NationController>/5
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(National), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] NationDTO nation)
        {
            var _mapperResult = _mapper.Map<National>(nation);
            await _nationService.EditNational(id, _mapperResult);

            return new OkObjectResult(_mapperResult);
        }

        // DELETE api/<NationController>/5
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(NationDTO), 200)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _nationService.Delete(id);
            return new OkResult();
        }
    }
}
