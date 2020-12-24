using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Models;

namespace Task4_Authorisation.Data.Interfaces
{
    public interface IUsers
    {
        IEnumerable<User> AllUsers { get; }

        void AddUser(User user);
        public void SaveDBChanges();        
    }
}
