using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Infrastructure.BaseClasses
{
    public class BasePageModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _timer;

        public BasePageModel(ILogger logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _timer.Start();
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _timer.Stop();
            _logger.LogRoutePerformance(context.ActionDescriptor.RelativePath,
                context.HttpContext.Request.Method,
                _timer.ElapsedMilliseconds);
        }
    }
}
