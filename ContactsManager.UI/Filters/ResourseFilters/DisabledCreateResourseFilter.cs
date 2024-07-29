using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactAppManager.Filters.ResourseFilters
{
    public class DisabledCreateResourseFilter : IAsyncResourceFilter
    {
        private readonly bool IsDisapled;

        public DisabledCreateResourseFilter(bool isDisapled = true)
        {
            IsDisapled = isDisapled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, 
            ResourceExecutionDelegate next)
        {
            if (IsDisapled)
            {
                context.Result = new NotFoundResult();
                return;
            }
            await next();
        }
    }
}
