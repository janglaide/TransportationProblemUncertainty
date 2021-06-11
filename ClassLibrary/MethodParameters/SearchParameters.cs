using ClassLibrary.Enums;

namespace ClassLibrary.MethodParameters
{
    public abstract class SearchParameters
    {
        public CChangeParameters CChangeParameters { get; }
        public SearchParameters(CChangeParameters parameters)
        {
            CChangeParameters = parameters;
        }
        public abstract void Clear();
    }
}
