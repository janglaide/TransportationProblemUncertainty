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
    public class DistributionParametersService
    {
        private readonly UnitOfWork _uow;
        public DistributionParametersService()
        {
            _uow = new UnitOfWork();
        }
        public List<int> GetAppropriateIds((double, double) cParameters, 
            (double, double) abParameters, (double, double) lParameters)
        {
            var distributionParametersAll = GetAll();

            var checkedDistribution = distributionParametersAll.Where(x => x.DistributionABId == 3 &&
                x.DistributionCsId == 3 && x.DistributionLId == 3); // for uniform only for start

            var checkedCs = checkedDistribution.Where(x =>
                (x.DelayMeanCs <= (cParameters.Item1 + (cParameters.Item1 / 2)) &&
                x.DelayMeanCs >= (cParameters.Item1 - (cParameters.Item1 / 2))) &&
                (x.DeviationCs <= (cParameters.Item2 + (cParameters.Item2 / 2)) &&
                x.DeviationCs >= (cParameters.Item2 - (cParameters.Item2 / 2)))
            );

            var checkedAB = checkedCs.Where(x =>
                (x.DelayMeanAB <= (abParameters.Item1 + (abParameters.Item1 / 2)) &&
                x.DelayMeanAB >= (abParameters.Item1 - (abParameters.Item1 / 2))) &&
                (x.DeviationAB <= (abParameters.Item2 + (abParameters.Item2 / 2)) &&
                x.DeviationAB >= (abParameters.Item2 - (abParameters.Item2 / 2)))
            );

            var checkedL = checkedAB.Where(x =>
                (x.DelayMeanL <= (lParameters.Item1 + (lParameters.Item1 / 2)) &&
                x.DelayMeanL >= (lParameters.Item1 - (lParameters.Item1 / 2))) &&
                (x.DeviationL <= (lParameters.Item2 + (lParameters.Item2 / 2)) &&
                x.DeviationL >= (lParameters.Item2 - (lParameters.Item2 / 2)))
            );
            
            return checkedL.Select(x => x.Id).ToList();
        }

        
        private IEnumerable<DistributionParametersModel> GetAll()
        {
            var distributionParametersAll = _uow.DistributionParametersRepository.GetAll();
            var mapper = new MapperConfiguration(x => x.CreateMap<DistributionParameters, DistributionParametersModel>()).CreateMapper();
            return mapper.Map<IEnumerable<DistributionParameters>, IEnumerable<DistributionParametersModel>>(distributionParametersAll);
        }
    }
}
