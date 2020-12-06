using System;
using System.Collections.Generic;
using CenterSpace.NMath.Analysis;
using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public static class Solver
    {
        public static DualSimplexSolver SolveSeveral(DoubleVector[] Cs, DoubleVector A, DoubleVector B, DoubleVector L, List<double> solutions)
        {
            int rows, columns, xQuantity, yQuantity, fullQuantity;
            rows = A.Length;
            columns = B.Length;
            xQuantity = rows * columns;
            yQuantity = Cs.Length;
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
                var constraintA = new LinearConstraint(coeffA, A[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = CreateSuperXColumn(rows, columns, j, yQuantity);
                var constraintB = new LinearConstraint(coeffB, B[j], ConstraintType.EqualTo);
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
                var constraint = new LinearConstraint(CreateYRow(i, yQuantity, Cs[i], solutions[i]), -L[i], ConstraintType.GreaterThanOrEqualTo);
                problem.AddConstraint(constraint);
            }
            solver.Solve(problem, solverParams);
            return solver;
        }
        public static DualSimplexSolver SolveOne(DoubleVector C, DoubleVector A, DoubleVector B)
        {
            int rows, columns, xQuantity;
            rows = A.Length;
            columns = B.Length;
            xQuantity = rows * columns;

            DualSimplexSolver solver = new DualSimplexSolver();
            var solverParams = new DualSimplexSolverParams
            {
                Minimize = true
            };

            var problem = new MixedIntegerLinearProgrammingProblem(C);
            for (int i = 0; i < rows; i++)
            {
                var coeffA = CreateUsualXRow(rows, columns, i);
                var constraintA = new LinearConstraint(coeffA, A[i], ConstraintType.EqualTo);
                problem.AddConstraint(constraintA);
            }
            for (int j = 0; j < columns; j++)
            {
                var coeffB = CreateUsualXColumn(rows, columns, j);
                var constraintB = new LinearConstraint(coeffB, B[j], ConstraintType.EqualTo);
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
        private static DoubleVector CreateYRow(int row, int yQuantity, DoubleVector C, double solution)
        {

            string strC = C.ToString().Replace(" ", " -");
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
        public static List<double> CalculateDeltas(DoubleVector[] Cs, DoubleVector X, List<double> solutions)
        {
            List<double> deltas = new List<double>();
            for (int i = 0; i < solutions.Count; i++)
            {
                deltas.Add(CountSumProduct(Cs[i], X) - solutions[i]);
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
        public static bool CheckABConstraints(DoubleVector X, DoubleVector A, DoubleVector B)
        {
            bool success = true;
            for (int i = 0; i < A.Length; i++)
            {
                if (RoundValue(CountSumProduct(X, CreateUsualXRow(A.Length, B.Length, i))) != A[i])
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                for (int i = 0; i < B.Length; i++)
                {
                    if (RoundValue(CountSumProduct(X, CreateUsualXColumn(A.Length, B.Length, i))) != B[i])
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