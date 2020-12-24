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
        private readonly AppDBContent appDbContent;

        public UsersRepository(AppDBContent appDbContent)
        {
            this.appDbContent = appDbContent;
        }
        public IEnumerable<User> AllUsers => appDbContent.users;

        public void AddUser(User user)
        {
            appDbContent.users.Add(user);
            appDbContent.SaveChanges();
        }
        public void SaveDBChanges()
        {
            appDbContent.SaveChanges();
        }
    }
}
