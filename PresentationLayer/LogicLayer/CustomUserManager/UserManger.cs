using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneRoomScheduler.LogicLayer.CustomUserManager
{
    class UserManager
    {
        public bool IsValid(string username, string password)
        {
            if(username=="nim")
            return true;
            return false;
           //return True whenever
        }
    }
    public class LoggedIn : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
                return false;
            return base.AuthorizeCore(httpContext);
        }
    }
}