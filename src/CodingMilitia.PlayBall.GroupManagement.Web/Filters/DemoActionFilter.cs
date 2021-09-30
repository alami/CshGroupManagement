using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Filters
{
    public class DemoActionFilter: IActionFilter
    {
        private readonly ILogger<DemoActionFilter> _logger;
        public DemoActionFilter(ILogger<DemoActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Before exec action {action} with arguments \"{@arguments}\" " +
                                   "and Model State \"{@modelState}\"",
                context.ActionDescriptor.DisplayName, 
                context.ActionArguments,
                context.ModelState);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("After  exec action {action}",context.ActionDescriptor.DisplayName);
        }
    }
}