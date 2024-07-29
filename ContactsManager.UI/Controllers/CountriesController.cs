using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContactAppManager.Controllers
{
    [Route("[controller]/[action]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesServices _countriesServices;

        public CountriesController(ICountriesServices countriesServices)
        {
            _countriesServices = countriesServices;
        }

        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            ViewBag.CurrentURL = "/Countries/UploadFromExcel";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelfile)
        {
            if (excelfile == null || excelfile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please Select xlsx file";
                return View();
            }
            if (!Path.GetExtension(excelfile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }
            int countryCountriesInserted = await _countriesServices.UploadCountriesFromExcelFile(excelfile);

            ViewBag.Message = $"{countryCountriesInserted} countries uploaded";
            return View();
        }
    }
}