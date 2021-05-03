using System;
using System.Linq;

namespace ClassLibrary
{
    public static class Solver
    {
        public static (double[], double) SolveOne(double[] c, double[] a, double[] b)
        {
            int rows = a.Length;
            int columns = b.Length;
            int xNumber = c.Length;
            int constrNumber = rows + columns;
            double[] scale = new double[xNumber];
            double[] varBoundL = new double[xNumber];
            double[] varBoundU = new double[xNumber];
            double[] constrBound = new double[constrNumber];
            double[,] constrVars = new double[constrNumber, xNumber];

            FillScale(ref scale);
            FillVarBoundUForOne(ref varBoundU);
            FillConstrBoundsForOne(ref constrBound, a, b);
            FillConstrVarsForOne(ref constrVars, rows, columns);

            alglib.minlpcreate(xNumber, out alglib.minlpstate state);
            alglib.minlpsetalgodss(state, 0.00001);
            alglib.minlpsetcost(state, c);
            alglib.minlpsetbc(state, varBoundL, varBoundU);
            alglib.minlpsetlc2dense(state, constrVars, constrBound, constrBound, constrNumber);
            alglib.minlpsetscale(state, scale);

            alglib.minlpoptimize(state);
            alglib.minlpresults(state, out double[] x, out alglib.minlpreport rep);
            return (x, rep.f);
        }
        public static (double[], double) SolveSeveral(double[][] cs, double[] a, double[] b, double[] l, double[] alpha, double[] solutions)
        {
            int rows = a.Length;
            int columns = b.Length;
            int xNumber = rows * columns;
            int yNumber = l.Length;
            int constrNumber = rows + columns + yNumber;
            int varNumber = xNumber + yNumber * 2;
            double[] scale = new double[varNumber];
            double[] z = new double[varNumber];
            double[] constrBoundL = new double[constrNumber];
            double[] constrBoundR = new double[constrNumber];
            double[] varBoundL = new double[varNumber];
            double[] varBoundU = new double[varNumber];
            double[,] constrVars = new double[constrNumber, varNumber];
            double[] variables;
            alglib.minlpstate state;
            alglib.minlpreport rep;

            FillScale(ref scale);
            FillZForSeveral(ref z, alpha);
            FillConstrBoundsForOne(ref constrBoundL, a, b);
            constrBoundL.CopyTo(constrBoundR, 0);
            FillConstrBoundLForSeveral(ref constrBoundL, yNumber);
            FillConstrBoundUForSeveral(ref constrBoundR, l);
            FillConstrVarsForOne(ref constrVars, rows, columns);
            FillConstrVarsForSeveral(ref constrVars, cs, solutions);
            FillVarBoundUForOne(ref varBoundU);
            FillVarBoundsForSeveral(ref varBoundL, ref varBoundU, yNumber);

            alglib.minlpcreate(varNumber, out state);
            alglib.minlpsetcost(state, z);
            alglib.minlpsetbc(state, varBoundL, varBoundU);
            alglib.minlpsetlc2dense(state, constrVars, constrBoundL, constrBoundR, constrNumber);
            alglib.minlpsetscale(state, scale);
            alglib.minlpsetalgodss(state, 0.00001);

            alglib.minlpoptimize(state);
            alglib.minlpresults(state, out variables, out rep);
            return (variables, rep.f);
        }
        private static void FillScale(ref double[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = 1;
            }
        }
        private static void FillVarBoundUForOne(ref double[] boundXU)
        {
            for (int i = 0; i < boundXU.Length; i++)
            {
                boundXU[i] = double.PositiveInfinity;
            }
        }
        private static void FillConstrBoundsForOne(ref double[] constrBound, double[] a, double[] b)
        {
            int aConstrNumber = a.Length;
            int constrNumber = aConstrNumber + b.Length;
            for (int i = 0; i < aConstrNumber; i++)
            {
                constrBound[i] = a[i];
            }
            for (int i = aConstrNumber; i < constrNumber; i++)
            {
                constrBound[i] = b[i - aConstrNumber];
            }
        }
        private static void FillConstrVarsForOne(ref double[,] constrVars, int rows, int columns)
        {
            int xNumber = rows * columns;
            int constrNumber = rows + columns;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < xNumber; j++)
                {
                    if (j >= columns * i && j < columns * (i + 1))
                    {
                        constrVars[i, j] = 1;
                    }
                }

            }
            for (int i = rows; i < constrNumber; i++)
            {
                for (int j = 0; j < xNumber; j++)
                {
                    if (j % columns == i - rows)
                    {
                        constrVars[i, j] = 1;
                    }
                }
            }
        }
        private static void FillConstrVarsForSeveral(ref double[,] constrVars, double[][] cs, double[] solutions)
        {
            int xNumber = cs[0].Length;
            int yNumber = solutions.Length;
            int constrNumber = constrVars.GetLength(0);
            for (int i = 0; i < yNumber; i++)
            {
                for(int j = 0; j < xNumber; j++)
                {
                    constrVars[constrNumber - yNumber + i, j] = cs[i][j];
                }
                constrVars[constrNumber - yNumber + i, xNumber + i] = -solutions[i];
                constrVars[constrNumber - yNumber + i, xNumber + yNumber + i] = -1;
            }
        }
        private static void FillConstrBoundLForSeveral(ref double[] constrBound, int yNumber)
        {
            int constNumber = constrBound.Length;
            for (int i = yNumber; i > 0; i--)
            {
                constrBound[constNumber - i] = -double.PositiveInfinity;
            }
        }
        private static void FillConstrBoundUForSeveral(ref double[] constrBound, double[] l)
        {
            int constNumber = constrBound.Length;
            int yNumber = l.Length;
            for (int i = 0; i < yNumber; i++)
            {
                constrBound[constNumber - yNumber + i] = l[i];
            }
        }
        private static void FillZForSeveral(ref double[] z, double[] alpha)
        {
            FillConstrBoundUForSeveral(ref z, alpha);
        }
        private static void FillVarBoundsForSeveral(ref double[] varBoundL, ref double[] varBoundU, int yNumber)
        {
            int varNumber = varBoundL.Length;
            for(int i = 0; i < yNumber; i++)
            {
                varBoundL[varNumber - yNumber - yNumber + i] = 1;
                varBoundU[varNumber - yNumber - yNumber + i] = 1;
            }
        }
        public static (double[][], double[]) GetSolutions(double[][] c, double[] a, double[] b)
        {
            int solutionNumber = c.Length;
            double[][] varValues = new double[solutionNumber][];
            double[] funcValues = new double[solutionNumber];
            for(int i = 0; i < solutionNumber; i++)
            {
                (varValues[i], funcValues[i]) = SolveOne(c[i], a, b);
            }
            return (varValues, funcValues);
        }
        public static double RoundValue(double value)
        {
            double eps = 0.0000000001;
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
        public static double[] DivideX(double[] values, int yQuantity)
        {
            int xNumber = values.Length - yQuantity * 2;
            double[] xValues = new double[xNumber];
            for (int i = 0; i < xNumber; i++)
            {
                xValues[i] = RoundValue(values[i]);
            }
            return xValues;
        }
        public static double SumProduct(double[] first, double[] second)
        {
            int valueNumber = first.Length;
            double result = 0;
            if (valueNumber != second.Length)
            {
                throw new Exception("You try to multiply vectors with different sizes.");
            }
            for (int i = 0; i < valueNumber; i++)
            {
                result += first[i] * second[i];
            }
            return result;
        }
        public static double CalculateDistance(double[] first, double[] second)
        {
            int valueNumber = first.Length;
            double result = 0;
            if (valueNumber != second.Length)
            {
                throw new Exception("Points are in different dimension spaces.");
            }
            for (int i = 0; i < valueNumber; i++)
            {
                result += Math.Pow(first[i] - second[i], 2);
            }
            return Math.Sqrt(result);
        }
        public static double[] CalculateDistances(double[][] cs, double[] x)
        {
            int distNumber = cs.Length;
            double[] distances = new double[distNumber];
            for (int i = 0; i < distNumber; i++)
            {
                distances[i] = CalculateDistance(cs[i], x);
            }
            return distances;
        }
        public static double[] CalculateDeltas(double[] fsForX, double[] fsForXs)
        {
            int deltNumber = fsForXs.Length;
            double[] deltas = new double[deltNumber];
            for (int i = 0; i < deltNumber; i++)
            {
                deltas[i] = fsForX[i] - fsForXs[i];
            }
            return deltas;
        }
        public static double[] CalculateYs(double[] deltas, double[] l)
        {
            int deltNumber = deltas.Length;
            double[] ys = new double[deltNumber];
            for (int i = 0; i < deltNumber; i++)
            {
                ys[i] = Math.Max(deltas[i] - l[i], 0);
            }
            return ys;
        }
        public static double[] CalculateFs(double[][] cs, double[] x)
        {
            int fsNumber = cs.Length;
            double[] fs = new double[fsNumber];
            for (int i = 0; i < fsNumber; i++)
            {
                fs[i] = SumProduct(cs[i], x);
            }
            return fs;
        }
        public static double CalculateOptimanFunc(double[] ys, double[] alpha)
        {
            return SumProduct(ys, alpha);
        }
        public static double[] RoundVector(double[] x)
        {
            int valueNumber = x.Length;
            double[] result = new double[valueNumber];
            for (int i = 0; i < valueNumber; i++)
            {
                result[i] = Math.Round(x[i]);
            }
            return result;
        }
        public static bool CheckABConstraints(double[] x, double[] a, double[] b)
        {
            bool success = true;
            int rows = a.Length;
            int columns = b.Length;
            int xNumber = x.Length;
            int constrNumber = rows + columns;
            double[] constrBound = new double[constrNumber];
            double[,] constrVars = new double[constrNumber, xNumber];

            FillConstrBoundsForOne(ref constrBound, a, b);
            FillConstrVarsForOne(ref constrVars, rows, columns);
            TransposeMatrix(ref constrVars);

            for (int i = 0; i < constrNumber; i++)
            {
                double sum = 0;
                for(int j = 0; j < xNumber; j++)
                {
                    sum += x[j] * constrVars[j, i];
                }
                if (RoundValue(sum) != constrBound[i])
                {
                    success = false;
                    break;
                }
            }
            return success;
        }
        private static void TransposeMatrix(ref double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            double[,] newMatrix = new double[columns, rows];
            for(int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    newMatrix[i, j] = matrix[j, i];
                }
            }
            matrix = newMatrix;
        }
        private static double[] IterativeProcedure(double[][] cs, double[] a, double[] b, double[] l, ref double[] alpha, double[] solutions, double[] oldX)
        {
            double[] newX = new double[oldX.Length];
            double[] newAlpha = new double[alpha.Length];
            oldX.CopyTo(newX, 0);
            alpha.CopyTo(newAlpha, 0);
            while (true)
            {
                double step = 0.1;
                double[] deltas = CalculateDeltas(CalculateFs(cs, newX), solutions);
                double[] ys = CalculateYs(deltas, l);
                if (!ys.Any(x => RoundValue(x) == 0) || ys.All(x => RoundValue(x) == 0))
                {
                    break;
                }
                int ysNumber = ys.Length;
                for (int i = 0; i < ysNumber; i++)
                {
                    if (ys[i] <= 0)
                    {
                        newAlpha[i] -= step;
                    }
                    else
                    {
                        newAlpha[i] += step;
                    }
                }
                (double[] solution, _) = SolveSeveral(cs, a, b, l, newAlpha, solutions);
                newX = DivideX(RoundVector(solution), cs.Length);
                if (CheckABConstraints(newX, a, b))
                {
                    alpha = newAlpha;
                    break;
                }

                if (newAlpha.Any(x => x <= 0))
                {
                    newX = oldX;
                    break;
                }
            }
            return newX;
        }
        public static double[] UpdateX(double[][] cs, double[] a, double[] b, double[] l, ref double[] alpha, double[] solutions, double[] oldX)
        {
            oldX = DivideX(oldX, cs.Length);
            double[] newX = new double[oldX.Length];
            RoundVector(oldX).CopyTo(newX, 0);
            if (CheckABConstraints(newX, a, b))
            {
                return newX;
            }
            else
            {
                newX = IterativeProcedure(cs, a, b, l, ref alpha, solutions, oldX);
                if (newX.Length == 0)
                {
                    newX = oldX;
                }
            }
            return newX;
        }
    }
}
