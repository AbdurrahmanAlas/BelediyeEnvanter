using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BelediyeEnvanter.Controllers;
[AllowAnonymous] public class AccountController(SignInManager<IdentityUser> signIn) : Controller
{
    [HttpGet] public IActionResult Login(string? returnUrl=null)=>View(new LoginModel(returnUrl:returnUrl));
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginModel model){if(!ModelState.IsValid)return View(model);var result=await signIn.PasswordSignInAsync(model.UserName,model.Password,model.RememberMe,false);if(result.Succeeded)return Redirect(model.returnUrl??"/");ModelState.AddModelError("","Kullanıcı adı veya parola hatalı.");return View(model);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Logout(){await signIn.SignOutAsync();return RedirectToAction(nameof(Login));}
    public record LoginModel(string UserName="",string Password="",bool RememberMe=false,string? returnUrl=null);
}
