using SiteFoot.Façades;
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
                    html += "<li><div class='card-panel'><center><h2>" + row["titre"].ToString() + "</h2></center><br><section>" + row["texte"].ToString() + "</section></div></li>";
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
    }

}
