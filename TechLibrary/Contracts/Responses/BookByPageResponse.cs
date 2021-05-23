using System.Collections.Generic;

namespace TechLibrary.Models
{
    public class BookByPageResponse
    {
        public int TotalRecords { get; set; }
        public List<BookResponse> Books { get; set; }

    }
}
