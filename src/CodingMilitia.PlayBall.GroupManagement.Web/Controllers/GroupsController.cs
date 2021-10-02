using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    // [ServiceFilter(typeof(DemoExceptionFilter))]
    [DemoExceptionFilterFactoryAttribute]
    [Route("groups")] 
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public async Task<IActionResult> IndexAsync()
        {
            var result = await _groupsService.GetAllAsync();
            return View(result.ToViewModel());
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DetailsAsync(long id)
        {
            var group = await _groupsService.GetByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group.ToViewModel());
        }
        [DemoActionFilter]
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(long id, GroupViewModel model)
        {
            var group = await _groupsService.UpdateAsync(model.ToServiceModel());
            if (group == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }
        [DemoActionFilter]
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReallyAsync(GroupViewModel model)
        {
            await _groupsService.AddAsync(model.ToServiceModel());
            
            return RedirectToAction("Index");
        }
    }
} 