namespace ClassLibrary
{
    public class Problem
    {
        private readonly GeneratorTaskCondition _generator;
        private readonly CChangeParameters _cChangeParameters;
        public readonly int N;
        public readonly int R;
        public double[] A;
        public double[] B;
        public double[] L;
        public double[] Alpha;
        public double[][] Cs;
        public Problem(int N, int R, GeneratorTaskCondition generator, CChangeParameters cChangeParameters)
        {
            this.N = N;
            this.R = R;
            _generator = generator;
            _cChangeParameters = cChangeParameters;
        }
        public Problem(double[] a, double[] b, double[] l, double[] alpha, double[][] cs, CChangeParameters cChangeParameters)
        {
            N = a.Length;
            R = l.Length;
            A = a;
            B = b;
            L = l;
            Alpha = alpha;
            Cs = cs;
            _cChangeParameters = cChangeParameters;
        }
        public FullSolution Run()
        {
            Solver solver = new Solver();
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

            (double[][] xs, double[] fsForXs) = solver.GetSolutions(Cs, A, B);
            (double[] solution, _) = solver.SolveSeveral(Cs, A, B, L, Alpha, fsForXs);
            double[] newAlpha = new double[R];
            Alpha.CopyTo(newAlpha, 0);
            double[] optimalX = solver.UpdateX(Cs, A, B, L, ref newAlpha, fsForXs, solution);
            double[] fsForX = solver.CalculateFs(Cs, optimalX);
            double[] deltas = solver.CalculateDeltas(fsForX, fsForXs);
            double[] ys = solver.CalculateYs(deltas, L);
            double functionValue = solver.CalculateOptimanFunc(ys, newAlpha);
            double[] distances = solver.CalculateDistances(Cs, optimalX);

            Solution solutionWithoutChange = new Solution(optimalX, functionValue, Alpha, newAlpha, Cs, fsForX, xs, fsForXs, deltas, ys, distances, B.Length);

            double[][] newCs = new double[R][];
            PercentFinder.CopyMultidimensional(Cs, ref newCs);

            ParametersForDefined parameters = new ParametersForDefined(optimalX, newCs, A, B, L, Alpha, _cChangeParameters);

            double persentOfChange = PercentFinder.FindPercentOfChange(parameters, solver);
            double[] newX = parameters.NewX;
            double[] newNewAlpha = new double[R];
            newAlpha.CopyTo(newNewAlpha, 0);
            (double[][] newXs, double[] newFsForXs) = solver.GetSolutions(newCs, A, B);
            newX = solver.UpdateX(newCs, A, B, L, ref newNewAlpha, newFsForXs, newX);
            double[] newfsForX = solver.CalculateFs(newCs, newX);
            double[] newDeltas = solver.CalculateDeltas(newfsForX, newFsForXs);
            double[] newYs = solver.CalculateYs(newDeltas, L);
            double newFunctionValue = solver.CalculateOptimanFunc(newYs, newNewAlpha);
            double[] newDistances = solver.CalculateDistances(newCs, newX);
            Solution solutionWithChange = new Solution(newX, newFunctionValue, newAlpha, newNewAlpha, newCs, newfsForX, newXs, newFsForXs, newDeltas, newYs, newDistances, B.Length);

            return new FullSolution(A, B, L, solutionWithoutChange, persentOfChange, solutionWithChange);
        }
    }
}
