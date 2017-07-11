﻿using Newtonsoft.Json;
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
                bool valid = false;
                
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
                    hasResult = true;
                    for (int i = 0; i < liste_equipe.Rows.Count; i++)
                    {
                        int id_entraineur = int.Parse(liste_equipe.Rows[i]["id_entraineur"].ToString());

                        String nom_entraîneur = "";

                        DataTable RecupInfoEntraineur = BackofficeManager.GetInfoEntraineur(id_entraineur);
                        if (RecupInfoEntraineur.Rows.Count > 0)
                        {
                            nom_entraîneur = RecupInfoEntraineur.Rows[0]["nom"].ToString() + " " + RecupInfoEntraineur.Rows[0]["prenom"].ToString();

                            valid = true;
                        }
                        
                        html = html + "<tr>";
                        html = html + "<td>" + liste_equipe.Rows[i]["nom_equipe"].ToString() + "</td><td>" + liste_equipe.Rows[i]["categorie"].ToString() + "</td><td>" + nom_entraîneur + "</td><td><img class='img-responsive' src='/Fichiers Foot/" + liste_equipe.Rows[i]["ecusson"].ToString() + "' /></td><td data-id_ligne='" + liste_equipe.Rows[i]["id"].ToString() + "' class='edit_ligne'><i class='material-icons prefix'>mode_edit</i><td class='delete delete_ligne' data-value='" + liste_equipe.Rows[i]["id"].ToString() + "'><i class='material-icons prefix'>clear</i></td>";
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


        public JsonResult EditEquipe(int id)
        {
            try
            {
                DataTable dataequipe = BackofficeManager.GetEventEquipeById(id);
                if (dataequipe.Rows.Count > 0)
                {
                    return Json(new { ok = true, hasResult = true, nom_equipe = dataequipe.Rows[0]["nom_equipe"].ToString(), categorie = dataequipe.Rows[0]["categorie"].ToString(), ecusson = dataequipe.Rows[0]["ecusson"].ToString(), id_entraineur = dataequipe.Rows[0]["id_entraineur"].ToString() });
                }
                else
                {
                    return Json(new { ok = true, hasResult = false });
                }
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }


        public JsonResult UpdateEquipe(String nom_equipe_update, String categorie_update, String entraineur_update, String ecusson_update)
        {
            try
            {
                BackofficeManager.UpdateEquipe(nom_equipe_update, categorie_update, entraineur_update, ecusson_update);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }


        public ActionResult GenerateListeEntraineur()
        {
            DataTable ListeEntraineur = BackofficeManager.GetlisteEntraineur();

            String html = "";
            if (ListeEntraineur.Rows.Count > 0)
            {
                html = html + "<select id='entraineur'>";
                html = html + "<option value='' disabled selected>Sélectionnez un entraîneur</option>";
                for (int i = 0; i < ListeEntraineur.Rows.Count; i++)
                {
                    html = html + "<option value='" + ListeEntraineur.Rows[i]["id"].ToString() + "'>" + ListeEntraineur.Rows[i]["nom"].ToString() + " " + ListeEntraineur.Rows[i]["prenom"].ToString() + "</option>";
                }
                html = html + "</select>";
            }
            return Content(html);
        }


        public ActionResult GenerateListeEntraineurUpdate()
        {
            DataTable ListeEntraineur = BackofficeManager.GetlisteEntraineur();

            String html = "";
            if (ListeEntraineur.Rows.Count > 0)
            {
                html = html + "<select id='entraineur_edit'>";
                for (int i = 0; i < ListeEntraineur.Rows.Count; i++)
                {
                    html = html + "<option value='" + ListeEntraineur.Rows[i]["id"].ToString() + "'>" + ListeEntraineur.Rows[i]["nom"].ToString() + " " + ListeEntraineur.Rows[i]["prenom"].ToString() + "</option>";
                }
                html = html + "</select>";
            }
            return Content(html);
        }
    }
}