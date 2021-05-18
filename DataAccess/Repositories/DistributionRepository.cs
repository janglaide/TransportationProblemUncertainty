using DataAccess.Entities;
using System.Collections.Generic;

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
