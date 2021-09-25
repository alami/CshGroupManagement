﻿using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")] 
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly SomeRootConfiguration _config;
        private readonly DemoSecretsConfiguration _secrets;

        public GroupsController(IGroupsService groupsService, SomeRootConfiguration config, DemoSecretsConfiguration secrets)
        {
            _groupsService = groupsService;
            _config = config;
            _secrets = secrets;
        }        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public IActionResult Index()
        {
            return View(_groupsService.GetAll().ToViewModel());
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(long id)
        {
            var group = _groupsService.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group.ToViewModel());
        }
        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, GroupViewModel model)
        {
            var group = _groupsService.Update(model.ToServiceModel());
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
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GroupViewModel model)
        {
            _groupsService.Add(model.ToServiceModel());
            
            return RedirectToAction("Index");
        }
    }
} 