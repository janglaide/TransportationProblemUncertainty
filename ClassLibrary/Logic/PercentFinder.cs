using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class PercentFinder
    {
        public delegate int PercentDelegate(SearchParameters parameters, Solver solver, Random rand);
        public static double SearchMeanPercent(PercentDelegate SearchPercent, SearchParameters parameters, double averChange, Solver solver, Random rand)
        {
            double average = 0;
            int runNumber = 0;
            int accuracyAmount = 0;
            int runFinish;
            do
            {
                double diff = runNumber != 0 ? average / runNumber : 0;
                average += SearchPercent(parameters, solver, rand);
                runNumber++;
                diff = Math.Abs(average / runNumber - diff);
                accuracyAmount = (diff < averChange) ? accuracyAmount + 1 : 0;
            } while (accuracyAmount < 10);
            runFinish = runNumber * 10;
            /*
            for (; runNumber <= runFinish; runNumber++) //simple
            {
                average += SearchPercent(parameters, solver, rand);
            }
            */
            var iterations = runFinish - runNumber;
            var quantity = Environment.ProcessorCount;
            var step = (int)Math.Ceiling((double)iterations / quantity);
            runNumber += step * quantity;

            int threadsSum = 0;
            Parallel.For(0, quantity, i =>
                {
                    int sumLocal = 0;
                    var solverLocal = new Solver();
                    var randLocal = new Random();
                    for (var j = 0; j < step; j++)
                    {
                        sumLocal += SearchPercent(parameters, solverLocal, randLocal);
                    }
                    Interlocked.Add(ref threadsSum, sumLocal);
                }
            );
            
            average += threadsSum;
            average /= runNumber;
            return average;
        }
        
        public static int FindPercentOfChange(SearchParameters parameters, Solver solver, Random rand)
        {
            if (!(parameters is ParametersForDefined))
            {
                throw new ArgumentException("Wrong parameters type in method PercentFinder.ParametersForDefined. Need to be ParametersForDefined.");
            }
            var param = (ParametersForDefined)parameters;
            int percent = 0;
            bool change = false;
            int cNumber = param.Cs.Length;
            double[] newX = new double[param.OldX.Length];
            double[] roundedOldX = solver.RoundVector(param.OldX);
            double[] selectedValues = null;
            double[] solutions;
            double[][] changedCs = new double[cNumber][];
            param.OldX.CopyTo(newX, 0);

            switch (parameters.CChangeParameters)
            {
                case CChangeParameters.Default:
                    selectedValues = GetAll(roundedOldX);
                    break;
                case CChangeParameters.Basic:
                    selectedValues = GetBasic(roundedOldX);
                    break;
                case CChangeParameters.NonBasic:
                    selectedValues = GetNonBasic(roundedOldX);
                    break;
            }

            while (!change)
            {
                if (solver.DivideX(solver.RoundVector(newX), cNumber).SequenceEqual(solver.DivideX(roundedOldX, cNumber)))
                {
                    CopyMultidimensional(param.Cs, ref changedCs);
                    percent++;
                    ChangeMatrixs(ref changedCs, percent, selectedValues, rand);
                    (_, solutions) = solver.GetSolutions(changedCs, param.A, param.B);
                    (newX, _) = solver.SolveSeveral(changedCs, param.A, param.B, param.L, param.Alpha, solutions);
                }
                else
                {
                    change = true;
                    changedCs.CopyTo(param.Cs, 0);
                }
            }
            param.DefineXs(newX);
            return percent;
        }

        public static void CopyMultidimensional(double[][] from, ref double[][] to)
        {
            int size = from.Length;
            for (int i = 0; i < size; i++)
            {
                to[i] = new double[from[i].Length];
                from[i].CopyTo(to[i], 0);
            }
        }
        private static void ChangeMatrixs(ref double[][] cs, double percent, double[] selected, Random rand)
        {
            for (int k = 0; k < cs.Length; k++)
            {
                int size = cs[k].Length;
                for (int i = 0; i < size; i++)
                {
                    double e = cs[k][i] * (percent / 100);
                    cs[k][i] += GeneratorValues.GetDoubleValue("Uniform", (-e, e), rand) * selected[i];
                }
            }
        }
        private static double[] GetBasic(double[] x)
        {
            int xNumber = x.Length;
            double[] basic = new double[xNumber];
            for (int i = 0; i < xNumber; i++)
            {
                basic[i] = (x[i] != 0) ? 1 : 0;
            }
            return basic;
        }
        private static double[] GetNonBasic(double[] x)
        {
            int xNumber = x.Length;
            double[] basic = new double[xNumber];
            for (int i = 0; i < xNumber; i++)
            {
                basic[i] = (x[i] != 0) ? 0 : 1;
            }
            return basic;
        }
        private static double[] GetAll(double[] x)
        {
            int xNumber = x.Length;
            double[] all = new double[xNumber];
            for (int i = 0; i < xNumber; i++)
            {
                all[i] = 1;
            }
            return all;
        }
    }
}
