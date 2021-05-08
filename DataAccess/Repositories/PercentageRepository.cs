using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class PercentageRepository : Repository<Percentage>
    {
        public PercentageRepository(ApplicationContext context) : base(context)
        {
        }
        public void Add(Percentage item)
        {
            _context.Percentages.Add(item);
            _context.SaveChanges();
        }
        public IEnumerable<Percentage> GetAll()
        {
            return _context.Percentages;
        }
    }
}
