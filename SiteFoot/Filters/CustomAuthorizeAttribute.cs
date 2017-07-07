using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SiteFoot.Façades;
using SiteFoot.Models;

namespace SiteFoot
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public String GroupeAllow { get; set; } //Permettra de vérifier si le groupe de la session correspond à ce groupe
        public String Page { get; set; }  //Permettra de vérifier que l'utilisateur dispose bien d'un accès à cette page

        public override void OnAuthorization(AuthorizationContext filterContext)
        {   
            //Si les utilisateurs anonymes peuvent accéder à l'application
            if(GroupeAllow == "Anonymous")
            {
                //On refresh la session au cas ou
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (Utilisateur.IsAccessGranted(filterContext.HttpContext))
                    {
                        //L'utilisateur est bien connecté et ses informations d'authentifications sont valides

                        //Tout est ok, il faut maintenant vérifier que les paramètres passés au filtre sont ok
                        //Pas besoins de gérer les groupes car le groupe est anonyme.
                    }
                    else
                    {
                        //Comme nous sommes sur une page publique, si l'utilisateur connecté n'es plus connecté,
                        //Ceci signifie que ses infrmations d'authentifications ne sont plus valides
                        //On le deconnecte et on le renvoie sur la page d'accueil
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }
            }
            else
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (Utilisateur.IsAccessGranted(filterContext.HttpContext))
                    {
                        //L'utilisateur est bien connecté et ses informations d'authentifications sont valides

                        //Tout est ok, il faut maintenant vérifier que les paramètres passés au filtre sont ok
                        if (String.IsNullOrEmpty(GroupeAllow))
                        {
                            //C'est ok, on ne vérifie que la connexion de l'utilisateur
                        }
                        else
                        {
                            //On souhaite vérifier que le groupe de l'utilisateur est identique au groupe passé en paramètre
                            List<Groupe> g = (List<Groupe>)filterContext.HttpContext.Session["CurrentGroupe"];
                            bool ok = false;
                            foreach (Groupe grp in g)
                            {
                                if (grp.Nom == GroupeAllow)
                                {
                                    ok = true;
                                }
                            }
                            if (ok)
                            {
                                //C'est ok
                            }
                            else
                            {
                                filterContext.Result = new RedirectToRouteResult(new
                                 RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                            }
                        }
                        //On souhaite vérifier que l'utilisateur a accès à la page demandée
                        if (String.IsNullOrEmpty(Page))
                        {
                            //C'est ok, on ne vérifie que la connexion de l'utilisateur
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                }
            }
            
        }
    }
}