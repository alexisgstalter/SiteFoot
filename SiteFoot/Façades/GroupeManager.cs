using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using SiteFoot.Models;

namespace SiteFoot.Facades
{
    public class GroupeManager
    {


        public static DataTable getAll()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
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
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
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

        public static Groupe GetById(int id)
        {
            Groupe groupe = new Groupe();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Groupe where id=@id", myConnection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                groupe.Id = (int)reader[0];
                groupe.Nom = reader[1].ToString();
                groupe.Code_Groupe = reader[2].ToString();

            }
            else
            {
                groupe = null;
            }
            reader.Close();
            myConnection.Close();
            return groupe;
        }

        public static bool Exists(Groupe g)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
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
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
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
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                String query = "INSERT INTO Groupe (nom,code_groupe) VALUES (@nom,@code_groupe)";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@nom", g.Nom);
                command.Parameters.AddWithValue("@code_groupe", g.Code_Groupe);

                command.ExecuteNonQuery();

                myConnection.Close();
                created = true;
            }


            return created;
        }

        public static DataTable getAllFromReflex() //On récupère tout les codes groupes dans reflex
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLProd"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT DISTINCT utcgpg as \"code_groupe\" FROM reflex.hlutilp", myConnection);
            DataTable table = new DataTable(); //On créé une table pour avoir une structure de nos données
            source.Fill(table);
            myConnection.Close();

            return table;
        }

        public static bool Delete(int id)
        {
            try
            {
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
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
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                String query = "UPDATE Groupe SET nom=@nom,code_groupe=@code_groupe WHERE id=@id";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@nom", g.Nom);

                command.Parameters.AddWithValue("@code_groupe", g.Code_Groupe);
                command.Parameters.AddWithValue("@id", g.Id);
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