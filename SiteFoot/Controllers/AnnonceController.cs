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
    public class AnnonceController : Controller
    {
        //
        // GET: /Annonce/

        public ActionResult Annonce()
        {
            return View(@"~/Views/Home/Annonces.cshtml");
        }
        public ActionResult GestionAnnonce()
        {
            return View(@"~/Views/Home/gerer_annonces.cshtml");
        }
        public JsonResult GetAnnonces(int offset)
        {
            try
            {
                DataTable annonces = AnnonceManager.GetAnnoncesScroll(offset);
                string html = "";

                foreach (DataRow row in annonces.Rows)
                {
                    html += "<li><div class='card-panel'><center><h2>" + row["titre"].ToString() + "</h2></center><br><blockquote>Auteur: "+ row["prenom"].ToString() + " " + row["nom"].ToString() +"</blockquote><section>" + row["texte"].ToString() + "</section><br>";

                    DataTable img = AnnonceManager.GetPiecesJointes(int.Parse(row["id"].ToString()));

                    foreach (DataRow i in img.Rows)
                    {
                        html += "<img class='responsive-img' src='/Fichiers SiteFoot/" + i["chemin"].ToString() + "'/>";
                    }

                    html += "<blockquote> Date : "+ row["date"].ToString() +"</blockquote></div></li>";

                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult GetGestionAnnonces()
        {
            try
            {
                DataTable annonces = AnnonceManager.GetAllAnnonces();
                string html = "<table class='striped bordered highlight'><thead><tr><th>Titre</th><th>Auteur</th><th>Date</th><th>Editer</th><th>Supprimer</th></tr></thead><tbody>";

                foreach (DataRow row in annonces.Rows)
                {
                    html += "<tr><td>" + row["titre"].ToString() + "</td><td>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</td><td>" + DateTime.Parse(row["date"].ToString()).ToString("dd/MM/yyyy HH:mm") + "</td><td class='edit' data-id='"+ row["id"].ToString() +"'><a><i class='material-icons dp48'>mode_edit</i></td><td class='supp' data-id='"+ row["id"].ToString() +"'><i class='material-icons dp48'>delete</i></td></tr>";
                }
                html += "</tbody></table>";
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult GetAnnonceById(int id_annonce)
        {
            try
            {
                DataTable annonce = AnnonceManager.GetAnnonceById(id_annonce);
                DataTable piecesjointes = AnnonceManager.GetPiecesJointes(id_annonce);
                string html = "";
                foreach (DataRow row in piecesjointes.Rows)
                {
                    html += "<div class='card-panel'><a>" + row["chemin"].ToString() + "</a><button class='btn supp_pj' type='button'>X</button></div>";
                }
                return Json(new { ok = true, titre = annonce.Rows[0]["titre"].ToString(), texte = annonce.Rows[0]["texte"].ToString(), html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult SaveGestionAnnonces(String titre, String texte)
        {
            try
            {
                User u = (User)Session["CurrentUser"];
                DateTime date = DateTime.Now;
                int id_annonce = AnnonceManager.SaveAnnonce(titre, texte, u.Id, date);
                return Json(new { ok = true, id = id_annonce });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
    }

}
