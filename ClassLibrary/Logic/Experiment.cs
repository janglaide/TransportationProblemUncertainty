using ClassLibrary.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _cChangeParameters = cChangeParameters;
        }
        private double SearchMeanPercentForSize(int size, int matrixQuantity, double averChange)
        {
            PercentFinder.PercentDelegate SearchPercent = GetPercentOfChange;
            var parameters = new ParametersForRandom(size, matrixQuantity, _cChangeParameters);
            return PercentFinder.SearchMeanPercent(SearchPercent, parameters, averChange);
        }
        private int GetPercentOfChange(SearchParameters parameters, Solver solver)
        {
            if (!(parameters is ParametersForRandom))
            {
                throw new ArgumentException("Wrong parameters type in method Experiment.GetPercentOfChange. Need to be ParametersForRandom.");
            }
            var generator = _generator.Copy();
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
                (a, b) = generator.GenerateAB(param.Size);
                l = generator.GenerateL(param.MatrixQuantity);
                alpha = generator.GenerateAlpha(param.MatrixQuantity);
                for (int i = 0; i < param.MatrixQuantity; i++)
                {
                    cs[i] = generator.GenerateMatrix(param.Size);
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
            percent = PercentFinder.FindPercentOfChange(parametersForDefined, solver);
            return percent;
        }
        public List<(int, double)> RunExperiment(int startSize, int finishSize, int step, int matrixQuantity, double averChange)
        {
            List<(int, double)> results = new List<(int, double)>();
            var task = Task.Run(() =>
            {
                for (int i = startSize; i <= finishSize; i += step)
                {
                    results.Add((i, SearchMeanPercentForSize(i, matrixQuantity, averChange)));

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            });
            task.Wait();

            var service = new ExperimentService();
            service.AddExperimentResult(results, _generator, averChange);

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
