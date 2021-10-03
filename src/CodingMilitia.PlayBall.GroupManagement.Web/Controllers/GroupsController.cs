using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    // [ServiceFilter(typeof(DemoExceptionFilter))]
    [DemoExceptionFilterFactoryAttribute]
    [Route("groups")] 
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupsService groupsService, ILogger<GroupsController> logger)
        {
            _groupsService = groupsService;
            _logger = logger;
        }        
        // [HttpGet]
        // [Route("")] //not needed because Index would be used as default anyway
        // public async Task<IActionResult> IndexAsync(CancellationToken ct)
        // {
        //     var result = await _groupsService.GetAllAsync(ct);
        //     return View(result.ToViewModel());
        // }
        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public IActionResult IndexAsync()
        {
            try
            {
                var result = _groupsService.GetAllAsync(CancellationToken.None).GetAwaiter().GetResult();
                // var result = _groupsService.GetAllAsync(CancellationToken.None).Result;
                return View(result.ToViewModel());
            }
            catch (NotImplementedException nex)
            {
                _logger.LogError(nex, "Not Implemented");
                return Content("Not Implemented");
            }
            catch (AggregateException aex)
            {
                _logger.LogError(aex, "Aggregate Exception");
                return Content("Aggregate Exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception type {exType}",ex.GetType());
                return Content("Kaaboom");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DetailsAsync(long id, CancellationToken ct)
        {
            var group = await _groupsService.GetByIdAsync(id,ct);
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
        public async Task<IActionResult> EditAsync(long id, GroupViewModel model, CancellationToken ct)
        {
            var group = await _groupsService.UpdateAsync(model.ToServiceModel(),ct);
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
        public async Task<IActionResult> CreateReallyAsync(GroupViewModel model, CancellationToken ct)
        {
            await _groupsService.AddAsync(model.ToServiceModel(),ct);
            
            return RedirectToAction("Index");
        }
    }
} 