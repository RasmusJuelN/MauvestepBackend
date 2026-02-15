using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class HighscoreRepository : Repository<Highscore>, IHighscoreRepository
    {
        public HighscoreRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Highscore>> GetAllHighscoresAsync()
        {
            return await _dbSet
                .Include(h => h.User)
                .OrderByDescending(h => h.Score)
                .ToListAsync();
        }
    }
}
