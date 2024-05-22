using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Bussiness.Exceptions
{
    public class PortfolioNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public PortfolioNotFoundException( string propertyName ,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
