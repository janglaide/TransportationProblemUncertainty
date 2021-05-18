using DataAccess.Repositories;

namespace DataAccess
{
    public class UnitOfWork
    {
        private readonly ApplicationContext _context;

        private DistributionRepository _distributionRepository;
        private DistributionParametersRepository _distributionParametersRepository;
        private ExperimentRepository _experimentRepository;
        private PercentageRepository _percentageRepository;
        public UnitOfWork()
        {
            _context = new ApplicationContext();
            _distributionRepository = new DistributionRepository(_context);
            _distributionParametersRepository = new DistributionParametersRepository(_context);
            _experimentRepository = new ExperimentRepository(_context);
            _percentageRepository = new PercentageRepository(_context);
        }
        public DistributionRepository DistributionRepository => _distributionRepository ??= new DistributionRepository(_context);
        public DistributionParametersRepository DistributionParametersRepository => _distributionParametersRepository ??= new DistributionParametersRepository(_context);
        public ExperimentRepository ExperimentRepository => _experimentRepository ??= new ExperimentRepository(_context);
        public PercentageRepository PercentageRepository => _percentageRepository ??= new PercentageRepository(_context);
    }
}
