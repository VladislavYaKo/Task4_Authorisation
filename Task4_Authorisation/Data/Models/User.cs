using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task4_Authorisation.Data.Models
{
    public class User
    {
        public int id { get; set; }
        [MaxLength(20)]
        public string name { get; set; }
        [MaxLength(30)]
        public string email { get; set; }
        public byte[] passwordHash { get; set; } 
        public DateTime registrationDate { get; set; }
        [MaxLength(20)]
        public string lastLogin { get; set; }
        public bool isBlocked { get; set; }
    }
}
