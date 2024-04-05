using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class TestService
    {
        private readonly AppDBContext _context;
        public TestService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Test>> GetAll()
        {
            return await _context.Tests.ToListAsync();
        }
    }
}
