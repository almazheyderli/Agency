using Agency.Bussiness.Exceptions;
using Agency.Bussiness.Service.Abstracts;
using Agency.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public IActionResult Index()
        {
            var portfolios=_portfolioService.GetAllPortfolio(); 
            return View(portfolios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portfolio portfolio)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _portfolioService.AddPortfolio(portfolio);
            }
            catch(FileSizeException  ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch(FileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var portfolio=_portfolioService.GetPortfolio(x=>x.Id == id);
            if(portfolio == null)
            {
                return NotFound();
            }
            _portfolioService.RemovePortfolio(portfolio.Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
         var oldPort=_portfolioService.GetPortfolio(x=>x.Id==id);
           if(oldPort == null)
            {
                return NotFound();
            }
           return View(oldPort);
        }
        [HttpPost]
        public IActionResult Update(int id, Portfolio portfolio)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            _portfolioService.UpdatePortfolio(portfolio.Id, portfolio);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
