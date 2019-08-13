using System.Collections.Generic;
using TechTest.Core.Interfaces;
using TechTest.Core.Models;

namespace TechTest.Data.Repositories
{
    public class SpyRepository : IRepository<Spy>
    {
        public IEnumerable<Spy> Get()
        {
            return new List<Spy>
            {
                new Spy { Id = 1, Name = "James Bond", CodeName = "007"},
                new Spy { Id = 2, Name = "Ethan Hunt", CodeName = "314"}
            };
        }
    }
}
