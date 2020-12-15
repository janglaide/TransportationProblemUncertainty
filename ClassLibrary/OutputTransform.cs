using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public static class OutputTransform
    {
        public static string MatrixTransform(DoubleVector matrix, int columns)
        {
            var result = "";
            int valueQuantity = matrix.Length;
            for (int i = 0; i < valueQuantity; i++)
            {
                double number = Solver.RoundValue(matrix[i]);

                if (number != 0)
                {
                    //result += $"{number}   ";
                    result += string.Format("{0:0.00}", number) + "\t";
                }
                else
                {
                    result += $"-\t";
                }

                if ((i + 1) % columns == 0)
                {
                    result += "\n";
                }

            }
            return result;
        }
        public static string VectorTransform(DoubleVector vector, string separ) // separ = "\t" || "\n"
        {
            var result = "";
            int valueQuantity = vector.Length;
            for (int i = 0; i < valueQuantity; i++)
            {
                double number = Solver.RoundValue(vector[i]);
                result += string.Format("{0:0.00}", number) + separ;
            }
            return result;
        }
        public static string[] ArrayMatrixTransform(DoubleVector[] vector, int columns)
        {
            int vectorQuantity = vector.Length;
            string[] result = new string[vectorQuantity];
            for (int i = 0; i < vectorQuantity; i++)
            {
                result[i] = MatrixTransform(vector[i], columns);
            }
            return result;
        }
        public static string[] ArrayValuesTransform(DoubleVector vector)
        {
            int vectorQuantity = vector.Length;
            string[] result = new string[vectorQuantity];
            for (int i = 0; i < vectorQuantity; i++)
            {
                result[i] = ValueTransform(vector[i]);
            }
            return result;
        }
        public static string ValueTransform(double value)
        {
            return Solver.RoundValue(value).ToString();
        }
    }
}
