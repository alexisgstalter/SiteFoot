using Newtonsoft.Json;
using SiteFoot.Façades;
using SiteFoot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    public class MessagerieController : Controller
    {
        //
        // GET: /Messagerie/

        public ActionResult BoiteDeReception()
        {
            return View();
        }
        public JsonResult GetAllMessages()
        {
            try
            {
                User u = (User)Session["CurrentUser"];
                DataTable Messages = MessagerieManager.GetAllMessages(u.Id);
                String html = "<table class='table table-bordered'><thead><th>Sujet</th><th>Expediteur</th><th>Date de réception</th><th>Lu</th><th>Supprimer</th></thead><tbody>";
                foreach (DataRow row in Messages.Rows)
                {
                    string etat = "";
                    if (row["lu"].ToString() == "1")
                    {
                        etat = "<i class='material-icons'>mail_outline</i>";
                    }
                    else
                    {
                        etat = "<i class='material-icons'>mail</i>";
                    }
                    html += "<tr data-value='"+ row["id"].ToString() +"'><td class='sujet'>" + row["sujet"].ToString() + "</td><td>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</td><td>" + row["date_reception"].ToString().Split(' ')[0] + "</td><td>" + etat + "</td><td class='supp'><i class='material-icons'>clear</i></td></tr>";
                }
                html += "</tbody></table>";
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult ChangeTopLu(int id)
        {
            try
            {
                MessagerieManager.ChangeTopLu(id, true);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult GetMessage(int id)
        {
            try
            {
                DataTable message = MessagerieManager.GetMessageById(id);
                String html = "";
                if (message.Rows.Count > 0)
                {
                    html += "<center><h3 id='sujet_message'>Sujet : " + message.Rows[0]["sujet"].ToString() + "</h3></center><br/>";
                    html += "<p id='expediteur' data-value='" + message.Rows[0]["login"].ToString() + "'>Envoyé par : " + message.Rows[0]["prenom"].ToString() + " " + message.Rows[0]["nom"].ToString() + "</p>";
                    html += "<p>" + message.Rows[0]["texte"].ToString() + "</p>";
                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult GetMessageElements(int id)
        {
            try
            {
                DataTable message = MessagerieManager.GetMessageById(id);
                if (message.Rows.Count > 0)
                {
                    return Json(new { ok = true, expediteur = message.Rows[0]["login"].ToString() + ";", sujet = message.Rows[0]["sujet"].ToString(), texte = message.Rows[0]["texte"].ToString() });
                }
                else
                {
                    return Json(new { ok = false, error = "Le message n'existe pas" });
                }
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public String AutocompleteLogin(String partial_name)
        {
            try
            {
                String[] content = MessagerieManager.GetAutoCompleteLogin(partial_name);
                string result = JsonConvert.SerializeObject(content);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult SendMessage(String[] logins, String sujet, String texte)
        {
            try
            {
                User usr = (User)Session["CurrentUser"];
                List<User> users = new List<User>();
                foreach (String login in logins)
                {
                    User u = new User();
                    u.Login = login;
                    u = Utilisateur.GetByName(u);
                    if (u != null)
                    {
                        users.Add(u);
                    }

                }
                MessagerieManager.SendMessage(users, sujet, texte, usr.Id);
                foreach (User u in users)
                {
                    Mail m = new Mail("n.hirtz@junglogistique.fr", u.Telephone + "@contact-everyone.fr", "Nouveau message sur le site du FC Sélestat", "FC Sélestat : Tu as un nouveau message, pour le consulter, connectes-toi sur fcselestat.ddns.net et vas dans ta boite de réception");
                    m.Send();
                }
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult DeleteMessage(int id)
        {
            try
            {
                MessagerieManager.DeleteMessage(id);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult LoadListJoueurParCategorie(String categorie)
        {
            try
            {
                DataTable joueurs = MessagerieManager.GetJoueurParCategorie(categorie);
                String html = "";
                foreach (DataRow row in joueurs.Rows)
                {
                    html += "<option value='"+ row["login"].ToString() +"'>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</option>";
                }
                return Json(new{ok=true, html=html});
            }
            catch (Exception e)
            {
                return Json(new{ok=false, error=e.Message});
            }
        }
    }
}
