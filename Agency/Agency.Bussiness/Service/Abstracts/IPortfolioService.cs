using Agency.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Bussiness.Service.Abstracts
{
    public interface IPortfolioService
    {
        void AddPortfolio(Portfolio portfolio);
        void RemovePortfolio(int id);
        void UpdatePortfolio(int id,Portfolio portfolio);
        Portfolio GetPortfolio(Func<Portfolio,bool>? func=null);
        List<Portfolio> GetAllPortfolio(Func<Portfolio,bool>? func=null);
    }
}
