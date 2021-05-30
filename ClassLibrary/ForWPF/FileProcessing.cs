using ClassLibrary.Enums;
using ClassLibrary.ForWPF.SolutionBundles;
using ClassLibrary.MethodParameters;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibrary.ForWPF
{
    public class FileProcessing
    {
        private const char V = '\n';
        public static Problem ReadProblemFromFile(string filename)
        {
            try
            {
                using StreamReader stream = new StreamReader(filename);
                var N = Convert.ToInt32(stream.ReadLine());
                stream.ReadLine();
                var R = Convert.ToInt32(stream.ReadLine());
                stream.ReadLine();

                double[][] Cs = new double[R][];
                var size = N * N;
                for (var r = 0; r < R; r++)
                {
                    Cs[r] = new double[size];
                    string matrix = "";
                    for (var c = 0; c < N; c++)
                    {
                        matrix += stream.ReadLine();
                    }
                    var i = 0;
                    foreach (var col in matrix.Trim().Split(' '))
                    {
                        Cs[r][i] = double.Parse(col);
                        i++;
                    }
                    stream.ReadLine();
                }

                double[] a = new double[N];
                string text = stream.ReadLine();

                var j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");

                foreach (var col in text.Trim().Split())
                {
                    a[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] b = new double[N];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");
                foreach (var col in text.Trim().Split())
                {
                    b[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] l = new double[R];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");
                foreach (var col in text.Trim().Split())
                {
                    l[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] alpha = new double[R];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");
                foreach (var col in text.Trim().Split())
                {
                    alpha[j] = double.Parse(col);
                    j++;
                }
                return new Problem(a, b, l, alpha, Cs, CChangeParameters.Default);
            }
            catch (Exception) { }
            return null;
        }
        public static void WriteProblemIntoFile(Problem data, string filename)
        {
            var fullText = data.N.ToString() + "\n\n";
            fullText += data.R.ToString() + V;
            foreach (var c in data.Cs)
            {
                for (var i = 0; i < c.Length; i++)
                {
                    if (i % Math.Sqrt(c.Length) == 0)
                        fullText += V;
                    fullText += c[i].ToString() + ' ';
                }
                fullText += V;
            }
            fullText += V;
            foreach (var a in data.A)
                fullText += a.ToString() + ' ';
            fullText += "\n\n";
            foreach (var b in data.B)
                fullText += b.ToString() + ' ';
            fullText += "\n\n";
            foreach (var l in data.L)
                fullText += l.ToString() + ' ';
            fullText += "\n\n";
            foreach (var alpha in data.Alpha)
                fullText += alpha.ToString() + ' ';
            fullText += V;

            File.WriteAllText(filename, fullText);
        }
        public static void WriteSolutionIntoFile(FullSolution solution, string filename)
        {
            var text = "Optimal value: " + solution.SolutionWithoutChange.FunctionValue + V + V;
            text += "A: " + solution.A + V;
            text += "B: " + solution.B + V;
            text += "L: " + solution.L + V;
            text += "Alphas: " + solution.SolutionWithoutChange.Alpha + V;

            if (solution.SolutionWithoutChange.Alpha != solution.SolutionWithoutChange.AlphaChanged)
            {
                text += "Alphas: " + solution.SolutionWithoutChange.AlphaChanged + V;
            }
            text += "\nOptimal X:\n";
            text += solution.SolutionWithoutChange.OptimalX + V + V;

            for (var i = 0; i < solution.SolutionWithoutChange.Cs.Length; i++)
            {
                text += $"Matrix C[{i + 1}]:\n";
                text += solution.SolutionWithoutChange.Cs[i] + V;
                text += $"Solution for matrix C[{i + 1}]:\n";
                text += solution.SolutionWithoutChange.Xs[i] + V;
                text += "F = " + solution.SolutionWithoutChange.FsForX[i] + V;
                text += "F (with optimal Xs) = " + solution.SolutionWithoutChange.FsForXs[i] + V;
                text += "Delta = " + solution.SolutionWithoutChange.Deltas[i] + V;
                text += "Y = " + solution.SolutionWithoutChange.Ys[i] + V;
                text += "Distance = " + solution.SolutionWithoutChange.Distances[i] + V + V + V;
            }

            text += $"After changes ({solution.PersentOfChange}%) " +
                $"Optimal Value:\t{solution.SolutionWithChange.FunctionValue}" + V + V;
            if (solution.SolutionWithoutChange.AlphaChanged != solution.SolutionWithChange.AlphaChanged)
            {
                text += "Alpha changed: " + solution.SolutionWithChange.AlphaChanged + V + V;
            }
            text += "Optimal X changed:\n";
            text += solution.SolutionWithChange.OptimalX + V + V;

            for (var i = 0; i < solution.SolutionWithChange.Cs.Length; i++)
            {
                text += $"Matrix C[{i + 1}] after changes:\n";
                text += solution.SolutionWithChange.Cs[i] + V;
                text += $"Solution for matrix C[{i + 1}] after changes:\n";
                text += solution.SolutionWithChange.Xs[i] + V;
                text += "F = " + solution.SolutionWithChange.FsForX[i] + V;
                text += "F (with optimal Xs) = " + solution.SolutionWithChange.FsForXs[i] + V;
                text += "Delta = " + solution.SolutionWithChange.Deltas[i] + V;
                text += "Y = " + solution.SolutionWithChange.Ys[i] + V;
                text += "Distance = " + solution.SolutionWithChange.Distances[i] + V + V + V;
            }

            File.WriteAllText(filename, text);
        }
        public static void WriteExperimentDataIntoFile(List<(int, double)> list, string filename)
        {
            var text = "Size of matrix\tPercentage of change by changing optimum\n\n";
            list.ForEach(x => text += (x.Item1.ToString() + "\t\t" + x.Item2.ToString() + '\n'));
            File.WriteAllText(filename, text);
        }
        public static void WriteExperimentDataIntoFile(List<List<(int, double)>> list, List<string> messages, string filename, string sizeOfMatrix)
        {
            var text = $"{sizeOfMatrix}\t";
            for(var i = 0; i < messages.Count; i++)
            {
                text += $"{messages[i]}\t\t";
            }
            text += "\n\n";
            for(var i = 0; i < list[0].Count; i++)
            {
                text += list[0][i].Item1.ToString() + "\t\t";
                for(var j = 0; j < list.Count; j++)
                {
                    text += string.Format("{0:0.00000000}", list[j][i].Item2) + "\t";
                }
                text += "\n";
            }
            
            //list.ForEach(x => text += (x.Item1.ToString() + "\t\t" + x.Item2.ToString() + '\n'));
            File.WriteAllText(filename, text);
        }
        public static ParametersForDefined ReadSolutionForPersistenceTest(string filename)
        {
            try
            {
                using StreamReader stream = new StreamReader(filename);
                var N = Convert.ToInt32(stream.ReadLine());
                stream.ReadLine();
                var R = Convert.ToInt32(stream.ReadLine());
                stream.ReadLine();

                var size = N * N;
                double[] xs = new double[size];
                string matrix = "";
                for (var c = 0; c < N; c++)
                {
                    matrix += stream.ReadLine();
                }
                var i = 0;
                foreach (var col in matrix.Trim().Split(' '))
                {
                    xs[i] = double.Parse(col);
                    i++;
                }
                stream.ReadLine();

                double[][] Cs = new double[R][];
                for (var r = 0; r < R; r++)
                {
                    Cs[r] = new double[size];
                    matrix = "";
                    for (var c = 0; c < N; c++)
                    {
                        matrix += stream.ReadLine();
                    }
                    i = 0;
                    foreach (var col in matrix.Trim().Split(' '))
                    {
                        Cs[r][i] = double.Parse(col);
                        i++;
                    }
                    stream.ReadLine();
                }

                double[] a = new double[N];
                string text = stream.ReadLine();

                var j = 0;
                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");

                foreach (var col in text.Trim().Split())
                {
                    a[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] b = new double[N];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");

                foreach (var col in text.Trim().Split())
                {
                    b[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] l = new double[R];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");

                foreach (var col in text.Trim().Split())
                {
                    l[j] = double.Parse(col);
                    j++;
                }
                stream.ReadLine();

                double[] alpha = new double[R];
                text = stream.ReadLine();

                j = 0;

                if (text.Trim().Split() == null)
                    throw new Exception("Incorrect format of input file");

                foreach (var col in text.Trim().Split())
                {
                    alpha[j] = double.Parse(col);
                    j++;
                }
                return new ParametersForDefined(xs, Cs, a, b, l, alpha, CChangeParameters.Default);
            }
            catch(Exception ex) {  }
            return null;

        }
    }
}
