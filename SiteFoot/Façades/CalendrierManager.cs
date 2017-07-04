using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiteFoot.Façades
{
    public class CalendrierManager
    {
        public static DataTable GetEventsBuvette(String date_debut, String date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from TestBuvette where start > @date_debut and fin < @date_fin", myConnection);
            source.SelectCommand.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = DateTime.Parse(date_debut);
            source.SelectCommand.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = DateTime.Parse(date_fin);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetEventBuvetteById(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from TestBuvette where id=@id", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static void UpdateEventBuvette(int id, String titre, String responsable, DateTime date_debut, DateTime date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("update TestEvent set title=@titre, start=@date_debut, fin=@date_fin, utilisateur=@responsable where id=@id");
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = date_debut;
            cmd.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = date_fin;
            cmd.Parameters.Add("@titre", SqlDbType.VarChar).Value = titre;
            cmd.Parameters.Add("@responsable", SqlDbType.VarChar).Value = responsable;
            cmd.ExecuteNonQuery();
        }




        public static DataTable GetEventsEntrainement(String date_debut, String date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from entrainement where start > @date_debut and fin < @date_fin", myConnection);
            source.SelectCommand.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = DateTime.Parse(date_debut);
            source.SelectCommand.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = DateTime.Parse(date_fin);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetEventBuvetteById(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteWeb"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from TestBuvette where id=@id", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

    }
}