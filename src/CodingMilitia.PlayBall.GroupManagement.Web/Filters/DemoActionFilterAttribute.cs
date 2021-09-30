using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Filters
{
    public class DemoActionFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("model", out var model)
                && model is GroupViewModel group && group.Id == 0)
            {
                group.Name += $" ( Added on {nameof(DemoActionFilterAttribute)}";
            }

            
        }
    }
}