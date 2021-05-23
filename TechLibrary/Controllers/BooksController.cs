using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using TechLibrary.Contracts.Requests;
using System;
using System.Net;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        //This task calls the get books by page and forms the response for the js to use
        [HttpPost()]
        public async Task<ActionResult<BookResponse>> GetBooks(BookByPageRequest data)
        {
            _logger.LogInformation($"Get {data.PerPage} books");

            var books = await _bookService.GetBooksByPageAsync(data.CurrentPage, data.PerPage, data.Filter);

            var response = new BookByPageResponse
            {
                TotalRecords = books.TotalRecords,
                Books = _mapper.Map<List<BookResponse>>(_mapper.Map<List<BookResponse>>(books.Books))
            };

            return Ok(response);
        }

        //This task calls update book and processes the update
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            _logger.LogInformation($"Update BookId: {book.BookId}");

            await _bookService.UpdateBookAsync(book);

            return Ok();


        }

        //Simple task that calls add book functionality
        [HttpPost("{Add}")]
        public async Task<IActionResult> AddBook(Book book)
        {
            _logger.LogInformation("Add New Book");

            await _bookService.AddBookAsync(book);

            return Ok();
        }

    }
}
