using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")] 
    public class GroupsController : Controller
    {

        private static long currentGroupId = 1;
        private static List<GroupViewModel> groups = new List<GroupViewModel>
        {
            new GroupViewModel { Id = 1, Name = "Sample Group N1" },
            new GroupViewModel { Id = 2, Name = "Sample Group N2" },
        };
        
        [HttpGet]
        [Route("")] //not needed because Index would be used as default anyway
        public IActionResult Index()
        {
            return View(groups);
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(long id)
        {
            var group = groups.SingleOrDefault(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }
    }
}