using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Task4_Authorisation.Data.Interfaces;
using Task4_Authorisation.Data.Models;
using Task4_Authorisation.ViewModels;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Task4_Authorisation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsers usersRepository;

        public AccountController(IUsers iUsers)
        {
            usersRepository = iUsers;
        }

        private bool IsCorrectAuthorisation(string login, string password)
        {
            User user = usersRepository.AllUsers.FirstOrDefault(u => u.email == login);
            if (user == null)
            {
                return false;
            }
            using (SHA256 sha256 = SHA256.Create())
            {
                if (user.passwordHash.SequenceEqual(sha256.ComputeHash(System.Text.Encoding.Unicode.GetBytes(password))))
                    return true;
                else
                    return false;
            }
        }
        private bool IsBlocked(string email)
        {
            User user = usersRepository.AllUsers.FirstOrDefault(u => u.email == email);
            return user != null ? user.isBlocked : false;
        }
        private bool HasUser(string email)
        {
            if (usersRepository.AllUsers.FirstOrDefault(u => u.email == email) == null)
                return false;
            else
                return true;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsCorrectAuthorisation(model.Email, model.Password))
                    if (!IsBlocked(model.Email))
                    {
                        await Authenticate(model.Email);
                        SetIdToSession(model.Email);
                        await usersRepository.UpdateLastLogin(model.Email);
                        return RedirectToAction("UsersList", "Users");
                    }
                    else                    
                        ModelState.AddModelError("", "Вы заблокированы");                    
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!HasUser(model.Email))
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        await usersRepository.AddUser(new User
                        {
                            name = model.Name,
                            email = model.Email,
                            passwordHash = sha256.ComputeHash(System.Text.Encoding.Unicode.GetBytes(model.Password)),
                            registrationDate = DateTime.Now, lastLogin = "-", 
                            isBlocked = false
                        });
                    } 
                    await Authenticate(model.Email);
                    return RedirectToAction("Login", "Account");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private void SetIdToSession(string email)
        {
            HttpContext.Session.SetInt32("userId", usersRepository.AllUsers.First(u => u.email == email).id);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
