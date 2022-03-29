﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WETT.Models;
using WETT.Data;
using System.Security.Claims;

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
			HomeViewModel model = new HomeViewModel();
			if(User.Identity.IsAuthenticated)
			{
				model.IsAuthenticated = "Y";
				try
				{
					model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					model.Name=User.FindFirstValue(ClaimTypes.Name);
					model.Source = "User.FindFirstValue(ClaimTypes.NameIdentifier)";
				}
				catch { }

				try
				{
					model.UserId = HttpContext.User.Identity.Name;
					model.Source = HttpContext.User.Identity.Name;
				}
				catch { }
			}
			else
			{
				model.IsAuthenticated = "N";
			}
			
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
