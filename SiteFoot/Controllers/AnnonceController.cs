using SiteFoot.Façades;
using SiteFoot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;



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
                    html += "<div class='card-panel'><a>" + row["chemin"].ToString() + "</a><a class='btn supp_pj' data-num_annonce='" + id_annonce + "' data-num_image='" + row["id"] + "' data-chemin='" + row["chemin"] + "'>X</button></div>";
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


        public JsonResult DeleteAnnonce(String id)
        {
            try
            {
                AnnonceManager.RemoveAnnonce(id);
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }


        public JsonResult UpdateAnnonce(int id_annonce_clef, String intitule_modif, String texte_modif)
        {
            try
            {
                AnnonceManager.UpdateAnnonce(id_annonce_clef, intitule_modif, texte_modif);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        
        public JsonResult DeletePieceJointe(int id_annonce, int id_image, String chemin)
        {
            try
            {
                AnnonceManager.DeletePJAnnonce(id_annonce, id_image, chemin);
                System.IO.File.Delete(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot\" + chemin);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }


         
        public JsonResult GetAnnoncesByTerme(int offset, String term)
        {
            try
            {
                //TEST Compte le nombre d'enregistrement pour la recherche avec l'offset
                int nb_annonce = 0;
                DataTable Nombre_enregistrement = AnnonceManager.GetNombreAnnonce();
                if (Nombre_enregistrement.Rows.Count > 0)
                {
                    nb_annonce = int.Parse(Nombre_enregistrement.Rows[0]["nb_annonce"].ToString());
                }
                
                
                DataTable annonces = AnnonceManager.GetAnnoncesScrollByTerms(offset,term,nb_annonce);
                string html = "";

                foreach (DataRow row in annonces.Rows)
                {
                    html += "<li><div class='card-panel'><center><h2>" + row["titre"].ToString() + "</h2></center><br><blockquote>Auteur: " + row["prenom"].ToString() + " " + row["nom"].ToString() + "</blockquote><section>" + row["texte"].ToString() + "</section><br>";

                    DataTable img = AnnonceManager.GetPiecesJointes(int.Parse(row["id"].ToString()));

                    foreach (DataRow i in img.Rows)
                    {
                        html += "<img class='responsive-img' src='/Fichiers SiteFoot/" + i["chemin"].ToString() + "'/>";
                    }

                    html += "<blockquote> Date : " + row["date"].ToString() + "</blockquote></div></li>";
                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }


    }

}
