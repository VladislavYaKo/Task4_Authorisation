using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Interfaces;
using Task4_Authorisation.Data.Models;
using Task4_Authorisation.ViewModels;

namespace Task4_Authorisation.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsers usersRepository;

        public UsersController(IUsers iUsers)
        {
            usersRepository = iUsers;
        }

        [Authorize]
        public ViewResult UsersList()
        {
            //Проверка пользователя на авторизованность todo
            string debug = usersRepository.AllUsers.FirstOrDefault(u => u.email == "bitch@mail.ru").lastLogin;
            return View(usersRepository.AllUsers);
        }
    }
}
