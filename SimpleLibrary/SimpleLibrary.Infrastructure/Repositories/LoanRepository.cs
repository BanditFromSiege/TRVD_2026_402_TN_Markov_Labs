using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleLibrary.Infrastructure.Models;

namespace SimpleLibrary.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly SimpleLibraryContext _context;

        public LoanRepository(SimpleLibraryContext context)
        {
            _context = context;
        }

        public async Task<Loan?> GetByIdAsync(int id) =>
            await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<Loan>> GetAllAsync() =>
            await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToListAsync();

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Loan loan)
        {
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}