using IdentityDemo.Auth;
using IdentityDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace IdentityDemo.Controllers
{
    public class AccountController : Controller
    {
        // inject UserManager
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser>signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }  
        [HttpGet]
        [Authorize]
        public IActionResult TestClaims(){
            string name = User.Identity.Name;
            Claim claimId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            return Content($"User Id {claimId.Value}");
        }
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser) {
            if (ModelState.IsValid) {
                // map from RegisterUser to UserModel
                ApplicationUser userModel = new ApplicationUser() {
                    UserName = registerUser.UserName,
                    PasswordHash = registerUser.Password,
                    Address = registerUser.Address,
                };

                IdentityResult result = await userManager.CreateAsync(userModel, registerUser.Password);
                //userManager.AddClaimsAsync(userModel, new Claim() { Type = "dfdf", Value = "" });
                if (result.Succeeded) {
                    //signInManager.SignInAsync(); // store only Id, Name, Roles in Cookies
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Color", "Red"));

                    // create Cookies
                    await signInManager.SignInWithClaimsAsync(user:userModel, isPersistent:false,claims);// store  Id, Name, Roles and some claims in Cookies  
                    // Role for 
                    // await userManager.AddToRoleAsync(userModel, "Student");
                    return RedirectToAction("Login", "Account");
                } else {
                    foreach (var errorItem in result.Errors) {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                    // save Db
                }
                return View(registerUser);
            }
            return View(registerUser);

        }
        [HttpGet]
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(LoginUserViewModel LoginUser){
            if (ModelState.IsValid) {
                var user = await userManager.FindByNameAsync(LoginUser.UserName);
                if(user == null)
                {
                    ModelState.AddModelError("UserName", "Invalid UserName");
                    return View();
                }
                var checkPassword = await userManager.CheckPasswordAsync( user ,password: LoginUser.Password);
                if(!checkPassword){
                    ModelState.AddModelError("Password", "Invalid Password");
                    return View();
                }
                await signInManager.SignInAsync(user, LoginUser.RememberMe == true ? true : false);
                return RedirectToAction("Index", "Employee");
            }
            return View(LoginUser);
        }
        public IActionResult Logout(){
            signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AddAdmin(){
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterUserViewModel registerUser){
            if (ModelState.IsValid) {
                ApplicationUser userModel = new ApplicationUser() {
                    UserName = registerUser.UserName,
                    PasswordHash = registerUser.Password,
                    Address = registerUser.Address,
                };
                var result = await userManager.CreateAsync(userModel, registerUser.Password);

                if (result.Succeeded ){
                    await userManager.AddToRoleAsync(userModel, "Admin");
                    await signInManager.SignInAsync(userModel,false);
                    return View("Login");
                }
                if (result.Errors.Count() > 0) {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerUser);
        }       
    }
}
