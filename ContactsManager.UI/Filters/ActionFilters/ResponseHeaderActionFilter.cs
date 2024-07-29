using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactAppManager.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        private readonly string Key;
        private readonly string value;
        public ResponseHeaderActionFilter(string key , string _value )
        {
            Key = key;
            value = _value;
        }



        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers[Key] = value;

            await next();

            context.HttpContext.Response.Headers[Key] = value+"ateya";
        }
    }
}
