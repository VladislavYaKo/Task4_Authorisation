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
using Task4_Authorisation.Data.Services;

namespace Task4_Authorisation.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsers usersRepository;
        private readonly TrackStatusesChangeService trackService;

        public UsersController(IUsers iUsers, TrackStatusesChangeService trackService)
        {
            usersRepository = iUsers;
            this.trackService = trackService;
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
                    .Invoke(this, new object[] { affectedUsers });
                await result;
            }
            catch
            { }            
            return RedirectToAction("UsersList");
        }
        [Authorize]
        private async Task DeleteUsers(User[] affectedUsers)
        {
            await usersRepository.DeleteUsers(affectedUsers);
            trackService.Unlogine(affectedUsers.Select(u => u.id).ToArray());
        }
        [Authorize]
        private async Task BlockUsers(User[] affectedUsers)
        {
            await usersRepository.ChangeStatus(affectedUsers, true);
            trackService.Unlogine(affectedUsers.Select(u => u.id).ToArray());
        }
        [Authorize]
        private async Task UnblockUsers(User[] affectedUsers)
        {
            await usersRepository.ChangeStatus(affectedUsers, false);
            trackService.ResetUnlogine(affectedUsers.Select(u => u.id).ToArray());
        }
    }
}
