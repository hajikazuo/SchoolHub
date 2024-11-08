using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Models.Dtos;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Common.Repositories.Interfaces;

namespace SchoolHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TennantController : ApiControllerBase
    {
        private readonly ITennantRepository _tennantRepository;

        public TennantController(
               ITennantRepository tennantRepository,
               ICombProvider comb,
               IMapper mapper) : base(comb, mapper)
        {
            _tennantRepository = tennantRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var tennants = await _tennantRepository.GetAllAsync();

            var response = _mapper.Map<List<TennantDto>>(tennants);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var tennant = await _tennantRepository.GetByIdAsync(id);

            if (tennant == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<TennantDto>(tennant);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TennantDto request)
        {
            var tennant = _mapper.Map<Tennant>(request);
            tennant.TennantId = _comb.Create();

            await _tennantRepository.CreateAsync(tennant);

            var response = _mapper.Map<TennantDto>(tennant);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, TennantDtoUpdate request)
        {
            var tennant = _mapper.Map<Tennant>(request);
            tennant.TennantId = id;

            var updateTennant = await _tennantRepository.UpdateAsync(tennant);

            if (updateTennant == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<TennantDto>(updateTennant);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var tennant = await _tennantRepository.DeleteAsync(id);

            if (tennant == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<TennantDto>(tennant);

            return Ok(response);
        }

    }
}
