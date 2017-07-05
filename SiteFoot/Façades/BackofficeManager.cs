using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiteFoot.Façades
{
    public class BackofficeManager
    {


        public static DataTable GetAllEquipe()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Equipe order by nom_equipe", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static void RemoveEquipe(String id)
        {
            String requete = "DELETE FROM Equipe WHERE id=@id";
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand(requete, myConnection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        
        public static void SaveEquipe(String nom_equipe, String liste_categorie, String entraineur, String ecusson)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("insert into Equipe (nom_equipe,categorie,ecusson,id_entraineur) values(@nom_equipe, @liste_categorie, @ecusson, @entraineur)", myConnection);
            cmd.Parameters.Add("@nom_equipe", SqlDbType.VarChar).Value = nom_equipe;
            cmd.Parameters.Add("@liste_categorie", SqlDbType.VarChar).Value = liste_categorie;
            cmd.Parameters.Add("@entraineur", SqlDbType.VarChar).Value = entraineur;
            cmd.Parameters.Add("@ecusson", SqlDbType.VarChar).Value = ecusson;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }

    }
}