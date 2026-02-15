using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IHighscoreRepository : IRepository<Highscore>
    {
        Task<IEnumerable<Highscore>> GetAllHighscoresAsync();
    }
}
