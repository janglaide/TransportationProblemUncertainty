namespace ClassLibrary
{
    public class Problem
    {
        private readonly GeneratorTaskCondition _generator;
        private readonly CChangeParameters _cChangeParameters;
        private readonly double _averChange;
        public readonly int N;
        public readonly int R;
        public double[] A;
        public double[] B;
        public double[] L;
        public double[] Alpha;
        public double[][] Cs;
        public Problem(int N, int R, GeneratorTaskCondition generator, CChangeParameters cChangeParameters, double averChange)
        {
            this.N = N;
            this.R = R;
            _averChange = averChange;
            _generator = generator;
            _cChangeParameters = cChangeParameters;
        }
        public Problem(double[] a, double[] b, double[] l, double[] alpha, double[][] cs, CChangeParameters cChangeParameters, double averChange)
        {
            N = a.Length;
            R = l.Length;
            A = a;
            B = b;
            L = l;
            Alpha = alpha;
            Cs = cs;
            _averChange = averChange;
            _cChangeParameters = cChangeParameters;
        }
        public FullSolution Run()
        {
            if (A is null)
            {
                if (_generator is null)
                {
                    throw new System.InvalidOperationException("Generating values parameters in GeneratorTaskCondition must be assigned to Problem");
                }
                (A, B) = _generator.GenerateAB(N);
                L = _generator.GenerateL(R);
                Alpha = _generator.GenerateAlpha(R);
                Cs = new double[R][];
                for (var i = 0; i < R; i++)
                    Cs[i] = _generator.GenerateMatrix(N);
            }

            (double[][] xs, double[] fsForXs) = Solver.GetSolutions(Cs, A, B);
            (double[] solution, _) = Solver.SolveSeveral(Cs, A, B, L, Alpha, fsForXs);
            double[] newAlpha = new double[R];
            Alpha.CopyTo(newAlpha, 0);
            double[] optimalX = Solver.UpdateX(Cs, A, B, L, ref newAlpha, fsForXs, solution);
            double[] fsForX = Solver.CalculateFs(Cs, optimalX);
            double[] deltas = Solver.CalculateDeltas(fsForX, fsForXs);
            double[] ys = Solver.CalculateYs(deltas, L);
            double functionValue = Solver.CalculateOptimanFunc(ys, newAlpha);
            double[] distances = Solver.CalculateDistances(Cs, optimalX);

            Solution solutionWithoutChange = new Solution(optimalX, functionValue, Alpha, newAlpha, Cs, fsForX, xs, fsForXs, deltas, ys, distances, B.Length);

            double[][] newCs = new double[R][];
            PercentFinder.CopyMultidimensional(Cs, ref newCs);

            ParametersForDefined parameters = new ParametersForDefined(optimalX, newCs, A, B, L, Alpha, _cChangeParameters);

            double persentOfChange = PercentFinder.FindPercentOfChange(parameters);
            double[] newX = parameters.NewX;
            double[] newNewAlpha = new double[R];
            newAlpha.CopyTo(newNewAlpha, 0);
            (double[][] newXs, double[] newFsForXs) = Solver.GetSolutions(newCs, A, B);
            newX = Solver.UpdateX(newCs, A, B, L, ref newNewAlpha, newFsForXs, newX);
            double[] newfsForX = Solver.CalculateFs(newCs, newX);
            double[] newDeltas = Solver.CalculateDeltas(newfsForX, newFsForXs);
            double[] newYs = Solver.CalculateYs(newDeltas, L);
            double newFunctionValue = Solver.CalculateOptimanFunc(newYs, newNewAlpha);
            double[] newDistances = Solver.CalculateDistances(newCs, newX);
            Solution solutionWithChange = new Solution(newX, newFunctionValue, newAlpha, newNewAlpha, newCs, newfsForX, newXs, newFsForXs, newDeltas, newYs, newDistances, B.Length);

            return new FullSolution(A, B, L, solutionWithoutChange, persentOfChange, solutionWithChange);
        }
    }
}
