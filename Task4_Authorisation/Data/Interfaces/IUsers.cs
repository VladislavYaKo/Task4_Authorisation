﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Models;

namespace Task4_Authorisation.Data.Interfaces
{
    public interface IUsers
    {
        IEnumerable<User> AllUsers { get; }

        public Task AddUser(User user);
        public Task DeleteUsers(User[] users);
        public Task ChangeStatus(User[] users, bool newStatus);
        public Task UpdateLastLogin(string email);
    }
}
