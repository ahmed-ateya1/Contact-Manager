using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContract.DTO;
using ContactAppManager.Controllers;

namespace ContactAppManager.Filters.ActionFilters
{
    public class PersonsListActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            PersonsController personsController = (PersonsController)context.Controller;

            IDictionary<string,object?>? paramters = 
                (IDictionary<string, object?>?)context.HttpContext.Items["Arguments"];
            if(paramters!=null)
            {
                if (paramters.ContainsKey("searchBy"))
                {
                    personsController.ViewData["CurrentSearchBy"] = Convert.ToString(paramters["searchBy"]);
                }
                if (paramters.ContainsKey("searchString"))
                {
                    personsController.ViewData["CurrentSearchString"] = Convert.ToString(paramters["searchString"]);
                }
                if (paramters.ContainsKey("sortBy"))
                {
                    personsController.ViewData["CurrentSortBy"] = Convert.ToString(paramters["sortBy"]);
                }
                if (paramters.ContainsKey("sortedOption"))
                {
                    personsController.ViewData["CurrentSortOrder"] = Convert.ToString(paramters["sortedOption"]);
                }
                personsController.ViewBag.SearchList = new Dictionary<string, string>()
                {
                    { nameof(PersonResponse.PersonName), "Person Name" },
                    { nameof(PersonResponse.Email), "Email" },
                    { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                    { nameof(PersonResponse.Gender), "Gender" },
                    { nameof(PersonResponse.CountryId), "Country" },
                    { nameof(PersonResponse.Address), "Address" }
                };
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["Arguments"] = context.ActionArguments;

            if(context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if(!String.IsNullOrEmpty(searchBy))
                {
                    var searchOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.Address),
                        nameof(PersonResponse.Age),
                        nameof(PersonResponse.Country),
                        nameof(PersonResponse.Email)
                    };
                    if(searchOptions.Any(x=>x==searchBy)==false)
                    {
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                }
            }
        }
    }
}
