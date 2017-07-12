using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFoot.Façades;
using SiteFoot.Models;
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
        public ActionResult CreateUser()
        {
            return View();
        }


        public ActionResult SaisieEquipe()
        {
            return View("~/Views/Backoffice/CreateEquipe.cshtml");
        }



        public JsonResult LoadAllEquipe()
        {
            /*try
            {*/
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

                        String nom_entraîneur = BackofficeManager.GetAllEntraineursEquipe(int.Parse(liste_equipe.Rows[i]["id"].ToString()));

                        
                        html = html + "<tr>";
                        html = html + "<td>" + liste_equipe.Rows[i]["nom_equipe"].ToString() + "</td><td>" + liste_equipe.Rows[i]["categorie"].ToString() + "</td><td>" + nom_entraîneur + "</td><td><img class='img-responsive' src='/Fichiers SiteFoot/" + liste_equipe.Rows[i]["ecusson"].ToString() + "' /></td><td data-id_ligne='" + liste_equipe.Rows[i]["id"].ToString() + "' class='edit_ligne'><i class='material-icons prefix'>mode_edit</i><td class='delete delete_ligne' data-value='" + liste_equipe.Rows[i]["id"].ToString() + "'><i class='material-icons prefix'>clear</i></td>";
                    }
                    html = html + "</tr>";
                }
                html += "</tbody></table>";

                return Json(new { ok = true, html = html, hasResult = hasResult });
            /*}
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }*/
            }

        
        public JsonResult SaveEquipe(String nom_equipe, String liste_categorie, int[] entraineur, String ecusson)
        {
            try
            {
     
                if (nom_equipe == "" || liste_categorie == "" || entraineur.Length == 0 || ecusson == "")
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
                html = html + "<select id='entraineur' multiple>";
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

        public ActionResult LoadGroupes()
        {
            DataTable groupes = GroupeManager.getAll();
            string html = "";
            foreach (DataRow row in groupes.Rows)
            {
                html += "<option value='" + row["id"].ToString() + "'>" + row["nom"].ToString() + "</option>";
            }
            return Content(html);
        }

        public String GenerateBlocEcusson()
        {
            String html = "";
            html += "<div class='input-group has-feedback'><span class='input-group-addon'>Ecussion de l'équipe</span><input id='pj' class='form-control piece_jointe' type='file' multiple /></div></br>";
            html += "<div class='text-center'><button type='button' class='btn btn-primary' id='upload'/>Enregistrer ecusson</button></div><br>";
            return html;
        }

        public JsonResult GetUtilisateurs()
        {
            try
            {
                String html = "<table class='striped bordered highlight centered'><tr><thead><th>ID</th><th>Login</th><th>Prénom</th><th>Nom</th><th>Email</th><th>Téléphone</th><th>Editer</th><th>Supprimer</th></tr></thead><tbody>";
                DataTable utilisateurs = Utilisateur.getAll();
                foreach (DataRow row in utilisateurs.Rows)
                {
                    List<Groupe> grps = GroupeManager.GetById(int.Parse(row["id"].ToString()));
                    string groupes = "";
                    foreach (Groupe g in grps)
                    {
                        groupes += g.Nom + ",";
                    }
                    groupes = groupes.Remove(groupes.LastIndexOf(','), 1);
                    html += "<tr><td>" + row["id"].ToString() + "</td><td>" + row["login"].ToString() + "</td><td>" + row["prenom"].ToString() + "</td><td>" + row["nom"].ToString() + "</td><td>" + row["email"].ToString() + "</td><td>" + row["telephone"].ToString() + "</td><td><a><i class='material-icons dp48 edit'>mode_edit</i></td><td><i class='material-icons dp48 supp'>delete</i></td></tr>";
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