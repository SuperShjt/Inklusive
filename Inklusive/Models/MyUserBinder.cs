using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inklusive.Models
{
    public class MyUserBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string Fname = bindingContext.HttpContext.Request.Form["Fname"];
            string Lname = bindingContext.HttpContext.Request.Form["Lname"];
            string email = bindingContext.HttpContext.Request.Form["email"];
            string password = bindingContext.HttpContext.Request.Form["password"];
            int RoleId = Convert.ToInt32(bindingContext.HttpContext.Request.Form["RoleId"]);

            var user = new User()
            {
                Username = Fname + Lname,  
                Email = email,
                PasswordHash = password,
                RoleId = RoleId
            };

            bindingContext.Result = ModelBindingResult.Success(user);

            return Task.CompletedTask;
        }
    }
}
