using System.Collections.Generic;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")] 
    public class GroupsController : Controller
    {

        private static long currentGroupId;
        private static List<GroupViewModel> groups = new List<GroupViewModel>
        {
            new GroupViewModel { Id = 1, Name = "Sample Group N1" },
            new GroupViewModel { Id = 1, Name = "Sample Group N2" }
        };
        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public IActionResult Index()
        {
            return View(groups);
        }
    }
}