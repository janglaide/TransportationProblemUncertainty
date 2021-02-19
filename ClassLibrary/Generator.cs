using System;

namespace ClassLibrary
{
    public class Generator
    {
        private static readonly Random rand = new Random();
        public int GetIntValue(string distr, (double, double) parameters)
        {
            return (int)Math.Round(GetDoubleValue(distr, parameters));
        }
        public double GetDoubleValue(string distr, (double, double) parameters)
        {
            return distr switch
            {
                "exp" => Exponential(parameters.Item1),
                "norm" => Normal(parameters.Item1, parameters.Item2),
                "unif" => Uniform(parameters.Item1, parameters.Item2),
                _ => double.MinValue,
            };
        }
        public double Exponential(double lambda)
        {
            return -Math.Log(rand.NextDouble()) / lambda;
        }
        public double Normal(double mean, double deviation)
        {
            double mu = 0;
            for (int j = 0; j < 12; j++)
            {
                mu += rand.NextDouble();
            }
            mu -= 6;
            return deviation * mu + mean;
        }
        public double Uniform(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }
    }
}
