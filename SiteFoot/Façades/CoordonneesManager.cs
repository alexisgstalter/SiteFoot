using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiteFoot.Façades
{
    public class CoordonneesManager
    {
        public static DataTable GetAllJoueurs()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.*, b.clear_password from Utilisateurs a, Passwords b where id_equipe is not null and a.id = b.id_user", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetJoueurByEquipe(int id_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Utilisateurs where id_equipe=@id_equipe", myConnection);
            source.SelectCommand.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetJoueurByName(String name)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Utilisateurs where id_equipe is not null and (nom like '%' + @nom + '%' or prenom like '%' + @nom + '%' or prenom + ' ' + nom like '%' + @nom + '%')", myConnection);
            source.SelectCommand.Parameters.Add("@nom", SqlDbType.VarChar).Value = name;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetEquipeByIdEntraineur(int id_entraineur)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Equipe where id in (select id_equipe from EntraineurParEquipe where id_entraineur=@id_entraineur)", myConnection);
            source.SelectCommand.Parameters.Add("@id_entraineur", SqlDbType.VarChar).Value = id_entraineur;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetEquipeByIDMembre(int id_membre)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Equipe where id in (select id_equipe from Utilisateurs where id=@id_membre)", myConnection);
            source.SelectCommand.Parameters.Add("@id_membre", SqlDbType.VarChar).Value = id_membre;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
    }
}