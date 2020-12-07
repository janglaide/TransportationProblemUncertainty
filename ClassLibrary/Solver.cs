using System;
using System.Collections.Generic;
using CenterSpace.NMath.Analysis;
using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public static class Solver
    {
        public static DualSimplexSolver SolveSeveral(DoubleVector[] cs, DoubleVector a, DoubleVector b, DoubleVector l, List<double> solutions)
        {
            int rows, columns, xQuantity, yQuantity, fullQuantity;
            rows = a.Length;
            columns = b.Length;
            xQuantity = rows * columns;
            yQuantity = cs.Length;
            fullQuantity = xQuantity + yQuantity;

            DualSimplexSolver solver = new DualSimplexSolver();
            var solverParams = new DualSimplexSolverParams
            {
                Minimize = true
            };

            var coeffZ = CreateSuperZ(xQuantity, yQuantity);

            var problem = new MixedIntegerLinearProgrammingProblem(coeffZ);
            for (int i = 0; i < rows; i++)
            {
                var coeffA = CreateSuperXRow(rows, columns, i, yQuantity);
                var constraintA = new LinearConstraint(coeffA, a[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = CreateSuperXColumn(rows, columns, j, yQuantity);
                var constraintB = new LinearConstraint(coeffB, b[j], ConstraintType.EqualTo);
                problem.AddConstraint(constraintB);
            }
            for (int i = 0; i < fullQuantity; i++)
            {
                var constraint = new LinearConstraint(CreateValueRow(i, fullQuantity + yQuantity), 0, ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            for (int i = fullQuantity; i < fullQuantity + yQuantity; i++)
            {
                var constraint = new LinearConstraint(CreateValueRow(i, fullQuantity + yQuantity), 1, ConstraintType.EqualTo);
                problem.AddConstraint(constraint);
            }
            for (int i = 0; i < yQuantity; i++)
            {
                var constraint = new LinearConstraint(CreateYRow(i, yQuantity, cs[i], solutions[i]), -l[i], ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            solver.Solve(problem, solverParams);
            return solver;
        }
        public static List<double> GetSolutions(DoubleVector[] cs, DoubleVector a, DoubleVector b)
        {
            List<double> solutions = new List<double>();
            for (int i = 0; i < cs.Length; i++)
            {
                DoubleVector z = new DoubleVector(cs[i]);
                var solver = Solver.SolveOne(z, a, b);
                if (solver.OptimalX.Length != 0)
                {
                    solutions.Add(Solver.RoundValue(solver.OptimalObjectiveFunctionValue));

                }
            }
            return solutions;
        }
        public static DualSimplexSolver SolveOne(DoubleVector c, DoubleVector a, DoubleVector b)
        {
            int rows, columns, xQuantity;
            rows = a.Length;
            columns = b.Length;
            xQuantity = rows * columns;

            DualSimplexSolver solver = new DualSimplexSolver();
            var solverParams = new DualSimplexSolverParams
            {
                Minimize = true
            };

            var problem = new MixedIntegerLinearProgrammingProblem(c);
            for (int i = 0; i < rows; i++)
            {
                var coeffA = CreateUsualXRow(rows, columns, i);
                var constraintA = new LinearConstraint(coeffA, a[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = CreateUsualXColumn(rows, columns, j);
                var constraintB = new LinearConstraint(coeffB, b[j], ConstraintType.EqualTo);
                problem.AddConstraint(constraintB);
            }
            for (int i = 0; i < xQuantity; i++)
            {
                var constraint = new LinearConstraint(CreateValueRow(i, xQuantity), 0, ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            solver.Solve(problem, solverParams);
            return solver;
        }
        public static double RoundValue(double value)
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
        private static DoubleVector CreateSuperZ(int xQuantity, int yQuantity)
        {
            string newZ = "";
            for (int i = 0; i < xQuantity; i++)
            {
                newZ += "0 ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                newZ += "1 ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                newZ += "0 ";
            }
            return new DoubleVector(newZ);
        }
        private static DoubleVector CreateUsualXRow(int rows, int columns, int row)
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
                result += $"{num} ";
            }
            return new DoubleVector(result);
        }
        private static DoubleVector CreateSuperXRow(int rows, int columns, int row, int yQuantity)
        {
            string result = CreateUsualXRow(rows, columns, row).ToString();
            AddZerosToSuperString(ref result, yQuantity);
            return new DoubleVector(result);
        }
        private static void AddZerosToSuperString(ref string value, int yQuantity)
        {
            value = value.Substring(0, value.Length - 1);
            for (int i = 0; i < yQuantity * 2; i++)
            {
                value += $"0 ";
            }
        }
        private static DoubleVector CreateUsualXColumn(int rows, int columns, int column)
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
                result += $"{num} ";
            }
            return new DoubleVector(result);
        }
        private static DoubleVector CreateSuperXColumn(int rows, int columns, int column, int yQuantity)
        {
            string result = CreateUsualXColumn(rows, columns, column).ToString();
            AddZerosToSuperString(ref result, yQuantity);
            return new DoubleVector(result);
        }
        private static DoubleVector CreateValueRow(int value, int valueQuantity)
        {
            string result = "";
            for (int i = 0; i < valueQuantity; i++)
            {
                int num = 0;
                if (i == value)
                {
                    num = 1;
                }
                result += $"{num} ";
            }
            return new DoubleVector(result);
        }
        private static DoubleVector CreateYRow(int row, int yQuantity, DoubleVector c, double solution)
        {

            string strC = c.ToString().Replace(" ", " -");
            string result = strC.Substring(0, strC.Length - 2);
            for (int i = 0; i < yQuantity; i++)
            {
                int num = 0;
                if (i == row)
                {
                    num = 1;
                }
                result += $"{num} ";
            }
            for (int i = 0; i < yQuantity; i++)
            {
                double num = 0;
                if (i == row)
                {
                    num = solution;
                }
                result += $"{num} ";
            }
            result += "]";
            return new DoubleVector(result);
        }
        public static DoubleVector DivideX(DoubleVector values, int yQuantity)
        {
            string strX;
            strX = $"{RoundValue(values[0])} ";
            for (int i = 1; i < values.Length - yQuantity * 2; i++)
            {
                strX += $"{RoundValue(values[i])} ";
            }
            return new DoubleVector(strX);
        }
        public static DoubleVector DivideY(DoubleVector values, int yQuantity)
        {
            string strY;
            strY = $"{values[values.Length - yQuantity * 2]} ";
            for (int i = values.Length - yQuantity * 2 + 1; i < values.Length - yQuantity; i++)
            {
                strY += $"{values[i]} ";
            }
            return new DoubleVector(strY);
        }
        private static double CountSumProduct(DoubleVector first, DoubleVector second)
        {
            double result = 0;
            if (first.Length != second.Length)
            {
                throw new Exception("You try to multiply vectors with different sizes.");
            }
            for (int i = 0; i < first.Length; i++)
            {
                result += first[i] * second[i];
            }
            return result;
        }
        public static List<double> CalculateDeltas(DoubleVector[] cs, DoubleVector x, List<double> solutions)
        {
            List<double> deltas = new List<double>();
            for (int i = 0; i < solutions.Count; i++)
            {
                deltas.Add(CountSumProduct(cs[i], x) - solutions[i]);
            }
            return deltas;
        }
        public static DoubleVector RoundMatrix(DoubleVector X)
        {
            string result = "";
            for (int i = 0; i < X.Length; i++)
            {
                result += $"{Math.Round(X[i])} ";
            }
            return new DoubleVector(result);
        }
        public static bool CheckABConstraints(DoubleVector x, DoubleVector a, DoubleVector b)
        {
            bool success = true;
            for (int i = 0; i < a.Length; i++)
            {
                if (RoundValue(CountSumProduct(x, CreateUsualXRow(a.Length, b.Length, i))) != a[i])
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (RoundValue(CountSumProduct(x, CreateUsualXColumn(a.Length, b.Length, i))) != b[i])
                    {
                        success = false;
                        break;
                    }
                }
            }
            return success;
        }
    }
}