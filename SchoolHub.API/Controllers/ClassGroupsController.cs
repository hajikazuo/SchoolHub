using AutoMapper;
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
    public class ClassGroupsController : ApiControllerBase
    {
        private readonly IClassGroupRepository _classGroupRepository;

        public ClassGroupsController(
               IClassGroupRepository classGroupRepository,
               ICombProvider comb,
               IMapper mapper) : base(comb, mapper)
        {
            _classGroupRepository = classGroupRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassGroup>>> GetAll()
        {
            var classGroups = await _classGroupRepository.GetAllAsync(this.TennantIdUserLoggedIn);

            var response = _mapper.Map<List<ClassGroupDto>>(classGroups);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var classGroup = await _classGroupRepository.GetByIdAsync(id);

            if (classGroup == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ClassGroupDto>(classGroup);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClassGroupDto request)
        {
            var tennantid = this.TennantIdUserLoggedIn;

            if (tennantid == default)
            {
                return Unauthorized("User does not have Tennant");
            }

            var classGroup = _mapper.Map<ClassGroup>(request);
            classGroup.ClassGroupId = _comb.Create();
            classGroup.TennantId = tennantid;

            await _classGroupRepository.CreateAsync(classGroup);

            var response = _mapper.Map<ClassGroupDto>(classGroup);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, ClassGroupDtoUpdate request)
        {
            var classGroup = _mapper.Map<ClassGroup>(request);
            classGroup.ClassGroupId = id;

            var updateClassGroup = await _classGroupRepository.UpdateAsync(classGroup);

            if (updateClassGroup == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ClassGroupDto>(updateClassGroup);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var classGroup = await _classGroupRepository.DeleteAsync(id);

            if (classGroup == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ClassGroupDto>(classGroup);

            return Ok(response);
        }
    }
}
