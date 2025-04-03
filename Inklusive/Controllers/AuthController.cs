using Inklusive.Data;
using Inklusive.Helper;
using Inklusive.Models;
using Inklusive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inklusive.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;


        public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (user.RoleId == 3)
            {
                string defaultpassword = UserService.GenerateRandomPassword();
                user.PasswordHash = UserService.HashPassword(defaultpassword);
                user.isFirstLogin = true;

                EmailService.SendEmail(user.Email, "Your Account Details",
                $"Hello {user.Username},\n\nYour account has been created.\nYour default password is: {defaultpassword}\n\nPlease log in and change it immediately.\n\nYour" +
                $"Email is  {user.Email}");
            }
            else
            {
                user.PasswordHash = UserService.HashPassword(user.PasswordHash);
            }

            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
            }


            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && UserService.VerifyPassword(user.PasswordHash, password))
            {

                if (user.isFirstLogin)
                {
                    var tmp = _jwtHelper.GenerateToken(user);
                    Response.Cookies.Append("AuthToken", tmp, new CookieOptions { HttpOnly = true });

                    return RedirectToAction("ChangePassword","User");
                }
                else
                {

                    // Console.WriteLine(user.Role.Name);
                    var token = _jwtHelper.GenerateToken(user);

                    Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true });

                    if (user.Role.Name == "Employee") return RedirectToAction("EmployeeHome", "Home");
                    else return RedirectToAction("AdminHome", "Home");
                }
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

    }
}
