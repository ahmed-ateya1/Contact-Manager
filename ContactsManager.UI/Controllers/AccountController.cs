using Microsoft.AspNetCore.Mvc;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using ContactsManager.Core.Domain.IdentityEntites;
using ContactsManager.Core.Enumerator;

namespace ContactsManager.UI.Controllers
{
    //[AllowAnonymous]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthentication")]
        public IActionResult Register()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "NotAuthentication")]

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return View(registerDTO);
            }

            var user = new ApplicationUser()
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                PersonName = registerDTO.Name,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                string roleName = registerDTO.UserOption == UserTypeOptions.Admin
                    ? UserTypeOptions.Admin.ToString()
                    : UserTypeOptions.User.ToString();

                var foundRole = await _roleManager.FindByNameAsync(roleName);
                if (foundRole == null)
                {
                    var applicationRole = new ApplicationRole() { Name = roleName };
                    var resRole = await _roleManager.CreateAsync(applicationRole);
                    if (!resRole.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error creating role.");
                        return View(registerDTO);
                    }
                }

                var resultAddRole = await _userManager.AddToRoleAsync(user, roleName);
                if (!resultAddRole.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error adding user to role.");
                    return View(registerDTO);
                }

                return RedirectToAction("Index","Persons");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registerDTO);
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthentication")]

        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "NotAuthentication")]

        public async Task<IActionResult> Login(LoginDTO loginDTO, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();
                ViewBag.ReturnUrl = returnUrl;
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.
                    FindByEmailAsync(loginDTO.Email);

                if(user != null && await _userManager.IsInRoleAsync(user , UserTypeOptions.Admin.ToString()))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Persons");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password");
            ViewBag.ReturnUrl = returnUrl;
            return View(loginDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            var user = await _userManager.
                FindByEmailAsync(email);
            if (user == null)
                return Json(true);
            else
                return Json(false);
        }
    }
}
