using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Contracts.Requests;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);

        Task<(int TotalRecords, IEnumerable<Book> Books)> GetBooksByPageAsync(int currentPage, int pageSize, string filter);

        Task UpdateBookAsync(Book book);

        Task AddBookAsync(Book book);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        //This task returns a tuple of records and book list taking in account any filters passed in by user
        public async Task<(int TotalRecords, IEnumerable<Book> Books)> GetBooksByPageAsync(int currentPage, int pageSize, string filter)
        {
            IQueryable<Book> books = _dataContext.Books;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var search = filter.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(search) || b.ShortDescr.ToLower().Contains(search));
            }

            return (TotalRecords: await books.CountAsync(), Books: await books.Skip((currentPage - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync());
        }

        //this task will update the short description of a book by matching its ID and then just updating the attribute
        public async Task UpdateBookAsync(Book book)
        {
            var bookRecord = _dataContext.Books.SingleOrDefault(x => x.BookId == book.BookId);

            bookRecord.ShortDescr = book.ShortDescr;

            await _dataContext.SaveChangesAsync();
        }

        //Simple ADD task, not production ready: validation is too poor for inserts to prod db
        public async Task AddBookAsync(Book book)
        {
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
        }
    }
}
