using Inklusive.Data;
using Inklusive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text;
using Inklusive.Helper;
using Inklusive.Services;

using Microsoft.AspNetCore.Authorization;
using Inklusive.DTO;
using System.Threading.Tasks;

namespace Inklusive.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;


        public UserController(ApplicationDbContext context, JwtHelper jwtHelper)
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

                UserService.SendEmail(user.Email, "Your Account Details",
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

                    return RedirectToAction("ChangePassword");
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


        public async Task<IActionResult> UserInfoAsync()
        {
            var token = Request.Cookies["AuthToken"];
            var claims = _jwtHelper.ValidateToken(token);
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");
            var userId = Convert.ToInt32(userIdClaim.Value);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return View(users);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> MyInfo(int id)
        {

            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword)
        {
            var token = Request.Cookies["AuthToken"];


            var claims = _jwtHelper.ValidateToken(token);


            var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");


            var userId = Convert.ToInt32(userIdClaim.Value);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            user.PasswordHash = UserService.HashPassword(newPassword);
            user.isFirstLogin = false;
            await _context.SaveChangesAsync();

            var newToken = _jwtHelper.GenerateToken(user);
            return RedirectToAction("Login");
        }


        [Authorize(Roles = "Super Admin")]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user.RoleId == 3)
            {
                string defaultpassword = UserService.GenerateRandomPassword();
                user.PasswordHash = UserService.HashPassword(defaultpassword);
                user.isFirstLogin = true;

                UserService.SendEmail(user.Email, "Your Account Details",
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
            return RedirectToAction("Index", "Home");

        }

        [Authorize(Roles = "Super Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully.");
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> ShowAllRequests()
        {
            var requests = await _context.ProfileUpdates.Include(u => u.User).ToListAsync();
            return View(requests);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        [HttpPut]
        public async Task<IActionResult> ApproveProfileUpdate(int profileUpdateId)
        {
            var profileUpdate = await _context.ProfileUpdates.Include(p => p.User).ThenInclude(r => r.Role).FirstOrDefaultAsync(p => p.Id == profileUpdateId);

            if (profileUpdate == null)
            {
                return NotFound("Profile update request not found.");
            }

            
            if (User.IsInRole("Admin") && profileUpdate.User.Role.Name != "Employee")
            {
                return Unauthorized("Admins can only approve changes for Employees.");
            }

            profileUpdate.IsApproved = true;
            profileUpdate.DateApproved = DateTime.UtcNow;
            _context.ProfileUpdates.Update(profileUpdate);

           
            var user = profileUpdate.User;
            user.Username = profileUpdate.UpdatedField;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            await UserService.SendEmailAsync(user.Email, "Your profile update was approved!", "Your profile changes have been approved by the admin.");

            return Ok("Profile update approved successfully.");
        }

        [Authorize(Roles = "Employee")]
        public IActionResult UpdateEProfile()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEProfile([FromBody]EmployeeProfileUpdateModel updateModel)
        {
            var token = Request.Cookies["AuthToken"];
            var claims = _jwtHelper.ValidateToken(token);
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");
            var userId = Convert.ToInt32(userIdClaim.Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            
            var profileUpdate = new ProfileUpdate
            {
                UserId = user.Id,
                UpdatedField = updateModel.NewUsername, 
                DateRequested = DateTime.UtcNow
            };

            _context.ProfileUpdates.Add(profileUpdate);
            await _context.SaveChangesAsync();

            return Ok("Profile update requested. Your changes will be visible once approved.");
        }

        [Authorize(Roles ="Super Admin,Admin")]
        public IActionResult EditAdminOwnInfo()
        {
            return View();
        }
        [HttpPut]
        public async Task<IActionResult> EditAdminOwnInfo([FromBody] AdminProfileUpdateModel updateModel)
        {
            var token = Request.Cookies["AuthToken"];
            var claims = _jwtHelper.ValidateToken(token);
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");
            var userId = Convert.ToInt32(userIdClaim.Value);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

            user.Username = updateModel.NewUsername;
            user.Email = updateModel.NewEmail;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Your Info is Update successfully");
        }

        [Authorize(Roles ="Super Admin")]
        public IActionResult EditAdminInfo(int id)
        {
            ViewBag.UserId = id;
            return View();
        }
        [HttpPut]
        public async Task<IActionResult> EditAdminInfo(int id, [FromBody] AdminProfileUpdateModel updateModel)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

            user.Username = updateModel.NewUsername;
            user.Email = updateModel.NewEmail;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Admin info Changed");

        }

    }
}
