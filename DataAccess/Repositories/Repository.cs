using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class Repository<T> where T : class
    {
        protected ApplicationContext _context;
        public Repository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
