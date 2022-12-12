using ITI.ElectroDev.Models;
using ITI.ElectroDev.Presentation;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmazonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        Context c;
        UserManager<User> UserManager;
        SignInManager<User> SignInManager;
        RoleManager<IdentityRole> RoleManager;
        public UserController(
            UserManager<User> usermanager,
            SignInManager<User> signInManager,
            Context _c,
            RoleManager<IdentityRole> roleManager
            )

        {
            UserManager = usermanager;
            SignInManager = signInManager;
            this.c = _c;
            RoleManager = roleManager;
        }
        [HttpPost]
        public async Task<ResultViewModel> SignUpAsViewer(UserCreateModel model)
        {
            model.Role = "Admin";
            ResultViewModel myModel = new ResultViewModel();
            if (ModelState.IsValid == false)
            {
                myModel.Success = false;
                myModel.Data =
                    ModelState.Values.SelectMany
                            (i => i.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                User user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                IdentityResult result
                      = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded == false)
                {
                    result.Errors.ToList().ForEach(i =>
                    {
                        ModelState.AddModelError("", i.Description);
                    });
                    myModel.Success = false;
                    myModel.Data =
                        ModelState.Values.SelectMany
                                (i => i.Errors.Select(x => x.ErrorMessage));
                }
                else
                {
                    await UserManager.AddToRoleAsync(user, model.Role);
                    myModel.Success = true;
                    myModel.Message = "successful sign up";
                    myModel.Data = null;
                }
            }
            return myModel;
        }

    }
}
