using EntityModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Budget
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            else 
            {
                var documentDb = new DocumentDBRepository<Income>("Income", httpContext.User.Identity.GetUserId());

                System.Web.HttpContext.Current.Application["Salary"] = documentDb.GetUserItemAsync();
                return true; 
            }
        }
    }
}