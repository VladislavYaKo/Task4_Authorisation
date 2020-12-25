using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Interfaces;
using Task4_Authorisation.Data.Models;

namespace Task4_Authorisation.Data.Repository
{
    public class UsersRepository : IUsers
    {
        private readonly AppDBContext appDbContext;

        public UsersRepository(AppDBContext appDbContent)
        {
            this.appDbContext = appDbContent;
        }
        public IEnumerable<User> AllUsers => appDbContext.users;

        public async Task AddUser(User user)
        {
            appDbContext.users.Add(user);
            await appDbContext.SaveChangesAsync();
        }
        public async Task DeleteUsers(User[] users)
        {
            appDbContext.users.RemoveRange(users);
            await appDbContext.SaveChangesAsync();
        }
    }
}
