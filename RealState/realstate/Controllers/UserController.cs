using Microsoft.AspNetCore.Mvc;
using realstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Random()
        {
            User user = new User() { Email = "myemail", Id = 1 };
            return View(user);
        }
    }
}
