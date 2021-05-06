using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class PercentFinder
    {
        public delegate double PercentDelegate(SearchParameters parameters);
        public static double SearchMeanPercentForDefinedCondition(PercentDelegate SearchPercent, SearchParameters parameters, double averChange)
        {
            double average = 0;
            int runNumber = 0;
            int accuracyAmount = 0;
            int runFinish;
            do
            {
                double diff = runNumber != 0 ? average / runNumber : 0;
                average += SearchPercent(parameters);
                runNumber++;
                diff = Math.Abs(average / runNumber - diff);
                accuracyAmount = (diff < averChange) ? accuracyAmount + 1 : 0;
            } while (accuracyAmount < 10);
            runFinish = runNumber * 10;
            /*
            for(; runNumber <= runFinish; runNumber++) //simple
            {
                average += SearchPercent(parameters);
            }
            */

                                                      //threads try (unsuccessfull)
            var iterations = runFinish - runNumber;
            var quantity = 10;
            int step = iterations / quantity;

            Task[] tasks = new Task[quantity];
            double[] averages = new double[quantity];

            for (var i = 0; i < quantity; i++)
            {
                var localThread = i;
                averages[localThread] = 0.0;
                tasks[localThread] = Task.Run(() => { 
                    for(var j = 0; j < step; j++)
                    {
                        averages[localThread] += SearchPercent(parameters);
                    }
                });
            }
            //Task.WaitAll(tasks);                // crash (focus on the window)
            //await Task.WhenAll(tasks);
            foreach (var counter in averages)
            {
                average += counter;
            }
            

            return average / runNumber;
        }
        
        public static double FindPercentOfChange(SearchParameters parameters)
        {
            if (!(parameters is ParametersForDefined))
            {
                throw new ArgumentException("Wrong parameters type in method PercentFinder.ParametersForDefined. Need to be ParametersForDefined.");
            }
            ParametersForDefined param = (ParametersForDefined)parameters;
            double percent = 0;
            bool change = false;
            int cNumber = param.Cs.Length;
            double[] newX = new double[param.OldX.Length];
            double[] roundedOldX = Solver.RoundVector(param.OldX);
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
                if (Solver.DivideX(Solver.RoundVector(newX), cNumber).SequenceEqual(Solver.DivideX(roundedOldX, cNumber)))
                {
                    CopyMultidimensional(param.Cs, ref changedCs);
                    percent++;
                    ChangeMatrixs(ref changedCs, percent, selectedValues);
                    (_, solutions) = Solver.GetSolutions(changedCs, param.A, param.B);
                    (newX, _) = Solver.SolveSeveral(changedCs, param.A, param.B, param.L, param.Alpha, solutions);
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
        private static void ChangeMatrixs(ref double[][] cs, double percent, double[] selected)
        {
            for (int k = 0; k < cs.Length; k++)
            {
                int size = cs[k].Length;
                for (int i = 0; i < size; i++)
                {
                    double e = cs[k][i] * (percent / 100);
                    cs[k][i] += GeneratorValues.GetDoubleValue("unif", (-e, e)) * selected[i];
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
