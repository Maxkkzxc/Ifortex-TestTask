using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthor()
        {
            var result = await _context.Books
                .OrderByDescending(b => b.Title.Length)
                .ThenBy(b => b.AuthorId)
                .Select(b => new { b.AuthorId })
                .FirstOrDefaultAsync();

            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == result.AuthorId);
        }

        public async Task<List<Author>> GetAuthors()
        {
            var authorsWithEvenBooksAfter2015 = await _context.Authors
               .Where(author => author.Books
               .Count(book => book.PublishDate.Year > 2015) % 2 == 0)
               .Where(author => author.Books.Any(book => book.PublishDate.Year > 2015))
               .ToListAsync();

            return authorsWithEvenBooksAfter2015;
        }
    }
}
