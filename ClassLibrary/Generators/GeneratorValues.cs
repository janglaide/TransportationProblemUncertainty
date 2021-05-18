using System;

namespace ClassLibrary.Generators
{
    public static class GeneratorValues
    {
        public static int GetIntValue(string distr, (double, double) parameters, Random random)
        {
            return (int)Math.Round(GetDoubleValue(distr, parameters, random));
        }
        public static double GetDoubleValue(string distr, (double, double) parameters, Random random)
        {
            return distr switch
            {
                "Exponential" => Exponential(parameters.Item1, random),
                "Normal" => Normal(parameters.Item1, parameters.Item2, random),
                "Uniform" => Uniform(parameters.Item1, parameters.Item2, random),
                _ => double.MinValue,
            };
        }
        private static double Exponential(double lambda, Random random)
        {
            return -Math.Log(random.NextDouble()) / lambda;
        }
        private static double Normal(double mean, double deviation, Random random)
        {
            double mu = 0;
            for (int j = 0; j < 12; j++)
            {
                mu += random.NextDouble();
            }
            mu -= 6;
            return deviation * mu + mean;
        }
        private static double Uniform(double min, double max, Random random)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
