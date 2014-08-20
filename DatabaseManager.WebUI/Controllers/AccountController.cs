using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatabaseManager.WebUI.Infrastructure.Abstract;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using DatabaseManager.WebUI.Models;
using DatabaseManager.WebUI.Infrastructure.Authentication;

namespace DatabaseManager.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider authProvider;
        private IUserRepository repository;

        public AccountController(IAuthProvider auth, IUserRepository repo)
        {
            authProvider = auth;
            repository = repo;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = repository.GetUser(model.Username);

                if (user == null)
                {
                    ModelState.AddModelError("NonExistentUser", "User does not exist");
                    return View();
                }
                else if (authProvider.Authenticate(model.Username, model.Password, user.Salt, user.PasswordHash))
                {
                    return Redirect(Url.Action("List", "Database", new { welcome = true }));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (repository.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "There already exists a user with that username.");
                    return View();
                }
                else if (model.Password != model.ReEnteredPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View();
                }
                else
                {
                    Tuple<string, string> credentials = new PasswordManager().GeneratePasswordHash(model.Password);
                    User user = new User {
                        Username = model.Username,
                        Salt = credentials.Item1,
                        PasswordHash = credentials.Item2
                    };
                    repository.SaveUser(user);
                    authProvider.Authenticate(user.Username, model.Password, user.Salt, user.PasswordHash);
                    return Redirect(Url.Action("List", "Database"));
                }

            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}