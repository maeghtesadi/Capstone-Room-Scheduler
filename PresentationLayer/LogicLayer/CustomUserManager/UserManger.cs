using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;

namespace CapstoneRoomScheduler.LogicLayer.CustomUserManager
{
    class UserManager
    {
        public bool IsValid(string username, string password)
        {
            UserCatalog userCatalog = new UserCatalog();
            //go through the usercatalog to find the user
            foreach(User user in userCatalog.registeredUsers)
            {
                if(user.name==username && user.password==password) {
                    return true;
                }
            }
            return false;
     
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