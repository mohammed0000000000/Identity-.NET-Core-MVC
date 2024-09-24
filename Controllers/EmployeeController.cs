using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize]
        public IActionResult Index() {
            return View();
        }
    }
}
