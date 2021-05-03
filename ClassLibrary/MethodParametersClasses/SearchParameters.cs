namespace ClassLibrary
{
    public abstract class SearchParameters
    {
        public CChangeParameters CChangeParameters { get; }
        public SearchParameters(CChangeParameters parameters)
        {
            CChangeParameters = parameters;
        }
    }
}
