﻿using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Experiment
    {
        private readonly CChangeParameters _cChangeParameters;
        private readonly GeneratorTaskCondition _generator;
        public Experiment() { }
        public Experiment(GeneratorTaskCondition generator)
        {
            _generator = generator;
            _cChangeParameters = CChangeParameters.Default;
        }
        public Experiment(GeneratorTaskCondition generator, CChangeParameters cChangeParameters) : this(generator)
        {
            this._cChangeParameters = cChangeParameters;
        }
        private double SearchMeanPercentForSize(int size, int matrixQuantity, double averChange)
        {
            PercentFinder.PercentDelegate SearchPercent = GetPercentOfChange;
            ParametersForRandom parameters = new ParametersForRandom(size, matrixQuantity, /*a, b, l, alpha,*/ _cChangeParameters);
            return PercentFinder.SearchMeanPercentForDefinedCondition(SearchPercent, parameters, averChange);
        }
        private double GetPercentOfChange(SearchParameters parameters)
        {
            if (!(parameters is ParametersForRandom))
            {
                throw new ArgumentException("Wrong parameters type in method Experiment.GetPercentOfChange. Need to be ParametersForRandom.");
            }
            ParametersForRandom param = (ParametersForRandom)parameters;
            double percent;
            double[] x = new double[param.Size * param.Size];
            double[][] cs = new double[param.MatrixQuantity][];
            double[] a = new double[param.Size];
            double[] b = new double[param.Size];
            double[] l = new double[param.MatrixQuantity];
            double[] alpha = new double[param.MatrixQuantity];
            double[] solutions;

            bool success = false;
            while (!success)
            {
                (a, b) = _generator.GenerateAB(param.Size);
                l = _generator.GenerateL(param.MatrixQuantity);
                alpha = _generator.GenerateAlpha(param.MatrixQuantity);
                for (int i = 0; i < param.MatrixQuantity; i++)
                {
                    cs[i] = _generator.GenerateMatrix(param.Size);
                }
                (_, solutions) = Solver.GetSolutions(cs, a, b);
                (x, _) = Solver.SolveSeveral(cs, a, b, l, alpha, solutions);
                if (!Solver.CheckABConstraints(Solver.DivideX(Solver.RoundVector(x), param.MatrixQuantity), a, b))
                {
                    continue;
                }
                success = true;
            }
            ParametersForDefined parametersForDefined = new ParametersForDefined(x, cs, a, b, l, alpha, _cChangeParameters);
            percent = PercentFinder.FindPercentOfChange(parametersForDefined);
            return percent;
        }
        public List<(int, double)> RunExperiment(int startSize, int finishSize, int step, int matrixQuantity, double averChange)
        {
            List<(int, double)> results = new List<(int, double)>();
            for (int i = startSize; i <= finishSize; i += step)
            {
                results.Add((i, SearchMeanPercentForSize(i, matrixQuantity, averChange)));
            }
            return results;
        }
        public List<List<(int, double)>> RunExperiment(int startSize, int finishSize, int step, int startMatrixQuantity, int finishMatrixQuantity, double averChange)
        {
            List<List<(int, double)>> results = new List<List<(int, double)>>();
            for (int i = startMatrixQuantity; i <= finishMatrixQuantity; i += step)
            {
                results.Add(RunExperiment(startSize, finishSize, step, i, averChange));
            }
            return results;
        }
    }
}
