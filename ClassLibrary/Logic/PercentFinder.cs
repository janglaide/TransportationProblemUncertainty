using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Enums;
using ClassLibrary.Generators;
using ClassLibrary.MethodParameters;

namespace ClassLibrary.Logic
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
                parameters.Clear();
                runNumber++;
                diff = Math.Abs(average / runNumber - diff);
                accuracyAmount = (diff < averChange) ? accuracyAmount + 1 : 0;
            } while (accuracyAmount < 10);
            runFinish = runNumber * 10;

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
                    var paramsLocal = parameters.Copy();
                    for (var j = 0; j < step; j++)
                    {
                        sumLocal += SearchPercent(paramsLocal, solverLocal, randLocal);
                        paramsLocal.Clear();
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
            double[] result = new double[param.OldX.Length];
            double[] oldX = param.OldX;
            double[] selectedValues = null;
            double[] solutions;
            double[][] changedCs = new double[cNumber][];
            param.OldX.CopyTo(newX, 0);
            param.OldX.CopyTo(result, 0);

            switch (parameters.CChangeParameters)
            {
                case CChangeParameters.Default:
                    selectedValues = GetAll(oldX);
                    break;
                case CChangeParameters.Basic:
                    selectedValues = GetBasic(oldX);
                    break;
                case CChangeParameters.NonBasic:
                    selectedValues = GetNonBasic(oldX);
                    break;
            }

            while (!change)
            {
                if (newX.SequenceEqual(oldX))
                {
                    CopyMultidimensional(param.Cs, ref changedCs);
                    percent++;
                    ChangeMatrixs(ref changedCs, percent, selectedValues, rand);
                    (_, solutions) = solver.GetSolutions(changedCs, param.A, param.B);
                    (newX, _) = solver.SolveSeveral(changedCs, param.A, param.B, param.L, param.Alpha, solutions);
                    result = newX;
                    newX = solver.DivideX(solver.RoundVector(newX), cNumber);
                }
                else
                {
                    change = true;
                    changedCs.CopyTo(param.Cs, 0);
                }
            }
            param.DefineXs(result);
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
        private static void ChangeMatrixs(ref double[][] cs, double percent, double[] selected, Random random)
        {
            for (int k = 0; k < cs.Length; k++)
            {
                int size = cs[k].Length;
                for (int i = 0; i < size; i++)
                {
                    double e = cs[k][i] * (percent / 100);
                    cs[k][i] += GeneratorValues.GetDoubleValue("Uniform", (-e, e), random) * selected[i];
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
        public static (double, double) GetCsRange(ParametersForDefined parameters)
        {
            double averMin = 0.0, averMax = 0.0;
            foreach (var c in parameters.Cs)
            {
                var min = double.MaxValue;
                var max = double.MinValue;
                foreach (var x in c)
                {
                    if (x < min)
                        min = x;

                    if (x > max)
                        max = x;
                }
                averMin += min;
                averMax += max;
            }
            return ((averMin / parameters.Cs.Length), (averMax / parameters.Cs.Length));
        }
        public static (double, double) GetABRange(ParametersForDefined parameters)
        {
            double minA = double.MaxValue, maxA = double.MinValue, minB = double.MaxValue, maxB = double.MinValue;

            for (var i = 0; i < parameters.A.Length; i++)
            {
                if (parameters.A[i] < minA)
                    minA = parameters.A[i];

                if (parameters.A[i] > maxA)
                    maxA = parameters.A[i];

                if (parameters.B[i] < minB)
                    minB = parameters.B[i];

                if (parameters.B[i] > maxB)
                    maxB = parameters.B[i];
            }

            return (((minA + minB) / 2), ((maxA + maxB) / 2));
        }

        public static (double, double) GetLRange(ParametersForDefined parameters)
        {
            double min = double.MaxValue, max = double.MinValue;

            foreach (var l in parameters.L)
            {
                if (l < min)
                    min = l;

                if (l > max)
                    max = l;
            }

            return (min, max);
        }
    }
}
