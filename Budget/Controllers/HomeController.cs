using Budget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
            => View();
    }
}
