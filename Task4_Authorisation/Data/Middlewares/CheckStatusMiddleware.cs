using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task4_Authorisation.Data.Services;

namespace Task4_Authorisation.Data.Middlewares
{
    public class CheckStatusMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TrackStatusesChangeService trackService;

        public CheckStatusMiddleware(RequestDelegate next, TrackStatusesChangeService trackService)
        {
            this.next = next;
            this.trackService = trackService;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            int? myId = context.Session.GetInt32("userId");
            if (myId != null ? ShouldUnloggine((int)myId) : false)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                trackService.ResetUnlogine((int)myId);
                context.Response.Redirect("/Account/Login");
            }
            else
                await next.Invoke(context);
        }
        private bool ShouldUnloggine(int myId)
        {
            return trackService.UnloginedIds.Contains(myId);
        }
    }
}
