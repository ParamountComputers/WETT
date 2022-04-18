using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WETT.Models;
using WETT.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace WETT.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly WETT_DBContext _dbContext;

		public HomeController(ILogger<HomeController> logger, WETT_DBContext dBContext)
		{
			_logger = logger;
			_dbContext = dBContext;
		}

		public IActionResult Index()
		{
			HomeViewModel model = new ();
			if(User.Identity.IsAuthenticated)
			{
				try
				{
					if (User.Identity.AuthenticationType == "aad")
					{
						if (User.Claims.SingleOrDefault(c => c.Type == "name") != null)
						{
							model.Name = User.Claims.SingleOrDefault(c => c.Type == "name").Value;
						}
					}
					else if (User.Identity.AuthenticationType == "NTLM")
					{
						model.Name = User.Claims.SingleOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value;
					}
				}
				catch { }
			}
			else
			{
				model.Name = "Uauthenticated user";
			}
			
			return View(model);
		}

		[Authorize(Roles = "e6b52d23-02e3-4a07-9e52-206faf0ae99d")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult AuthView()
		{
			AuthViewModel model = new();
#if (DEBUG)
			model.CompileMode = "Debug";
#else
	model.CompileMode = "Release";
#endif
			if (User.Identity.IsAuthenticated)
			{
				model.IsAuthenticated = "Y";
				model.AuthType = User.Identity.AuthenticationType;
				try
				{
					model.Claims = User.Claims;
					model.UserId = User.Identity.Name;
					model.IsInRole = User.IsInRole("e6b52d23-02e3-4a07-9e52-206faf0ae99d") ? "Yes" : "No";
					if (User.Identity.AuthenticationType == "aad")
					{
						if (User.Claims.SingleOrDefault(c => c.Type == "name") != null)
						{
							model.Name = User.Claims.SingleOrDefault(c => c.Type == "name").Value;
						}
					}
					else if (User.Identity.AuthenticationType == "NTLM")
					{
						model.Name = User.Claims.SingleOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value;
					}
				}
				catch { }

			}
			else
			{
				model.IsAuthenticated = "N";
			}

			return View(model);
		}
	}
}
