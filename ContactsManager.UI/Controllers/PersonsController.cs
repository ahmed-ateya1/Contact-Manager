using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContract;
using ServiceContract.DTO;
using ServiceContract.Enumerator;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Rotativa.AspNetCore;
using ContactAppManager.Filters.ActionFilters;
using System.Reflection;
using ContactAppManager.Filters.ResourseFilters;
using ContactAppManager.Filters.AuthorizationFilters;
using ContactAppManager.Filters.ResultFilters;
using Microsoft.AspNetCore.Authorization;


namespace ContactAppManager.Controllers
{
 
    public class PersonsController : Controller
    {
        private readonly IPersonsServices _personsServices;
        private readonly ICountriesServices _countriesServices;

        public PersonsController(IPersonsServices personsServices, ICountriesServices countriesServices)
        {
            _personsServices = personsServices;
            _countriesServices = countriesServices;
        }

        [Route("persons/index")]
        [Route("/")]
        [PersonsListActionFilter]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
    string sortBy = nameof(PersonResponse.PersonName), SortedOptions sortedOption = SortedOptions.ASC)
        {

            List<PersonResponse> persons = await _personsServices.GetFilteredPersons(searchBy, searchString);
          

            List<PersonResponse> sortedPersons = await _personsServices.GetSortedPersons(persons, sortBy, sortedOption);
          

            return View(sortedPersons);
        }


        [Route("person/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesServices.GetAllCountry();
            ViewBag.Countries = countries.Select(x =>
                new SelectListItem()
                {
                    Text = x.CountryName,
                    Value = x.CountryId.ToString()
                }).ToList();
            return View(new PersonAddRequest());
        }

        [Route("person/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            var personResponse = await _personsServices.AddPerson(personRequest);

            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{personId}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid personId)
        {
            var person = await _personsServices.GetPersonBy(personId);
            if (person == null)
            {
                return RedirectToAction("Index");
            }

            var personUpdateRequest = person.ToPersonUpdateRequest();
            var countries = await _countriesServices.GetAllCountry();
            ViewBag.Countries = countries.Select(x =>
                new SelectListItem()
                {
                    Text = x.CountryName,
                    Value = x.CountryId.ToString()
                }).ToList();

            return View(personUpdateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            var personUpdate = await _personsServices.GetPersonBy(personRequest.PersonId);

            if (personUpdate == null)
            {
                return View("Index");
            }

            PersonResponse personResponse = await _personsServices.UpdatePerson(personRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            var personResponse = await _personsServices.GetPersonBy(personId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            var personResponse = await _personsServices.GetPersonBy(personUpdateRequest.PersonId);
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personsServices.DeletePerson(personUpdateRequest.PersonId);
            return RedirectToAction("Index");
        }
        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPdf()
        {
            var PersonResponse = await _personsServices.GetAllPersons();
            return new ViewAsPdf("PersonsPdf", PersonResponse, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 20,
                    Right = 20,

                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            var memeoryStram = await _personsServices.GetPersonsCSV();

            return File(memeoryStram, "application/octet-stream" ,"persons.csv");
        }
        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            var memeoryStram = await _personsServices.GetPersonExcel();

            return File(memeoryStram, "	application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
