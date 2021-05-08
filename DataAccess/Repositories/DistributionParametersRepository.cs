using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
