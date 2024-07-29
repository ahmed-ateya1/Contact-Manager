using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactAppManager.Filters.ResultFilters
{
    public class TokenAuthorizationResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, 
            ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Cookies.Append("API-KEY", "API100");
            await next();
        }
    }
}
