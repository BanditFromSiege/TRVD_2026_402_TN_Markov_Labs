using Microsoft.EntityFrameworkCore;
using SimpleLibrary.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrary.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly SimpleLibraryContext _context;

        public BookRepository(SimpleLibraryContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetByIdAsync(int id) =>
            await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

        public async Task<IEnumerable<Book>> GetAllAsync() =>
            await _context.Books.ToListAsync();

        public async Task<Book?> FindByTitleAndAuthorAsync(string title, string author)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title == title && b.Author == author);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}