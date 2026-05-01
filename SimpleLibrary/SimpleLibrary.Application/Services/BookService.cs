using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.Infrastructure.Repositories;

namespace SimpleLibrary.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var existing = await _bookRepository.FindByTitleAndAuthorAsync(book.Title, book.Author);
            if (existing != null)
                throw new Exception("Book with this title and author already exists");

            await _bookRepository.AddAsync(book);
            return book;
        }

        public async Task<Book?> UpdateBookAsync(int id, Book book)
        {
            var existing = await _bookRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            var duplicate = await _bookRepository.FindByTitleAndAuthorAsync(book.Title, book.Author);
            if (duplicate != null && duplicate.Id != id)
                throw new Exception("Book with this title and author already exists");

            int borrowedCopies = existing.TotalCopies - existing.AvailableCopies;

            if (book.TotalCopies < borrowedCopies)
                throw new Exception("TotalCopies cannot be less than borrowed copies");

            int difference = book.TotalCopies - existing.TotalCopies;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Genre = book.Genre;
            existing.Isbn = book.Isbn;
            existing.TotalCopies = book.TotalCopies;
            existing.AvailableCopies += difference;

            await _bookRepository.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return false;

            await _bookRepository.DeleteAsync(book);
            return true;
        }
    }
}
