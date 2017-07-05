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
                html += "<th>Nom de l'équipe</th><th>Entraîneur</th><th>Ecusson</th><th>Supprimer</th>";
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
                        html = html + "<td>" + liste_equipe.Rows[i]["nom_equipe"].ToString() + "</td><td>" + nom_entraîneur + "</td><td class='delete delete_ligne' data-value='" + liste_equipe.Rows[i]["id"].ToString() + "'></td>";
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



    }

}