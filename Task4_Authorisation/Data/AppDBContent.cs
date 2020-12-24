using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Models;

namespace Task4_Authorisation.Data
{
    public class AppDBContent : DbContext
    {
        public DbSet<User> users { get; set; }
        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
        {

        }
    }
}
