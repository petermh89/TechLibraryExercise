namespace TechLibrary.Contracts.Requests
{
    public class BookByPageRequest
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public string Filter { get; set; }
    }
}
