using JiraDashboard.Models.DTO;
using JiraDashboard.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JiraDashboard.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                if (model.username == GenericHelper.username && model.password == GenericHelper.password)
                {
                    FormsAuthentication.SetAuthCookie(model.username.ToString(), true);
                    HttpCookie cookieUser = new HttpCookie(model.username);
                    HttpContext.Response.Cookies.Add(cookieUser);

                    FormsAuthentication.SetAuthCookie(model.password.ToString(), true);
                    HttpCookie cookiePassword = new HttpCookie(model.password);
                    HttpContext.Response.Cookies.Add(cookiePassword);
                    
                    if (model.rememberme == true)
                    {
                        HttpCookie cookieFlag = new HttpCookie(Convert.ToString(model.rememberme));
                        HttpContext.Response.Cookies.Add(cookieFlag);
                    }

                    return RedirectToAction("Issue", "Dashboard");

                }
                else
                {
                    ViewBag.LoginStatus = "Username or password is incorrect.";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}