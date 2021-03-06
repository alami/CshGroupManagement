using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [ApiController]
    [Route("groups")] 
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var result = await _groupsService.GetAllAsync(ct);
            return Ok(result.ToModel());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken ct)
        {
            var group = await _groupsService.GetByIdAsync(id,ct);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.ToModel());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, GroupModel model, CancellationToken ct)
        {
            model.Id = id;
            var group = await _groupsService.UpdateAsync(model.ToServiceModel(),ct);
            return Ok(group.ToModel());
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(GroupModel model, CancellationToken ct)
        {
            model.Id = 0;
            var group = await _groupsService.AddAsync(model.ToServiceModel(),ct);
            return Ok(group.ToModel());//CreatedAtAction(nameof(GetByIdAsync), new { id=group.Id }, group);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveAsync(long id, CancellationToken ct)
        {
            await _groupsService.RemoveAsync(id,ct);
            return NoContent() ;
        }
        [HttpOptions]
        [Route("{id}")]
        public async Task<IActionResult> OptionsAsync(long id, CancellationToken ct)
        {
            return Ok() ;
        }
    }
} 