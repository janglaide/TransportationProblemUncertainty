using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
