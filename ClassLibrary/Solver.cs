using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenterSpace.NMath.Analysis;
using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public class Solver
    {
        public static DualSimplexSolver SolveSeveral(string[] coeffC, DoubleVector vectorA, DoubleVector vectorB, DoubleVector vectorL)
        {
            int rows, columns, xQuantity, yQuantity, fullQuantity;
            rows = vectorA.Length;
            columns = vectorB.Length;
            xQuantity = rows * columns;
            yQuantity = coeffC.Length;
            fullQuantity = xQuantity + yQuantity;

            List<double> solutions = new List<double>();
            for (int i = 0; i < yQuantity; i++)
            {
                var solve = SolveOne(new DoubleVector(coeffC[i]), vectorA, vectorB);
                if (solve.Result.ToString() == "Optimal")
                {
                    solutions.Add(RoundBitch(solve.OptimalObjectiveFunctionValue));
                }
            } // а если solutions.Lenght != coeffC.Lenght, то хз, гг

            var solver = new DualSimplexSolver();
            var solverParams = new DualSimplexSolverParams
            {
                Minimize = true
            };

            var coeffZ = new DoubleVector(CreateSuperZ(xQuantity, yQuantity));

            var problem = new MixedIntegerLinearProgrammingProblem(coeffZ);
            for (int i = 0; i < rows; i++)
            {
                var coeffA = new DoubleVector(CreateSuperXRowBitch(rows, columns, i, yQuantity));
                var constraintA = new LinearConstraint(coeffA, vectorA[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = new DoubleVector(CreateSuperXColumnBitch(rows, columns, j, yQuantity));
                var constraintB = new LinearConstraint(coeffB, vectorB[j], ConstraintType.EqualTo);
                problem.AddConstraint(constraintB);
            }
            for (int i = 0; i < fullQuantity; i++)
            {
                var constraint = new LinearConstraint(new DoubleVector(CreateValueBitch(i, fullQuantity + yQuantity)), 0, ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            for (int i = fullQuantity; i < fullQuantity + yQuantity; i++)
            {
                var constraint = new LinearConstraint(new DoubleVector(CreateValueBitch(i, fullQuantity + yQuantity)), 1, ConstraintType.EqualTo);
                problem.AddConstraint(constraint);
            }
            for (int i = 0; i < yQuantity; i++)
            {
                var constraint = new LinearConstraint(new DoubleVector(CreateYRowBitch(i, yQuantity, Cs.C[i], solutions[i])), -vectorL[i], ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            solver.Solve(problem, solverParams);
            return solver;
        }
        public static DualSimplexSolver SolveOne(DoubleVector coeffC, DoubleVector vectorA, DoubleVector vectorB)
        {
            int rows, columns, xQuantity;
            rows = vectorA.Length;
            columns = vectorB.Length;
            xQuantity = rows * columns;

            var solver = new DualSimplexSolver();
            var solverParams = new DualSimplexSolverParams
            {
                Minimize = true
            };

            var problem = new MixedIntegerLinearProgrammingProblem(coeffC);
            for (int i = 0; i < rows; i++)
            {
                var coeffA = new DoubleVector(CreateUsualXRowBitch(rows, columns, i));
                var constraintA = new LinearConstraint(coeffA, vectorA[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = new DoubleVector(CreateUsualXColumnBitch(rows, columns, j));
                var constraintB = new LinearConstraint(coeffB, vectorB[j], ConstraintType.EqualTo);
                problem.AddConstraint(constraintB);
            }
            for (int i = 0; i < xQuantity; i++)
            {
                var constraint = new LinearConstraint(new DoubleVector(CreateValueBitch(i, xQuantity)), 0, ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            solver.Solve(problem, solverParams);
            return solver;
        }
        public static void PrintOptimalX(DoubleVector x, int columns)
        {
            int xQuantity = x.Length;
            for (int i = 0; i < xQuantity; i++)
            {
                double number = RoundBitch(x[i]);

                if (number != 0)
                {
                    Console.Write($"{number}\t");
                }
                else
                {
                    Console.Write($"-\t");
                }

                if ((i + 1) % columns == 0)
                {
                    Console.WriteLine();
                }

            }
        }
        public static double RoundBitch(double value)
        {
            double eps = 0.00001;
            double roundedValue = Math.Round(value, 0);
            if (Math.Abs(roundedValue - value) < eps)
            {
                return roundedValue;
            }
            else
            {
                return Math.Round(value, 5);
            }
        }
        public static string CreateSuperZ(int xQuantity, int yQuantity)
        {
            string newZ = "";
            for (int i = 0; i < xQuantity; i++)
            {
                newZ += "0, ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                newZ += "1, ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                newZ += "0";
                if (i < yQuantity - 1)
                {
                    newZ += ", ";
                }
            }
            return newZ;
        }
        public static string CreateUsualXRowBitch(int rows, int columns, int row)
        {
            string result = "";
            int xQuantity = rows * columns;
            for (int i = 0; i < xQuantity; i++)
            {
                int num = 0;
                if (i >= columns * row && i < columns * (row + 1))
                {
                    num = 1;
                }
                result += $"{num}";
                if (i < xQuantity - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
        public static string CreateSuperXRowBitch(int rows, int columns, int row, int yQuantity)
        {
            string result = CreateUsualXRowBitch(rows, columns, row);
            AddZerosToSuperString(ref result, yQuantity);
            return result;
        }
        public static string AddZerosToSuperString(ref string value, int yQuantity)
        {
            value += ", ";
            for (int i = 0; i < yQuantity * 2; i++)
            {
                value += $"0";
                if (i < yQuantity * 2 - 1)
                {
                    value += ", ";
                }
            }
            return value;
        }
        public static string CreateUsualXColumnBitch(int rows, int columns, int column)
        {
            string result = "";
            int xQuantity = rows * columns;
            for (int i = 0; i < xQuantity; i++)
            {
                int num = 0;
                if (i % columns == column)
                {
                    num = 1;
                }
                result += $"{num}";
                if (i < xQuantity - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
        public static string CreateSuperXColumnBitch(int rows, int columns, int column, int yQuantity)
        {
            string result = CreateUsualXColumnBitch(rows, columns, column);
            AddZerosToSuperString(ref result, yQuantity);
            return result;
        }
        public static string CreateValueBitch(int value, int valueQuantity)
        {
            string result = "";
            for (int i = 0; i < valueQuantity; i++)
            {
                int num = 0;
                if (i == value)
                {
                    num = 1;
                }
                result += $"{num}";
                if (i < valueQuantity - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
        public static string CreateYRowBitch(int row, int yQuantity, string C, double solution)
        {
            C = C.Replace(" ", " -").Replace("[", "[-");
            string result = C.Substring(0, C.Length - 1) + ", ";
            for (int i = 0; i < yQuantity; i++)
            {
                int num = 0;
                if (i == row)
                {
                    num = 1;
                }
                result += $"{num}, ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                double num = 0;
                if (i == row)
                {
                    num = solution;
                }
                result += $"{num}";
                if (i < yQuantity - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
    }
    public static class Cs
    {
        public static string[] C = new string[3] {
            "[9, 13, 8, 8, 14, 14, 14, 3, 5, 11, 9, 3, " +
            "11, 12, 5, 1, 4, 15, 6, 14, 3, 6, 1, 6, " +
            "1, 9, 4, 14, 7, 1, 9, 4, 8, 15, 10, 14, " +
            "14, 12, 9, 8, 11, 5, 1, 5, 13, 8, 2, 1, " +
            "6, 13, 10, 4, 4, 12, 10, 12, 13, 12, 1, 6, " +
            "8, 4, 13, 13, 6, 5, 2, 2, 14, 2, 6, 12, " +
            "15, 3, 13, 4, 5, 9, 8, 13, 15, 2, 4, 14, " +
            "10, 9, 3, 15, 6, 14, 13, 12, 10, 1, 12, 1, " +
            "4, 8, 1, 9, 7, 6, 11, 15, 5, 12, 7, 5, " +
            "15, 12, 8, 5, 12, 1, 6, 9, 11, 9, 10, 8, " +
            "5, 1, 15, 5, 14, 10, 5, 15, 1, 1, 4, 2, " +
            "3, 12, 14, 14, 13, 12, 3, 8, 15, 8, 12, 1, " +
            "6, 5, 11, 1, 12, 1, 12, 2, 12, 15, 10, 9, " +
            "3, 14, 1, 15, 1, 11, 6, 1, 9, 4, 3, 14, " +
            "13, 7, 10, 7, 3, 6, 8, 12, 13, 13, 5, 2]"
            ,
            "[1, 5, 7, 5, 14, 13, 15, 3, 15, 13, 4, 9, " +
            "8, 10, 12, 11, 15, 8, 5, 11, 11, 14, 6, 3, " +
            "8, 4, 10, 4, 8, 9, 5, 12, 8, 5, 7, 7, " +
            "14, 8, 6, 7, 10, 12, 6, 12, 8, 1, 2, 12, " +
            "15, 12, 12, 12, 9, 10, 12, 8, 6, 8, 12, 7, " +
            "13, 11, 6, 9, 11, 15, 9, 13, 5, 3, 1, 9, " +
            "5, 8, 12, 12, 15, 11, 5, 5, 12, 6, 15, 9, " +
            "14, 3, 11, 10, 12, 14, 15, 6, 13, 6, 3, 15, " +
            "2, 11, 2, 8, 7, 13, 13, 11, 15, 9, 10, 7, " +
            "10, 1, 10, 10, 3, 1, 7, 6, 8, 8, 13, 13, " +
            "5, 11, 9, 14, 9, 5, 1, 5, 13, 3, 4, 6, " +
            "5, 11, 1, 3, 1, 7, 15, 2, 11, 8, 5, 7, " +
            "13, 13, 2, 13, 13, 7, 15, 4, 2, 9, 5, 14, " +
            "12, 15, 1, 7, 5, 1, 8, 2, 10, 1, 1, 12, " +
            "1, 4, 6, 11, 3, 5, 8, 3, 4, 3, 7, 12]"
            ,
            "[10, 9, 2, 2, 12, 10, 9, 6, 10, 15, 15, 3, " +
            "15, 6, 3, 15, 3, 7, 13, 15, 14, 9, 13, 3, " +
            "12, 14, 2, 12, 3, 6, 11, 4, 7, 15, 15, 3, " +
            "3, 8, 10, 14, 5, 15, 12, 12, 10, 8, 4, 4, " +
            "7, 6, 2, 9, 15, 2, 15, 5, 15, 4, 1, 3, " +
            "3, 8, 7, 3, 13, 15, 3, 7, 12, 14, 8, 11, " +
            "15, 2, 14, 12, 11, 14, 3, 12, 2, 1, 14, 5, " +
            "6, 15, 8, 14, 13, 8, 12, 15, 14, 12, 1, 14, " +
            "2, 13, 2, 7, 14, 7, 15, 3, 4, 11, 9, 11, " +
            "6, 3, 15, 9, 15, 8, 1, 12, 6, 4, 15, 8, " +
            "4, 3, 14, 9, 12, 4, 14, 11, 4, 3, 3, 11, " +
            "11, 11, 6, 1, 5, 3, 13, 7, 1, 1, 5, 7, " +
            "4, 5, 9, 11, 8, 8, 9, 3, 12, 3, 8, 1, " +
            "13, 4, 2, 7, 7, 3, 8, 15, 8, 7, 12, 10, " +
            "12, 7, 9, 2, 5, 12, 4, 7, 3, 9, 1, 4]" };
    }
}