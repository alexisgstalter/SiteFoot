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
                        String nom_entraîneur = BackofficeManager.GetAllEntraineursEquipe(int.Parse(liste_equipe.Rows[i]["id"].ToString()));

                        html = html + "<tr>";
                        html = html + "<td>" + liste_equipe.Rows[i]["nom_equipe"].ToString() + "</td><td>" + liste_equipe.Rows[i]["categorie"].ToString() + "</td><td>" + nom_entraîneur + "</td><td><img class='img-responsive' src='/Fichiers SiteFoot/" + liste_equipe.Rows[i]["ecusson"].ToString() + "' /></td><td data-id_ligne='" + liste_equipe.Rows[i]["id"].ToString() + "' class='edit_ligne'><i class='material-icons prefix'>mode_edit</i><td class='delete delete_ligne' data-value='" + liste_equipe.Rows[i]["id"].ToString() + "'><i class='material-icons prefix'>clear</i></td>";
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

        
        public JsonResult SaveEquipe(String nom_equipe, String liste_categorie, String ecusson, int[] entraineur)
        {
            try
            {
                if (nom_equipe == "" || liste_categorie == "" || entraineur.Length == 0)
                {
                    throw new Exception("Veuillez renseigner tous les champs");
                }

                BackofficeManager.SaveEquipe(nom_equipe, liste_categorie, ecusson, entraineur);

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
                    return Json(new { ok = true, hasResult = true, nom_equipe = dataequipe.Rows[0]["nom_equipe"].ToString(), categorie = dataequipe.Rows[0]["categorie"].ToString(), ecusson = dataequipe.Rows[0]["ecusson"].ToString(), id_equipe = dataequipe.Rows[0]["id"].ToString() });
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


        public JsonResult UpdateEquipe(String nom_equipe_update, String categorie_update, int[] entraineur_update, String ecusson_update, int id_equipe_clef)
        {
            try
            {
                BackofficeManager.UpdateEquipe(entraineur_update, ecusson_update, id_equipe_clef);
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
                html = html + "<option value='' disabled>Sélectionnez un entraîneur</option>";
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
                for (int i = 0; i < ListeEntraineur.Rows.Count; i++)
                {
                    html = html + "<option value='" + ListeEntraineur.Rows[i]["id"].ToString() + "'>" + ListeEntraineur.Rows[i]["nom"].ToString() + " " + ListeEntraineur.Rows[i]["prenom"].ToString() + "</option>";
                }
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
            html += "<div class='input-group has-feedback'><span class='input-group-addon'>Ecusson de l'équipe</span><input id='pj' class='form-control piece_jointe' type='file' multiple /></div></br>";
            html += "<div class='text-center'><button type='button' class='btn btn-primary' id='upload'/>Enregistrer ecusson</button></div><br>";
            return html;
        }


        public JsonResult GetUtilisateurs()
        {
            try
            {
                String html = "<table class='striped bordered highlight centered'><thead><tr><th>ID</th><th>Login</th><th>Mot de passe</th><th>Groupes</th><th>Prénom</th><th>Nom</th><th>Email</th><th>Téléphone</th><th>Editer</th><th>Supprimer</th></tr></thead><tbody>";
                DataTable utilisateurs = Utilisateur.getAll();
                foreach (DataRow row in utilisateurs.Rows)
                {
                    User u = new User();
                    u.Id = int.Parse(row["id"].ToString());
                    String password = Utilisateur.GetClearPassword(u);
                    List<Groupe> grps = GroupeManager.GetById(int.Parse(row["id"].ToString()));
                    string groupes = "";
                    foreach (Groupe g in grps)
                    {
                        groupes += g.Nom + ",";
                    }
                    groupes = groupes.Remove(groupes.LastIndexOf(','), 1);
                    html += "<tr><td>" + row["id"].ToString() + "</td><td>" + row["login"].ToString() + "</td><td>"+ password +"</td><td>"+ groupes +"</td><td>" + row["prenom"].ToString() + "</td><td>" + row["nom"].ToString() + "</td><td>" + row["email"].ToString() + "</td><td>" + row["telephone"].ToString() + "</td><td class='edit'><a><i class='material-icons dp48'>mode_edit</i></td><td class='supp'><i class='material-icons dp48'>delete</i></td></tr>";
                }
                html += "</tbody></table>";
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult SaveUser(String login, String password, int[] groupes, String email, String telephone, String nom, String prenom)
        {
            try
            {
                User u = new User();
                u.Login = login;
                u.Password = password;
                u.Groupe = groupes.ToList();
                u.Email = email;
                u.Telephone = telephone;
                u.Salt = Hash.GetNewSaltKey();
                u.Nom = nom;
                u.Prenom = prenom;

                Utilisateur.Create(u);

                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult UpdateUser(int id, String login, String password, int[] groupes, String email, String telephone, String nom, String prenom)
        {
            try
            {
            User u = new User();
            u.Id = id;
            u.Login = login;
            u.Password = password;
            u.Groupe = groupes.ToList();
            u.Email = email;
            u.Telephone = telephone;
            u.Salt = Hash.GetNewSaltKey();
            u.Nom = nom;
            u.Prenom = prenom;

            Utilisateur.Update(u);

            return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult DeleteUser(int id)
        {
            try
            {


            Utilisateur.Delete(id);

            return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public ActionResult ParametrageEntrainement()
        {
            return View();
        }
        public JsonResult SaveConfigEntrainement(int[] equipes, int terrain, DateTime date_debut, DateTime date_fin, String[] jour_semaine, String heure_debut, String heure_fin, String intitule)
        {
            List<String> days_of_week = jour_semaine.ToList();
            var culture = new System.Globalization.CultureInfo("fr-FR");
            DateTime temp = date_debut;
            try
            {
                foreach (int equipe in equipes)
                {
                    date_debut = temp;
                    while (date_debut != date_fin)
                    {
                        if (days_of_week.Contains(culture.DateTimeFormat.GetDayName(date_debut.DayOfWeek)))
                        {
                            CalendrierManager.SaveEntrainement(intitule, DateTime.Parse(date_debut.ToString("dd/MM/yyyy") + " " + heure_debut + ":00"), DateTime.Parse(date_debut.ToString("dd/MM/yyyy") + " " + heure_fin + ":00"), terrain, equipe);
                        }
                        date_debut = date_debut.AddDays(1);
                    }
                }
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult DeleteConfigEntrainement(int[] equipes, DateTime date_debut, DateTime date_fin, String[] jour_semaine)
        {
            List<String> days_of_week = jour_semaine.ToList();
            var culture = new System.Globalization.CultureInfo("fr-FR");
            DateTime temp = date_debut;
            try
            {
                foreach (int equipe in equipes)
                {
                    date_debut = temp;
                    while (date_debut != date_fin)
                    {
                        if (days_of_week.Contains(culture.DateTimeFormat.GetDayName(date_debut.DayOfWeek)))
                        {
                            CalendrierManager.DeleteEntrainementByEquieAndDate(equipe, date_debut, date_debut.AddDays(1));
                        }
                        date_debut = date_debut.AddDays(1);
                    }
                }
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public ActionResult GestionDesGroupes()
        {
            return View();
        }
        public JsonResult GetAllGroupes()
        {
            try
            {
                DataTable groupes = GroupeManager.getAll();
                String html = "<table class='table bordered striped'><thead><th>Nom</th><th>Droit buvette</th><th>Droit entrainement</th><th>Droit sur l'entrainement des autres</th><th>Droit éducateur</th><th>Droit sur les autres éducateurs</th><th>Droit de publication d'annonces</th><th>Editer</th><th>Supprimer</th></thead><tbody>";

                foreach (DataRow row in groupes.Rows)
                {
                    html += "<tr><td>" + row["nom"].ToString() + "</td><td>" + row["droit_gerer_buvette"].ToString() + "</td><td>" + row["droit_gerer_entrainement"].ToString() + "</td><td>" + row["droit_entrainement_autres"].ToString() + "</td><td>" + row["droit_gerer_formateur"].ToString() + "</td><td>" + row["droit_formateur_autres"].ToString() + "</td><td>" + row["droit_poster_annonce"].ToString() + "</td><td data-value='" + row["id"].ToString() + "' class='edit'><i class='material-icons prefix'>mode_edit</i></td>";
                    if (row["estGroupeImportant"].ToString() == "1")
                    {
                        html += "<td></td></tr>";
                    }
                    else
                    {
                        html += "<td class='supp' data-value='" + row["id"].ToString() + "'><i class='material-icons prefix'>clear</i></td></tr>";
                    }
                }

                html += "</tbody></table>";

                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult SaveGroupe(String nom, bool droit_gerer_buvette, bool droit_gerer_entrainement, bool droit_entrainement_autres, bool droit_gerer_formateur, bool droit_formateur_autres, bool droit_poster_annonce)
        {
            try
            {
                Groupe g = new Groupe();
                g.Droit_poster_annonce = droit_poster_annonce;
                g.Droit_gerer_buvette = droit_gerer_buvette;
                g.Droit_formateur_autre = droit_formateur_autres;
                g.Droit_gerer_formateur = droit_gerer_formateur;
                g.Droit_gerer_entrainement = droit_gerer_entrainement;
                g.Droit_entrainement_autre = droit_entrainement_autres;
                g.Nom = nom;
                g.Code_Groupe = nom;
                GroupeManager.Create(g);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult DeleteGroupe(int id)
        {
            try
            {
                GroupeManager.Delete(id);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult UpdateGroupe(int id, String nom, bool droit_gerer_buvette, bool droit_gerer_entrainement, bool droit_entrainement_autres, bool droit_gerer_formateur, bool droit_formateur_autres, bool droit_poster_annonce)
        {
            try
            {
                Groupe g = new Groupe();
                g.Id = id;
                g.Droit_poster_annonce = droit_poster_annonce;
                g.Droit_gerer_buvette = droit_gerer_buvette;
                g.Droit_formateur_autre = droit_formateur_autres;
                g.Droit_gerer_formateur = droit_gerer_formateur;
                g.Droit_gerer_entrainement = droit_gerer_entrainement;
                g.Droit_entrainement_autre = droit_entrainement_autres;
                g.Nom = nom;
                g.Code_Groupe = nom;
                GroupeManager.Update(g);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public ActionResult GestionDesJoueurs()
        {
            return View();
        }
        public JsonResult GetJoueurs()
        {
            try
            {
                DataTable joueurs = CoordonneesManager.GetAllJoueurs();
                String html = "<table class='table bordered highlight'><thead><th>Prénom</th><th>Nom</th><th>Adresse</th><th>Telephone</th><th>E-mail</th><th>Equipe</th><th>Editer</th><th>Supprimer</th></thead><tbody>";
                
                foreach (DataRow row in joueurs.Rows)
                {
                    DataTable equipe = CoordonneesManager.GetEquipeByIDMembre(int.Parse(row["id"].ToString()));
                    html += "<tr><td>" + row["prenom"].ToString() + "</td><td>" + row["nom"].ToString() + "</td><td>" + row["adresse"].ToString() + "</td><td>" + row["telephone"].ToString() + "</td><td>" + row["email"].ToString() + "</td><td><img class='img-responsive' src='/Fichiers SiteFoot/" + equipe.Rows[0]["ecusson"].ToString() + "'/>  " + equipe.Rows[0]["nom_equipe"].ToString() + "</td><td class='edit' data-value='" + row["id"].ToString() + "'><a><i class='material-icons dp48'>mode_edit</i></td><td class='supp' data-value='" + row["id"].ToString() + "'><i class='material-icons dp48'>delete</i></td></tr>";

                }
                html += "</tbody></table>";
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult SaveJoueur(int id_equipe, String prenom, String nom, String adresse, String telephone, String email)
        {
            try
            {
                BackofficeManager.SaveJoueur(id_equipe, prenom, nom, adresse, telephone, email);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult UpdateJoueur(int id, int id_equipe, String prenom, String nom, String adresse, String telephone, String email)
        {
            try
            {
                BackofficeManager.UpdateJoueur(id, id_equipe, prenom, nom, adresse, telephone, email);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult DeleteJoueur(int id)
        {
            try
            {
                BackofficeManager.DeleteJoueur(id);
                return Json(new { ok = true });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
    }

}