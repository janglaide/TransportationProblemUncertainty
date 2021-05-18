using ClassLibrary.Enums;

namespace ClassLibrary.MethodParameters
{
    public class ParametersForRandom : SearchParameters
    {
        public int Size { get; }
        public int MatrixQuantity { get; }
        public ParametersForRandom(int size, int matrixQuantity, CChangeParameters parameters) : base(parameters)
        {
            Size = size;
            MatrixQuantity = matrixQuantity;
        }
    }
}
