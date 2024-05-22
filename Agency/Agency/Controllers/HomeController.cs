using Agency.Bussiness.Service.Abstracts;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Agency.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IPortfolioService _portfolioService;

        public HomeController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public IActionResult Index()
        {
            var portfolios= _portfolioService.GetAllPortfolio();
            return View(portfolios);
        }

    
    }
}