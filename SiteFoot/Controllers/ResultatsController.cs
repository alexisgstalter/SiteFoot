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
    public class ResultatsController : Controller
    {
        //
        // GET: /Match/

        public ActionResult Championnat()
        {
            return View();
        }
        public ActionResult Match()
        {
            return View();
        }
        public String AutocompleteAdversaire(String partial_name)
        {
            /*try
            {*/
                String[] content = ResultatManager.GetAutoComplete(Server.HtmlEncode(partial_name));
                string result = JsonConvert.SerializeObject(content);
                return result;
            /*}
            catch (Exception)
            {
                return null;
            }*/
        }

        public JsonResult GetMatchs(int id_equipe, String adversaire, DateTime date_debut, DateTime date_fin)
        {
            try
            {
                DataTable matchs = ResultatManager.GetMatchs(id_equipe, adversaire, date_debut, date_fin);
                String html = "";
                if (matchs.Rows.Count > 0)
                {
                    html = "<table class='table bordered highlight'><thead><tr><th>Equipe</th><th>Adversaire</th><th>Score</th><th>Date</th>";
                    User u = (User)Session["CurrentUser"];
                    List<Groupe> grps = new List<Groupe>();
                    if (u != null)
                    {
                        grps = GroupeManager.GetById(u.Id);
                    }
                    bool can_manage_matchs = false;
                    foreach (Groupe g in grps)
                    {
                        if (g.Droit_gerer_match)
                        {
                            can_manage_matchs = true;
                        }
                    }
                    if (can_manage_matchs)
                    {
                        html += "<th>Editer</th><th>Supprimer</th>";
                    }
                    html += "</tr></thead><tbody>";
                    foreach (DataRow row in matchs.Rows)
                    {
                        html += "<tr><td><img class='img-responsive icon' src='/Fichiers SiteFoot/" + row["ecusson_equipe"].ToString() + "'/> " + row["nom_equipe"].ToString() + " - " + row["categorie_equipe"].ToString() + "</td><td><img class='img-responsive icon' src='/Fichiers SiteFoot/" + row["ecusson_adversaire"].ToString() + "'/> " + row["nom"].ToString() + " - " + row["categorie_adversaire"].ToString() + "</td><td>" + row["score"].ToString() + "</td><td>" + row["date"].ToString().Split(' ')[0] + "</td>";
                        if (can_manage_matchs)
                        {
                            html += "<td data-value='" + row["id"].ToString() + "' class='edit'><i class='material-icons prefix'>mode_edit</i></td><td data-value='" + row["id"].ToString() + "' class='supp'><i class='material-icons prefix'>clear</i></td>";
                        }
                        html += "</tr>";
                    }
                    html += "</tbody></table>";
                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult SaveAdversaire(String nom, String categorie)
        {
            try
            {
                int id = ResultatManager.SaveAdversaire(nom, categorie);

                return Json(new { ok = true, id = id });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult DeleteAdversaire(String nom, String categorie)
        {
            try
            {
                ResultatManager.DeleteAdversaire(nom, categorie);

                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult SaveMatch(int id_equipe, String adversaire, int score_equipe, int score_adversaire, DateTime date)
        {
            try
            {
                int id_adversaire = ResultatManager.GetAdversaireByNameAndCategory(adversaire);

                if (id_adversaire == -1)
                {
                    throw new Exception("erreur : l'adversaire n'existe pas");
                }

                ResultatManager.SaveMatch(id_equipe, id_adversaire, score_equipe + " - " + score_adversaire, date);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult UpdateMatch(int id, int id_equipe, String adversaire, int score_equipe, int score_adversaire, DateTime date)
        {
            try
            {
                int id_adversaire = ResultatManager.GetAdversaireByNameAndCategory(adversaire);

                if (id_adversaire == -1)
                {
                    throw new Exception("erreur : l'adversaire n'existe pas");
                }

                ResultatManager.UpdateMatch(id, id_equipe, id_adversaire, score_equipe + " - " + score_adversaire, date);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult DeleteMatch(int id)
        {
            try
            {


                ResultatManager.DeleteMatch(id);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
    }
}
