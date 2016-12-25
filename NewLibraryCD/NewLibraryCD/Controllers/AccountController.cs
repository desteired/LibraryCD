using NewLibraryCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NewLibraryCD.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;

                using (ModelDataBaseContext context = new ModelDataBaseContext())
                {
                    user = context.Users.FirstOrDefault(u => u.UserName == model.UserName);
                }

                if(user == null)
                {
                    using (ModelDataBaseContext context = new ModelDataBaseContext())
                    {
                        context.Users.Add(new User() { BirthDate = model.BirthDate, Email = model.Email, Password = model.Password, UserName = model.UserName });
                        context.SaveChanges();

                        user = context.Users.Where(u => u.UserName == model.UserName && u.Password == model.Password).FirstOrDefault();
                    }

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "Disks");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
                
            }
            else
            {
                    ModelState.AddModelError("", "Ошибка при регистрации");
            }
            return View(model);
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;

                using (ModelDataBaseContext context = new ModelDataBaseContext())
                {
                    user = context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);
                }

                if(user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Index", "Disks");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем не существует.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при попытке войти");
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Disks");
        }
    }
}