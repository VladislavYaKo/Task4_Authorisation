using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            return View(usersRepository.AllUsers);
        }
        [HttpPost]
        public async Task<IActionResult> ProcessList(string actionBtn)
        {
            string[] ids = Request.Form["choosenUser"].ToArray();
            User[] affectedUsers = usersRepository.AllUsers.Where(u => ids.Contains(u.id.ToString())).ToArray();
            try
            {
                Task result = (Task)this.GetType()
                    .GetMethod(actionBtn + "Users", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(this, new object[] { affectedUsers});
                await result;
            }
            catch(Exception e)
            {
                string debug = e.Message;
            }
            
            return RedirectToAction("UsersList");
        }

        private async Task DeleteUsers(User[] affectedUsers)
        {
            await usersRepository.DeleteUsers(affectedUsers);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private void BlockUsers(User[] affectedUsers)
        {

        }
        private void UnblockUsers(User[] affectedUsers)
        {

        }
    }
}
