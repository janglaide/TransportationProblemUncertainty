using CenterSpace.NMath.Analysis;
using CenterSpace.NMath.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public class Experiment
    {
        private Generator generator = new Generator();
        private string distributionC;
        public string DistributionC
        {
            get { return distributionC; }
        }
        private string distributionAB;
        public string DistributionAB
        {
            get { return distributionAB; }
        }
        private string distributionL;
        public string DistributionL
        {
            get { return distributionL; }
        }
        private (double, double) parametersC;
        private (double, double) parametersAB;
        private (double, double) parametersL;

        public Experiment((string, string, string) distribution, (double, double) paramC, (double, double) paramAB, (double, double) paramL)
        {
            distributionC = distribution.Item1;
            distributionAB = distribution.Item2;
            distributionL = distribution.Item3;
            parametersC = paramC;
            parametersAB = paramAB;
            parametersL = paramL;
        }
        public DoubleVector GenerateMatrix(int size)
        {
            string matrix = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix += $"{generator.GetDoubleValue(distributionC, parametersC)} ";
                }
            }
            return new DoubleVector(matrix);
        }
        public (DoubleVector, DoubleVector) GenerateAB(int size)
        {
            bool success = false;
            List<double> a, b;
            do
            {
                a = new List<double>();
                b = new List<double>();
                for (int i = 0; i < size; i++)
                {
                    a.Add(generator.GetIntValue(distributionAB, parametersAB));
                    if (i < size - 1)
                    {
                        b.Add(generator.GetIntValue(distributionAB, parametersAB));
                    }
                }
                double value = a.Sum() - b.Sum();
                if (value > 0)
                {
                    success = true;
                    b.Add(value);
                }
            } while (!success);
            string strA = "";
            string strB = "";
            a.ForEach(x => strA += $"{x} ");
            b.ForEach(x => strB += $"{x} ");

            return (new DoubleVector(strA), new DoubleVector(strB));
        }
        public DoubleVector GenerateL(int quantity)
        {
            string l = "";
            for (int i = 0; i < quantity; i++)
            {
                l += $"{Math.Round(generator.GetDoubleValue(distributionL, parametersL))} ";
            }
            return new DoubleVector(l);
        }
        public DoubleVector GenerateAlpha(int quantity)
        {
            string alpha = "";
            for (int i = 0; i < quantity; i++)
            {
                alpha += $"1 ";
            }
            return new DoubleVector(alpha);
        }
        private void ChangeMatrixs(ref DoubleVector[] cs, double percent)
        {
            for (int k = 0; k < cs.Length; k++)
            {
                int size = (int)Math.Round(Math.Sqrt(cs[k].Length));
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        double e = cs[k][i * size + j] * (percent / 100);
                        cs[k][i * size + j] += generator.GetDoubleValue("unif", (-e, e));
                    }
                }
            }
        }
        private double SearchMeanPercent(int size, int matrixQuantity, double averChange)
        {
            (DoubleVector a, DoubleVector b) = GenerateAB(size);
            DoubleVector l = GenerateL(matrixQuantity);
            DoubleVector alpha = GenerateAlpha(matrixQuantity);
            double average = 0;
            double diff;
            int runAmount = 0;
            do
            {
                diff = runAmount != 0 ? average / runAmount : 0;
                double percent;
                percent = GetPercentOfChange(size, matrixQuantity, a, b, l, alpha);
                average += percent;
                runAmount++;
                diff = Math.Abs(average / runAmount - diff);
            } while (!(diff < averChange && runAmount > 10));
            return average / runAmount;
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
        private double GetPercentOfChange(int size, int matrixQuantity, DoubleVector a, DoubleVector b, DoubleVector l, DoubleVector alpha)
        {
            double percent;
            DoubleVector x = new DoubleVector();

            DoubleVector[] cs = new DoubleVector[matrixQuantity];
            DoubleVector solutions;

            bool success = false;
            while (!success)
            {
                for (int i = 0; i < matrixQuantity; i++)
                {
                    cs[i] = GenerateMatrix(size);
                }
                (_, solutions) = Solver.GetSolutions(cs, a, b);
                DualSimplexSolver solution = Solver.SolveSeveral(cs, a, b, l, alpha, solutions);
                x = Solver.DivideX(Solver.RoundMatrix(solution.OptimalX), matrixQuantity);
                if (!Solver.CheckABConstraints(x, a, b))
                {
                    continue;
                }
                success = true;
            }
            (percent, _) = FindPercentOfChange(x, ref cs, a, b, l, alpha);
            return percent;
        }
        public (double, DoubleVector) FindPercentOfChange(DoubleVector oldX, ref DoubleVector[] cs, DoubleVector a, DoubleVector b, DoubleVector l, DoubleVector alpha)
        {
            double percent = 0;
            DoubleVector newX = new DoubleVector();
            DoubleVector solutions;
            bool change = false;
            DualSimplexSolver solution;
            DoubleVector compareX = oldX;

            while (!change)
            {
                if (compareX == oldX)
                {
                    percent++;
                    ChangeMatrixs(ref cs, 1);
                    (_, solutions) = Solver.GetSolutions(cs, a, b);
                    solution = Solver.SolveSeveral(cs, a, b, l, alpha, solutions);
                    newX = solution.OptimalX;
                    compareX = Solver.DivideX(Solver.RoundMatrix(newX), cs.Length);
                }
                else
                {
                    change = true;
                }
            }
            return (percent, newX);
        }
    }
}