using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public class Experiment
    {
        private readonly Generator generator = new Generator();
        private readonly string distributionC;
        public string DistributionC
        {
            get { return distributionC; }
        }
        private readonly string distributionAB;
        public string DistributionAB
        {
            get { return distributionAB; }
        }
        private readonly string distributionL;
        public string DistributionL
        {
            get { return distributionL; }
        }
        private readonly (double, double) parametersC;
        private readonly (double, double) parametersAB;
        private readonly (double, double) parametersL;

        public Experiment((string, string, string) distribution, (double, double) paramC, (double, double) paramAB, (double, double) paramL)
        {
            distributionC = distribution.Item1;
            distributionAB = distribution.Item2;
            distributionL = distribution.Item3;
            parametersC = paramC;
            parametersAB = paramAB;
            parametersL = paramL;
        }
        public double[] GenerateMatrix(int size)
        {
            int fullSize = size * size;
            double[] matrix = new double[fullSize];
            for (int i = 0; i < fullSize; i++)
            {
                matrix[i] = generator.GetDoubleValue(distributionC, parametersC);

            }
            return matrix;
        }
        public (double[], double[]) GenerateAB(int size)
        {
            bool success = false;
            double[] a, b;
            do
            {
                a = new double[size];
                b = new double[size];
                for (int i = 0; i < size; i++)
                {
                    a[i] = generator.GetIntValue(distributionAB, parametersAB);
                    if (i < size - 1)
                    {
                        b[i] = generator.GetIntValue(distributionAB, parametersAB);
                    }
                }
                double value = a.Sum() - b.Sum();
                if (value > 0)
                {
                    success = true;
                    b[size - 1] = value;
                }
            } while (!success);

            return (a, b);
        }
        public double[] GenerateL(int quantity)
        {
            double[] l = new double[quantity];
            for (int i = 0; i < quantity; i++)
            {
                l[i] = Math.Round(generator.GetDoubleValue(distributionL, parametersL));
            }
            return l;
        }
        public double[] GenerateAlpha(int quantity)
        {
            double[] alpha = new double[quantity];
            for (int i = 0; i < quantity; i++)
            {
                alpha[i] = 1;
            }
            return alpha;
        }
        private void ChangeMatrixs(ref double[][] cs, double percent)
        {
            for (int k = 0; k < cs.Length; k++)
            {
                int size = cs[k].Length;
                for (int i = 0; i < size; i++)
                {
                    double e = cs[k][i] * (percent / 100);
                    cs[k][i] += generator.GetDoubleValue("unif", (-e, e));
                }
            }
        }
        private double SearchMeanPercent(int size, int matrixQuantity, double averChange)
        {
            (double[] a, double[] b) = GenerateAB(size);
            double[] l = GenerateL(matrixQuantity);
            double[] alpha = GenerateAlpha(matrixQuantity);
            double average = 0;
            double diff;
            int runNumber = 0;
            int accuracyAmount = 0;
            int runFinish = int.MaxValue;
            do
            {
                diff = runNumber != 0 ? average / runNumber : 0;
                double percent;
                percent = GetPercentOfChange(size, matrixQuantity, a, b, l, alpha);
                average += percent;
                runNumber++;
                diff = Math.Abs(average / runNumber - diff);
                accuracyAmount = (diff < averChange) ? accuracyAmount + 1 : 0;
                if (accuracyAmount == 10 && runFinish == int.MaxValue)
                {
                    runFinish = runNumber * 10;
                }
            } while (runNumber != runFinish);
            return average / runNumber;
        }
        public List<(int, double)> RunExperiment(int startSize, int finishSize, int step, int matrixQuantity, double averChange)
        {
            List<(int, double)> results = new List<(int, double)>();
            for (int i = startSize; i <= finishSize; i += step)
            {
                results.Add((i, SearchMeanPercent(i, matrixQuantity, averChange)));
            }
            return results;
        }
        private double GetPercentOfChange(int size, int matrixQuantity, double[] a, double[] b, double[] l, double[] alpha)
        {
            double percent;
            double[] x = new double[size * size];
            double[][] cs = new double[matrixQuantity][];
            double[] solutions;

            bool success = false;
            while (!success)
            {
                for (int i = 0; i < matrixQuantity; i++)
                {
                    cs[i] = GenerateMatrix(size);
                }
                (_, solutions) = Solver.GetSolutions(cs, a, b);
                (x, _) = Solver.SolveSeveral(cs, a, b, l, alpha, solutions);
                if (!Solver.CheckABConstraints(Solver.DivideX(Solver.RoundVector(x), matrixQuantity), a, b))
                {
                    continue;
                }
                success = true;
            }
            (percent, _) = FindPercentOfChange(x, ref cs, a, b, l, alpha);
            return percent;
        }
        public (double, double[]) FindPercentOfChange(double[] oldX, ref double[][] cs, double[] a, double[] b, double[] l, double[] alpha)
        {
            double percent = 0;
            bool change = false;
            int cNumber = cs.Length;
            double[] newX = new double[oldX.Length];
            double[] solutions;
            double[][] changedCs = new double[cNumber][];
            oldX.CopyTo(newX, 0);

            while (!change)
            {
                if (Solver.DivideX(Solver.RoundVector(newX), cNumber).SequenceEqual(Solver.DivideX(Solver.RoundVector(oldX), cNumber)))
                {
                    CopyMultidimensional(cs, ref changedCs);
                    percent++;
                    ChangeMatrixs(ref changedCs, percent);
                    (_, solutions) = Solver.GetSolutions(changedCs, a, b);
                    (newX, _) = Solver.SolveSeveral(changedCs, a, b, l, alpha, solutions);
                }
                else
                {
                    change = true;
                    changedCs.CopyTo(cs, 0);
                }
            }
            return (percent, newX);
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
    }
}
