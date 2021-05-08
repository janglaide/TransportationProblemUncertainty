using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class DistributionRepository : Repository<Distribution>
    {
        public DistributionRepository(ApplicationContext context) : base(context) { }
        public IEnumerable<Distribution> GetAll()
        {
            return _context.Distributions;
        }
        
    }
}
