using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using ClassLibrary.Enums;
using ClassLibrary.Generators;
using ClassLibrary.Logic.Services;
using ClassLibrary.MethodParameters;

namespace ClassLibrary.Logic
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
            _cChangeParameters = cChangeParameters;
        }
        private double SearchMeanPercentForSize(int size, int matrixQuantity, double averChange, Solver solver, Random random)
        {
            PercentFinder.PercentDelegate SearchPercent = GetPercentOfChange;
            var parameters = new ParametersForRandom(size, matrixQuantity, _cChangeParameters);
            return PercentFinder.SearchMeanPercent(SearchPercent, parameters, averChange, solver, random);
        }
        private int GetPercentOfChange(SearchParameters parameters, Solver solver, Random random)
        {
            if (!(parameters is ParametersForRandom))
            {
                throw new ArgumentException("Wrong parameters type in method Experiment.GetPercentOfChange. Need to be ParametersForRandom.");
            }
            var param = (ParametersForRandom)parameters;
            int percent;
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
                (a, b) = _generator.GenerateAB(param.Size, random);
                l = _generator.GenerateL(param.MatrixQuantity, random);
                alpha = _generator.GenerateAlpha(param.MatrixQuantity);
                for (int i = 0; i < param.MatrixQuantity; i++)
                {
                    cs[i] = _generator.GenerateMatrix(param.Size, random);
                }
                (_, solutions) = solver.GetSolutions(cs, a, b);
                (x, _) = solver.SolveSeveral(cs, a, b, l, alpha, solutions);
                if (!solver.CheckABConstraints(solver.DivideX(solver.RoundVector(x), param.MatrixQuantity), a, b))
                {
                    continue;
                }
                success = true;
            }
            var parametersForDefined = new ParametersForDefined(x, cs, a, b, l, alpha, _cChangeParameters);
            percent = PercentFinder.FindPercentOfChange(parametersForDefined, solver, random);
            return percent;
        }
        public List<(int, double)> RunExperiment(int startSize, int finishSize, int step, int matrixQuantity, double averChange, BackgroundWorker worker)
        {
            List<(int, double)> results = new List<(int, double)>();
            var random = new Random();
            var solver = new Solver();
            var quantity = Math.Floor((double)((finishSize - startSize) / step) + 1);
            var progress = 1;
            var interval = (int)(100 / (quantity));
            worker.ReportProgress(0, string.Format("Working on N = {0}", startSize));
            for (int i = startSize; i <= finishSize; i += step)
            {
                results.Add((i, SearchMeanPercentForSize(i, matrixQuantity, averChange, solver, random)));
                if((i + step) <= finishSize)
                    worker.ReportProgress(progress * interval, string.Format("Working on N = {0}", i + step));
                progress++;
            }

            var service = new ExperimentService();
            service.AddExperimentResult(results, _generator, averChange);

            return results;
        }
        public List<List<(int, double)>> RunExperiment(int startSize, int finishSize, int step, int startMatrixQuantity, int finishMatrixQuantity, double averChange, BackgroundWorker worker)
        {
            List<List<(int, double)>> results = new List<List<(int, double)>>();
            for (int i = startMatrixQuantity; i <= finishMatrixQuantity; i += step)
            {
                results.Add(RunExperiment(startSize, finishSize, step, i, averChange, worker));
            }
            return results;
        }
    }
}
