namespace MoogleEngine
{
    public class SearchTerm
    {
        public string Operador { get; set; }
        public string Term { get; set; }

        public SearchTerm DependentTerm { get; set; }
    }
}
