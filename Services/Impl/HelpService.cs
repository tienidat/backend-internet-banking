using BPIBankSystem.API.Data;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class HelpService : IHelpService
    {
        private readonly AppDbContext _context;

        public HelpService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Help>> GetAllHelpsAsync()
        {
            return await _context.Helps
                .Include(h => h.Category)
                .ToListAsync();
        }

        public async Task<List<CategoryHelp>> GetAllCategoriesAsync()
        {
            return await _context.CategoryHelps.ToListAsync();
        }
    }
}
