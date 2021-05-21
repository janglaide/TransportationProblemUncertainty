using AutoMapper;
using ClassLibrary.Models;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClassLibrary.Logic.Services
{
    public class PercentageService
    {
        private readonly UnitOfWork _uow;
        public PercentageService()
        {
            _uow = new UnitOfWork();
        }
        public List<double> GetAppropriate(int N, List<int> appropriateIds)
        {
            var percentages = GetAll();
            return percentages
                .Where(p => p.N == N)
                .Where(p => appropriateIds.Contains(p.ExperimentId))
                .Select(p => p.Value).ToList();
        }
        private IEnumerable<PercentageModel> GetAll()
        {
            var percentagesAll = _uow.PercentageRepository.GetAll();
            var mapper = new MapperConfiguration(x => x.CreateMap<Percentage, PercentageModel>()).CreateMapper();
            return mapper.Map<IEnumerable<Percentage>, IEnumerable<PercentageModel>>(percentagesAll);
        }
    }
}
