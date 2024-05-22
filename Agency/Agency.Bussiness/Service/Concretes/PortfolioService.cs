using Agency.Bussiness.Exceptions;
using Agency.Bussiness.Service.Abstracts;
using Agency.Core.Models;
using Agency.Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Matching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Bussiness.Service.Concretes
{
    public class PortfolioService : IPortfolioService
    {
      private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PortfolioService(IPortfolioRepository portfolioRepository, IWebHostEnvironment webHostEnvironment)
        {
            _portfolioRepository = portfolioRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddPortfolio(Portfolio portfolio)
        {
            if (portfolio.ImgFile == null)
            {
                throw new ArgumentNullException("ImgFile", "nulll");
            }
            if (portfolio.ImgFile.Length > 2097125)
            {
                throw new FileSizeException("ImgFile", "olcu boyukdur");
            }
            if (!portfolio.ImgFile.ContentType.Contains("image"))
            {
                throw new FileContentException("ImgFile", "Formata uygun deyil");
            }
            string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + portfolio.ImgFile.FileName;
            using(FileStream stream=new FileStream(path, FileMode.Create))
            {
                portfolio.ImgFile.CopyTo(stream);
            }
            portfolio.ImgUrl = portfolio.ImgFile.FileName;
            _portfolioRepository.Add(portfolio);
            _portfolioRepository.Commit();
        }

        public List<Portfolio> GetAllPortfolio(Func<Portfolio, bool>? func = null)
        {
           return  _portfolioRepository.GetAll(func);
        }

        public Portfolio GetPortfolio(Func<Portfolio, bool>? func = null)
        {
          return _portfolioRepository.Get(func);
        }

        public void RemovePortfolio(int id)
        {
            var portfolio = _portfolioRepository.Get(x => x.Id == id);
            if (portfolio == null)
            {
                throw new Exception();
            }
            string path=_webHostEnvironment.WebRootPath+ @"/Upload/Service/"+ portfolio.ImgUrl;
            if (!File.Exists(path))
            {
                throw new PortfolioNotFoundException("ImgUrl", "tapilmadi");
            }
            File.Delete(path);
            _portfolioRepository.Remove(portfolio);
            _portfolioRepository.Commit();
        }
        public void UpdatePortfolio(int id, Portfolio portfolio)
        {
            var oldPort=_portfolioRepository.Get(x=>x.Id == id);
            if (oldPort == null)
            {
                throw new NullReferenceException();
            }
            if(portfolio.ImgFile != null)
            {
                string filename = portfolio.ImgFile.FileName;
                string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + portfolio.ImgFile.FileName;
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    portfolio.ImgFile.CopyTo(stream);
                }
                FileInfo fileInfo = new FileInfo(path + oldPort.ImgUrl);
               if(fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                oldPort.ImgUrl = filename;
            }
            oldPort.Title = portfolio.Title;
            oldPort.Description = portfolio.Description;
            _portfolioRepository.Commit();

        }
    }
}
