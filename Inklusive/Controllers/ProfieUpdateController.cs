using Inklusive.Data;
using Inklusive.DTO;
using Inklusive.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inklusive.Controllers
{
    public class ProfieUpdateController : Controller
    {
        private readonly JwtHelper _jwtHelper;
        private readonly ApplicationDbContext _context;

        public ProfieUpdateController(JwtHelper jwtHelper, ApplicationDbContext context)
        {
            _jwtHelper = jwtHelper;
            _context = context;
        }


        [Authorize(Roles = "Super Admin,Admin")]
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


    }
}
