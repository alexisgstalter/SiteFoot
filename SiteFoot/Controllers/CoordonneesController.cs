using SiteFoot.Façades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    public class CoordonneesController : Controller
    {
        //
        // GET: /Coordonnees/

        public ActionResult CoordonneesEntraineurs()
        {
            return View();
        }
        public ActionResult GetCoordonneesEntraineurs()
        {
            try
            {
                DataTable entraineurs = CalendrierManager.GetAllEntraineurs();
                String html = "";
                int compteur_ligne = 1;
                foreach (DataRow row in entraineurs.Rows)
                {
                    if (compteur_ligne % 2 != 0)
                    {
                        html += "<div class='row' ><div class='card-panel col s5' style='margin-right : 2%;'><center><strong><h4>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</h4></strong></center><p>tél : " + row["telephone"].ToString() + "</p><p>email : " + row["email"].ToString() + "</p><p>Equipes :</p>";
                        DataTable equipes = CoordonneesManager.GetEquipeByIdEntraineur(int.Parse(row["id"].ToString()));
                        html += "<table class=''>";
                        foreach (DataRow e in equipes.Rows)
                        {
                            html += "<tr><td><img src='/Fichiers SiteFoot/" + e["ecusson"].ToString() + "'/></td><td>" + e["nom_equipe"].ToString() + "</td></tr>";
                        }
                        html += "</table>";
                        html += "</div><div class='col s2'></div>";
                    }
                    else
                    {
                        html += "<div class='card-panel col s5' style='margin-right : 2%;'><center><strong><h4>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</h4></strong></center><p>tél : " + row["telephone"].ToString() + "</p><p>email : " + row["email"].ToString() + "</p><p>Equipes :</p>";
                        DataTable equipes = CoordonneesManager.GetEquipeByIdEntraineur(int.Parse(row["id"].ToString()));
                        html += "<table class=''>";
                        foreach (DataRow e in equipes.Rows)
                        {
                            html += "<tr><td><img src='/Fichiers SiteFoot/" + e["ecusson"].ToString() + "'/></td><td>" + e["nom_equipe"].ToString() + "</td></tr>";
                        }
                        html += "</table>";
                        html += "</div></div>";
                    }
                }
                return Content(html);
            }
            catch (Exception)
            {
                return Content(null);
            }
        }
        public ActionResult CoordonneesJoueurs()
        {
            return View();
        }
        public JsonResult GetMembre(int id_equipe, String name)
        {
            try
            {
                if (id_equipe != 0 && name != "")
                {
                    throw new Exception("pas plus d'un critère à la fois");
                }
                DataTable membres;
                String html = "";
                if (id_equipe != 0)
                {
                    membres = CoordonneesManager.GetJoueurByEquipe(id_equipe);
                }
                else
                {
                    membres = CoordonneesManager.GetJoueurByName(name);
                }
                int compteur_ligne = 1;
                foreach (DataRow row in membres.Rows)
                {
                    if (compteur_ligne % 2 != 0)
                    {

                        html += "<div class='row spec_render' ><div class='card-panel'><center><strong><h4>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</h4></strong></center><p>tél : " + row["telephone"].ToString() + "</p><p>email : " + row["email"].ToString() + "</p><p>Adresse : " + row["adresse"].ToString() + "</p><p>Equipes :</p>";
                        DataTable equipe = CoordonneesManager.GetEquipeByIDMembre(int.Parse(row["id"].ToString()));
                        html += "<table class=''>";
                        foreach (DataRow e in equipe.Rows)
                        {
                            html += "<tr><td><img src='/Fichiers SiteFoot/" + e["ecusson"].ToString() + "'/></td><td>" + e["nom_equipe"].ToString() + "</td></tr>";
                        }
                        html += "</table>";
                        html += "</div>";
                    }
                    else
                    {
                        html += "<div class='card-panel'><center><strong><h4>" + row["prenom"].ToString() + " " + row["nom"].ToString() + "</h4></strong></center><p>tél : " + row["telephone"].ToString() + "</p><p>email : " + row["email"].ToString() + "</p><p>Adresse : " + row["adresse"].ToString() + "</p><p>Equipes :</p>";
                        DataTable equipe = CoordonneesManager.GetEquipeByIDMembre(int.Parse(row["id"].ToString()));
                        html += "<table class=''>";
                        foreach (DataRow e in equipe.Rows)
                        {
                            html += "<tr><td><img src='/Fichiers SiteFoot/" + e["ecusson"].ToString() + "'/></td><td>" + e["nom_equipe"].ToString() + "</td></tr>";
                        }
                        html += "</table>";
                        html += "</div></div>";
                    }
                    compteur_ligne++;
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
