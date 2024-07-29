using ContactAppManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering; // Correct namespace for SelectListItem
using ServiceContract;
using ServiceContract.DTO;

namespace ContactAppManager.Filters.ActionFilters
{
    public class PersonCreateAndEditActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesServices _countriesServices;

        public PersonCreateAndEditActionFilter(ICountriesServices countriesServices)
        {
            _countriesServices = countriesServices;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesServices.GetAllCountry();
                    personsController.ViewBag.Countries =
                        countries.Select(temp => new SelectListItem()
                        {
                            Value = temp.CountryId.ToString(),
                            Text = temp.CountryName
                        }).ToList();

                    personsController.ViewBag.Errors = personsController.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var personRequest = context.ActionArguments["personRequest"];

                    context.Result = personsController.View(personRequest);

                    return;
                }
                await next();
            }

        }
    }
}
