using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.Infrastructure.Repositories;

namespace SimpleLibrary.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            var loans = await _loanRepository.GetAllAsync();
            return loans;
        }

        public async Task<Loan?> GetLoanByIdAsync(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
                return null;

            return loan;
        }

        public async Task<Loan> CreateLoanAsync(Loan loan)
        {
            var book = await _bookRepository.GetByIdAsync(loan.BookId);
            if (book == null || book.AvailableCopies <= 0)
                throw new Exception("Book is not available");

            book.AvailableCopies--;

            loan.IssuedAt = DateTime.UtcNow;

            await _loanRepository.AddAsync(loan);
            await _bookRepository.UpdateAsync(book);

            return loan;
        }

        public async Task<Loan?> UpdateLoanAsync(int id, Loan loan)
        {
            var existing = await _loanRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            bool isReturningNow = existing.ReturnedAt == null && loan.ReturnedAt != null;

            if (loan.DueDate != default)
            {
                existing.DueDate = loan.DueDate;
            }

            if (loan.ReturnedAt != null)
            {
                existing.ReturnedAt = loan.ReturnedAt;
            }

            if (isReturningNow)
            {
                var book = await _bookRepository.GetByIdAsync(existing.BookId);
                if (book != null)
                {
                    book.AvailableCopies++;
                    await _bookRepository.UpdateAsync(book);
                }
            }

            await _loanRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
                return false;

            await _loanRepository.DeleteAsync(loan);
            return true;
        }

        public async Task<bool> CheckBookAvailabilityAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            return book != null && book.AvailableCopies > 0;
        }
    }
}