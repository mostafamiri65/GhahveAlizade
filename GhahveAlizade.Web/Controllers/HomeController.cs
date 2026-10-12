using GhahveAlizade.Web.Extensions;
using GhahveAlizade.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GhahveAlizade.Web.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			if (User.Identity?.IsAuthenticated == true && User.IsAdmin())
			{
				return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult About()
		{
			ViewData["Title"] = "درباره ما";
			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Title"] = "تماس با ما";
			return View();
		}
	}
}