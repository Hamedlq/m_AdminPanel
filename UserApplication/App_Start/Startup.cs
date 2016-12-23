using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace WebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie"
                //ExpireTimeSpan = TimeSpan.FromDays(30),
                //SlidingExpiration = false
                //LoginPath = new PathString("/user/login")
            });
        }
    }
}