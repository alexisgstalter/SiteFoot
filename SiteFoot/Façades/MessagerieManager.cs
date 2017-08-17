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
    public class MessagerieManager
    {
        public static DataTable GetAllMessages(int id_user)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.*, b.login, b.nom, b.prenom from Message a, Utilisateurs b, DestinataireMessage c where c.id_destinataire=@id_destinataire and a.id = c.id_message and a.id_expediteur = b.id order by a.id desc", myConnection);
            source.SelectCommand.Parameters.AddWithValue("@id_destinataire", id_user);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetMessageById(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.*, b.login, b.nom, b.prenom from Message a, Utilisateurs b where a.id_expediteur = b.id and a.id=@id", myConnection);
            source.SelectCommand.Parameters.AddWithValue("@id", id);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetJoueurParCategorie(String categorie)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Utilisateurs where id_equipe in (select id from Equipe where categorie=@categorie)", myConnection);
            source.SelectCommand.Parameters.AddWithValue("@categorie", categorie);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static void ChangeTopLu(int id, bool top)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("update Message set lu=@lu where id=@id", myConnection);
            cmd.Parameters.Add("@lu", SqlDbType.TinyInt).Value = top;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void DeleteMessage(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("delete from Message where id=@id", myConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }

        public static String[] GetAutoCompleteLogin(String partial_name)
        {
            List<String> res = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT login + ' : ' + (prenom + ' ' + nom) from Utilisateurs where login like '%' + @nom + '%' or prenom + ' ' + nom like '%' + @nom + '%'", myConnection);
            command.Parameters.Add("@nom", SqlDbType.VarChar).Value = partial_name;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                res.Add(reader[0].ToString());
            }
            return res.ToArray();
        }

        public static void SendMessage(List<User> users, String sujet, String texte, int id_expediteur)
        {
            DateTime date_reception = DateTime.Now;
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("insert into Message (sujet, texte, id_expediteur, lu, date_reception) output INSERTED.ID values (@sujet, @texte, @id_expediteur, 0, @date)", myConnection);
            cmd.Parameters.Add("@sujet", SqlDbType.VarChar).Value = sujet;
            cmd.Parameters.Add("@texte", SqlDbType.VarChar).Value = texte;
            cmd.Parameters.Add("@id_expediteur", SqlDbType.Int).Value = id_expediteur;
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date_reception;
            int id_message = int.Parse(cmd.ExecuteScalar().ToString());
            foreach (User u in users)
            {
                cmd = new SqlCommand("insert into DestinataireMessage values (@id_message, @id_destinataire)", myConnection);
                cmd.Parameters.AddWithValue("@id_message", id_message);
                cmd.Parameters.AddWithValue("@id_destinataire", u.Id);
                cmd.ExecuteNonQuery();
            }
            myConnection.Close();
        }
    }
}