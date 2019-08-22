using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightsAndBites.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestController : Controller
    {
        public IActionResult RoleTest()
        {
            return View();
        }
    }

}