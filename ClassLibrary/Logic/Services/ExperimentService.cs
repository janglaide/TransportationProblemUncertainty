﻿using AutoMapper;
using ClassLibrary.Enums;
using ClassLibrary.Generators;
using ClassLibrary.Models;
using DataAccess;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Logic.Services
{
    public class ExperimentService
    {
        private readonly UnitOfWork _uow;
        public ExperimentService()
        {
            _uow = new UnitOfWork();
        }
        public void AddExperimentResult(List<(int, double)> results, GeneratorTaskCondition generator, 
            double accuracy, int R, CChangeParameters parameters)
        {
            var CsDistributionId = _uow.DistributionRepository.GetAll().First(x => x.Name == generator.DistributionC).Id;
            var ABDistributionId = _uow.DistributionRepository.GetAll().First(x => x.Name == generator.DistributionAB).Id;
            var LDistributionId = _uow.DistributionRepository.GetAll().First(x => x.Name == generator.DistributionL).Id;

            var distrParameters = new DistributionParametersModel(CsDistributionId, ABDistributionId, LDistributionId,
                                                                  generator.ParametersC.Item1, generator.ParametersC.Item2,
                                                                  generator.ParametersAB.Item1, generator.ParametersAB.Item2,
                                                                  generator.ParametersL.Item1, generator.ParametersL.Item2);

            if (CsDistributionId == 1) //nice code
            {
                distrParameters.DeviationCs = null;
            }
            if (ABDistributionId == 1)
            {
                distrParameters.DeviationAB = null;
            }
            if (LDistributionId == 1)
            {
                distrParameters.DeviationL = null;
            }

            var distributionParamsId = (_uow.DistributionParametersRepository.GetAll().Count() + 1);

            var mapper = new MapperConfiguration(x => x.CreateMap<DistributionParametersModel, DistributionParameters>()).CreateMapper();
            var distributionParameters = mapper.Map<DistributionParametersModel, DistributionParameters>(distrParameters);

            _uow.DistributionParametersRepository.Add(distributionParameters);

            var expModel = new ExperimentModel(distributionParamsId, accuracy);
            var experimentId = (_uow.ExperimentRepository.GetAll().Count() + 1);

            var mapperExp = new MapperConfiguration(x => x.CreateMap<ExperimentModel, DataAccess.Entities.Experiment>()).CreateMapper();
            var experiment = mapperExp.Map<ExperimentModel, DataAccess.Entities.Experiment>(expModel);

            _uow.ExperimentRepository.Add(experiment);

            int paramId;
            switch (parameters)
            {
                case CChangeParameters.Default:
                    paramId = 1;
                    break;
                case CChangeParameters.Basic:
                    paramId = 2;
                    break;
                default:
                    paramId = 3;
                    break;
            }

            foreach (var result in results)
            {
                var percentageModel = new PercentageModel(experimentId, result.Item1, result.Item2, R, paramId);
                var mapperPercentage = new MapperConfiguration(x => x.CreateMap<PercentageModel, Percentage>()).CreateMapper();
                var percentage = mapperPercentage.Map<PercentageModel, Percentage>(percentageModel);
                _uow.PercentageRepository.Add(percentage);
            }
        }
    }
}
