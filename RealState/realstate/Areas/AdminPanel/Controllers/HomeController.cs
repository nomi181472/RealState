using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realstate.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = SD.AdminUser + "," + SD.VerifiedUser)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
