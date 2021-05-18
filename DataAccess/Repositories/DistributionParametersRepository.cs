using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class DistributionParametersRepository : Repository<DistributionParameters>
    {
        public DistributionParametersRepository(ApplicationContext context) : base(context)
        {
        }
        public void Add(DistributionParameters item)
        {
            _context.DistributionParameters.Add(item);
            _context.SaveChanges();
        }
        public IEnumerable<DistributionParameters> GetAll()
        {
            return _context.DistributionParameters;
        }
    }
}
