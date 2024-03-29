﻿namespace Hirundo.Model.Infrastructure
{
    using System;
    using System.Security;
    using System.Web;
    using System.Web.Security;

    public class UserContextProvider : IUserContextProvider
    {
        public const string UserContextKey = "__UserContextKey__";
        public const int CookieUsernameMaxLength = 40;
        private HttpContextBase httpContext;

        public UserContextProvider(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public UserContext GetCurrentUserContext()
        {
            if (this.httpContext.Items[UserContextKey] == null)
            {
                HttpCookie authCookie = this.httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                {
                    return null;
                }

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null || authTicket.Expired)
                {
                    throw new SecurityException("Invalid authentication ticket.");
                }

                this.httpContext.Items[UserContextKey] = new UserContext(authTicket.Name, authTicket.UserData);
            }

            return (UserContext)this.httpContext.Items[UserContextKey];
        }

        public void SetCurrentUserContext(UserContext userContext)
        {
            string username = userContext.Username;
            if (username.Length > CookieUsernameMaxLength)
            {
                username = username.Substring(0, CookieUsernameMaxLength);
            }

            string userData = this.httpContext.Server.UrlEncode(username);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
              2,
              userContext.UserId,
              DateTime.Now,
              DateTime.Now.Add(FormsAuthentication.Timeout),
              false, // isPersistent
              userData,
              FormsAuthentication.FormsCookiePath);

            HttpCookie authCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(authTicket));

            authCookie.HttpOnly = true;
            authCookie.Path = FormsAuthentication.FormsCookiePath;
            authCookie.Secure = FormsAuthentication.RequireSSL;

            if (FormsAuthentication.CookieDomain != null)
            {
                authCookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }

            this.httpContext.Response.Cookies.Add(authCookie);

            this.httpContext.Items[UserContextKey] = userContext;
        }

        public void ClearCurrentUserContext()
        {
            FormsAuthentication.SignOut();

            this.httpContext.Items[UserContextKey] = null;
        }
    }
}
