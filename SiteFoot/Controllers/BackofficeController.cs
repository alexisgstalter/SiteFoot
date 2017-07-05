using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFoot.Façades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SiteFoot.Controllers
{
    [CustomAuthorize(GroupeAllow="ADMIN")]
    public class BackofficeController : Controller
    {
        public ActionResult CreateEquipe()
        {
            return View();
        }


        public JsonResult LoadAllEquipe()
        {
            try
            {
                String html = "<table id='equipetable' class='highlight'>";
                html += "<thead>";
                html += "<tr role='row'>";
                html += "<th>Nom de l'équipe</th><th>Catégorie</th><th>Entraîneur</th><th>Ecusson</th><th>Modifier</th><th>Supprimer</th>";
                html += "</tr></thead>";
                html += "<tbody>";
                bool hasResult = false;
                DataTable liste_equipe = BackofficeManager.GetAllEquipe();

                if (liste_equipe.Rows.Count > 0)
                {
                    String nom_entraîneur = "";

                    hasResult = true;
                    for (int i = 0; i < liste_equipe.Rows.Count; i++)
                    {
                        html = html + "<tr>";
                        html = html + "<td>" + liste_equipe.Rows[i]["nom_equipe"].ToString() + "</td><td>" + liste_equipe.Rows[i]["categorie"].ToString() + "</td><td>" + liste_equipe.Rows[i]["id_entraineur"].ToString() + "</td><td><img class='img-responsive' src='/Content/images/" + liste_equipe.Rows[i]["ecusson"].ToString() + "' /></td><td data-value='" + liste_equipe.Rows[i]["id"].ToString() + "' class='edit edit_ligne'><i class='material-icons prefix'>mode_edit</i><td class='delete delete_ligne' data-value='" + liste_equipe.Rows[i]["id"].ToString() + "'><i class='material-icons prefix'>clear</i></td>";
                    }
                    html = html + "</tr>";
                }
                html += "</tbody></table>";

                return Json(new { ok = true, html = html, hasResult = hasResult });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        
        public JsonResult SaveEquipe(String nom_equipe, String liste_categorie, String entraineur, String ecusson)
        {
            try
            {
    
                if (nom_equipe == "" || liste_categorie == "" || entraineur == "" || ecusson == "")
                {
                    throw new Exception("Veuillez renseigner tous les champs");
                }

                BackofficeManager.SaveEquipe(nom_equipe, liste_categorie, entraineur, ecusson);
                return Json(new { ok = true });

            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }


        public JsonResult DeleteEquipe(String id)
        {
            try
            {
                BackofficeManager.RemoveEquipe(id);
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }



    }

}