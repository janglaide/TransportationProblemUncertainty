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
        private string distribution;
        private (double, double) parametersC;
        private (double, double) parametersAB;
        private (double, double) parametersL;

        public Experiment(string distr, (double, double) paramC, (double, double) paramAB, (double, double) paramL)
        {
            distribution = distr;
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
                    matrix += $"{generator.GetDoubleValue(distribution, parametersC)} ";
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
                    a.Add(generator.GetIntValue(distribution, parametersAB));
                    if (i < size - 1)
                    {
                        b.Add(generator.GetIntValue(distribution, parametersAB));
                    }
                }
                double value = a.Sum() - b.Sum();
                if (value >= 0)
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
                l += $"{Math.Round(generator.GetDoubleValue(distribution, parametersL))} ";
            }
            return new DoubleVector(l);
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
            double average = 0;
            double diff;
            int runAmount = 0;
            do
            {
                diff = runAmount != 0 ? average / runAmount : 0;
                average += FindPercentOfChange(size, matrixQuantity, a, b, l);
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
        private double FindPercentOfChange(int size, int matrixQuantity, DoubleVector a, DoubleVector b, DoubleVector l)
        {
            double percent = 0;
            bool change = false;
            while (!change)
            {
                DoubleVector[] cs = new DoubleVector[matrixQuantity];
                List<double> solutions;
                DoubleVector newX, oldX;

                for (int i = 0; i < matrixQuantity; i++)
                {
                    cs[i] = GenerateMatrix(size);
                }
                solutions = Solver.GetSolutions(cs, a, b);
                DualSimplexSolver solution = Solver.SolveSeveral(cs, a, b, l, solutions);
                newX = Solver.RoundMatrix(Solver.DivideX(solution.OptimalX, matrixQuantity));
                if (!Solver.CheckABConstraints(newX, a, b))
                {
                    continue;
                }
                oldX = newX;
                while (!change)
                {
                    if (newX == oldX)
                    {
                        percent++;
                        ChangeMatrixs(ref cs, 1);
                        solutions = Solver.GetSolutions(cs, a, b);
                        solution = Solver.SolveSeveral(cs, a, b, l, solutions);
                        newX = Solver.RoundMatrix(Solver.DivideX(solution.OptimalX, matrixQuantity));
                    }
                    else
                    {
                        change = true;
                    }
                }
            }
            return percent;
        }
    }
}