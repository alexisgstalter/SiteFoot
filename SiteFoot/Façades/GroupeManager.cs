using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using SiteFoot.Models;
using System.Diagnostics;

namespace SiteFoot.Façades
{
    public class GroupeManager
    {


        public static DataTable getAll()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * FROM Groupe", myConnection);
            DataTable table = new DataTable(); //On créé une table pour avoir une structure de nos données
            source.Fill(table);
            myConnection.Close();

            return table;
        }

        public static int getIdByName(String gr_name)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT id from Groupe where nom=@nom", myConnection);
            command.Parameters.AddWithValue("@nom", gr_name);
            SqlDataReader reader = command.ExecuteReader();
            int id = 1;
            if (reader.Read())
            {
                id = reader.GetInt32(0);

            }
            reader.Close();
            myConnection.Close();
            return id;
        }

        public static List<Groupe> GetById(int id_user)
        {
            List<Groupe> groupes = new List<Groupe>();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Groupe where id in (Select id_groupe from GroupesUtilisateur where id_utilisateur=@id)", myConnection);
            command.Parameters.AddWithValue("@id", id_user);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Groupe groupe = new Groupe();
                    groupe.Id = (int)reader[0];
                    groupe.Nom = reader[1].ToString();
                    groupe.Code_Groupe = reader[2].ToString();
                    groupe.Droit_gerer_buvette = Convert.ToBoolean(reader[4]);
                    groupe.Droit_gerer_entrainement = Convert.ToBoolean(reader[5]);
                    groupe.Droit_entrainement_autre = Convert.ToBoolean(reader[6]);
                    groupe.Droit_gerer_formateur = Convert.ToBoolean(reader[7]);
                    groupe.Droit_formateur_autre = Convert.ToBoolean(reader[8]);
                    groupe.Droit_poster_annonce = Convert.ToBoolean(reader[9]);
                    groupes.Add(groupe);
                }
            }



            reader.Close();
            myConnection.Close();
            return groupes;
        }

        public static bool Exists(Groupe g)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Groupe where nom=@nom", myConnection);
            command.Parameters.AddWithValue("@nom", g.Nom);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                myConnection.Close();
                return true;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return false;
            }
        }

        public static bool ExistsForUpdate(Groupe g)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Groupe where nom=@nom AND id<>@id", myConnection);
            command.Parameters.AddWithValue("@nom", g.Nom);
            command.Parameters.AddWithValue("@id", g.Id);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                myConnection.Close();
                return true;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return false;
            }
        }

        public static bool Create(Groupe g)
        {
            bool created = false;

            if (Exists(g))
            {
                created = false;
            }
            else
            {
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                String query = "INSERT INTO Groupe (nom,code_groupe, droit_gerer_buvette, droit_gerer_entrainement,droit_entrainement_autres, droit_gerer_formateur, droit_formateur_autres, droit_poster_annonce) VALUES (@nom,@code_groupe,@droit_gerer_buvette,@droit_gerer_entrainement,@droit_entrainement_autres,@droit_gerer_formateur,@droit_formateur_autres, @droit_poster_annonce)";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@nom", g.Nom);
                command.Parameters.AddWithValue("@code_groupe", g.Code_Groupe);
                command.Parameters.Add("@droit_gerer_buvette", SqlDbType.TinyInt).Value = Convert.ToInt32(g.Droit_gerer_buvette);
                command.Parameters.Add("@droit_gerer_entrainement", SqlDbType.TinyInt).Value = Convert.ToInt32(g.Droit_gerer_entrainement);
                command.Parameters.Add("@droit_entrainement_autres", SqlDbType.TinyInt).Value =Convert.ToInt32( g.Droit_entrainement_autre);
                command.Parameters.Add("@droit_gerer_formateur", SqlDbType.TinyInt).Value = Convert.ToInt32(g.Droit_gerer_formateur);
                command.Parameters.Add("@droit_formateur_autres", SqlDbType.TinyInt).Value = Convert.ToInt32(g.Droit_formateur_autre);
                command.Parameters.Add("@droit_poster_annonce", SqlDbType.TinyInt).Value = Convert.ToInt32(g.Droit_poster_annonce);
                command.ExecuteNonQuery();

                myConnection.Close();
                created = true;
            }


            return created;
        }


        public static bool Delete(int id)
        {
            try
            {
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                String query = "DELETE FROM Groupe WHERE id=@id";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                myConnection.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool Update(Groupe g)
        {
            try
            {
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                String query = "UPDATE Groupe SET nom=@nom,code_groupe=@code_groupe, Droit_gerer_buvette=@droit_gerer_buvette,droit_gerer_entrainement=@droit_gerer_entrainement,droit_entrainement_autres=@droit_entrainement_autres,droit_gerer_formateur=@droit_gerer_formateur,droit_formateur_autres=@droit_formateur_autres, droit_poster_annonce=@droit_poster_annonce WHERE id=@id";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@nom", g.Nom);

                command.Parameters.AddWithValue("@code_groupe", g.Code_Groupe);
                command.Parameters.AddWithValue("@id", g.Id);
                command.Parameters.Add("@droit_gerer_buvette", SqlDbType.TinyInt).Value = g.Droit_gerer_buvette;
                command.Parameters.Add("@droit_gerer_entrainement", SqlDbType.TinyInt).Value = g.Droit_gerer_entrainement;
                command.Parameters.Add("@droit_entrainement_autres", SqlDbType.TinyInt).Value = g.Droit_entrainement_autre;
                command.Parameters.Add("@droit_gerer_formateur", SqlDbType.TinyInt).Value = g.Droit_gerer_formateur;
                command.Parameters.Add("@droit_formateur_autres", SqlDbType.TinyInt).Value = g.Droit_formateur_autre;
                command.Parameters.Add("@droit_poster_annonce", SqlDbType.TinyInt).Value = g.Droit_poster_annonce;
                command.ExecuteNonQuery();
                myConnection.Close();
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }


    }
}