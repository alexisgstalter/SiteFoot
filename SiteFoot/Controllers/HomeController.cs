using SiteFoot.Façades;
using SiteFoot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SiteFoot.Controllers
{
    [CustomAuthorize(GroupeAllow = "Anonymous")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            Debug.WriteLine("cc");
            return View();
        }

        public ActionResult Annonces()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult Login(String username, String password)
        {
            try
            {
            //On récupère le nom d'utilisateur et le mot de passe
            User u = new User();
            u.Login = username;
            u.Password = password;

            //On essaye de connecter l'utilisateur
            if (Utilisateur.Connect(u))
            {
                User connected_user = Utilisateur.GetByName(u); //On récupère toutes les informations de l'utilisateur et on les place dans la base de données
                String authString = connected_user.Login + "#" + u.Password;    //On stocke le nom d'utilisateur et le mot de passe dans un cookie
                FormsAuthentication.SetAuthCookie(authString, true);
                //On ajoute dans un cookie l'activité et le dépot de l'utilisateur

                Session["CurrentUser"] = connected_user;    //On place l'utilisateur dans la session
                Session["CurrentGroupe"] = GroupeManager.GetById(connected_user.Id);
                return Json(new { ok = true, isGranted = true });
            }
            else
            {
                return Json(new { ok = true, isGranted = false });
            }

            }
            catch (Exception ex)
            {
                
                return Json(new { ok = false, error = ex.Message });
            }
        }
        public ActionResult Deconnexion()
        {
            //On supprime le cookie de connexion
            FormsAuthentication.SignOut();
            //On supprime toutes les sessions de l'utilisateur
            Session.RemoveAll();
            //On abandonne la session pour la rendre non valide
            Session.Abandon();
            //On supprime le cookie qui contient l'activité courrante de l'utilisateur
            return new RedirectResult("/");
        }
    }
}
