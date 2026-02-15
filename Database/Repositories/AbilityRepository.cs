using Database.Context;
using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class AbilityRepository : Repository<Ability>, IAbilityRepository
    {
        public AbilityRepository(AppDbContext context) : base(context)
        {
        }

    
    }
}
