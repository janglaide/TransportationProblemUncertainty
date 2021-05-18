using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class ExperimentRepository : Repository<Experiment>
    {
        public ExperimentRepository(ApplicationContext context) : base(context) { }
        public void Add(Experiment item)
        {
            _context.Experiments.Add(item);
            _context.SaveChanges();
        }
        public IEnumerable<Experiment> GetAll()
        {
            return _context.Experiments;
        }
    }
}
