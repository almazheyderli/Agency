using Agency.Core.DTOs.AccountDto;
using Agency.Core.Models;
using Agency.Helpers.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public AccountController(UserManager<User> usermanager, SignInManager<User> signInManager, RoleManager<IdentityRole> rolemanager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _rolemanager = rolemanager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                UserName = registerDto.Usurname
            };
            var result= await _usermanager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _usermanager.AddToRoleAsync(user, UserRole.Admin.ToString());

            //_signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user =await _usermanager.FindByNameAsync(loginDto.UsernameOrEmail);
            if (user == null)
            {
                user = await  _usermanager.FindByEmailAsync(loginDto.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "username ve ya password yanlisdir");
                    return View();
                }
                var result= await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "birazdan yeniden cehd edin");
                    return View();
                }
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("", "username ve ya password yanlisdir");
                    return View();
                }

               
            }
           var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
           var res2 = await _signInManager.PasswordSignInAsync(user, loginDto.Password,loginDto.IsRemember,false);
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> CreateRole()
        {
            foreach(var item in Enum.GetValues(typeof(UserRole)))
            {
                await _rolemanager.CreateAsync(new IdentityRole
                {
                    Name = item.ToString(),
                });
            }
            return Ok();
        }
        
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
