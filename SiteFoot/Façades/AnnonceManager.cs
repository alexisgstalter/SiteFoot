using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace SiteFoot.Façades
{
    public class AnnonceManager
    {
        public static DataTable GetAllAnnonces()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.id, a.titre, a.date, b.nom, b.prenom from Annonce a, Utilisateurs b where a.id_auteur=b.id order by date desc", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetAnnonceById(int id_annonce)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select titre, texte, id from Annonce where id=@id", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id_annonce;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetPiecesJointes(int id_annonce)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from ImageAnnonce where id in (select id_image from BanqueImage where id_annonce=@id_annonce)", myConnection);
            source.SelectCommand.Parameters.Add("@id_annonce", SqlDbType.Int).Value = id_annonce;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetAnnoncesScroll(int offset)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset and @offset+4  order by a.date desc", myConnection);
            source.SelectCommand.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static DataTable GetNombreAnnonce()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT Count(*) as 'nb_annonce' from annonce", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static DataTable GetAnnoncesScrollByTerms(int offset, String term, int nb_annonce)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset and @offset+@nb_annonce and (titre like '%' + @term + '%' or texte like '%' + @term + '%' or convert(varchar, date , 103) like '%' + @term + '%' or prenom + ' ' + nom like '%' + @term + '%') order by a.date desc", myConnection);
            source.SelectCommand.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            source.SelectCommand.Parameters.Add("@term", SqlDbType.VarChar).Value = term;
            source.SelectCommand.Parameters.Add("@nb_annonce", SqlDbType.Int).Value = nb_annonce;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static int SaveAnnonce(String titre, String texte, int id_auteur, DateTime date) //on récupèrera l'id pour faire le lien avec les pièces jointes
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("insert into Annonce output INSERTED.ID values (@titre, @texte,@date,@id_auteur)", myConnection);
            source.Parameters.Add("@titre", SqlDbType.VarChar).Value = titre;
            source.Parameters.Add("@texte", SqlDbType.VarChar).Value = texte;
            source.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
            source.Parameters.Add("@id_auteur", SqlDbType.VarChar).Value = id_auteur;
            int id = (int)source.ExecuteScalar();
            myConnection.Close();
            return id;
        }

        public static void SavePieceJointeAnnonce(int id_annonce, String chemin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("insert into ImageAnnonce output INSERTED.ID values (@chemin)", myConnection);
            source.Parameters.Add("@chemin", SqlDbType.VarChar).Value = chemin;
            int id = (int)source.ExecuteScalar();
            source = new SqlCommand("insert into BanqueImage values (@id_annonce, @id_image)", myConnection);
            source.Parameters.Add("@id_annonce", SqlDbType.Int).Value = id_annonce;
            source.Parameters.Add("@id_image", SqlDbType.Int).Value = id;
            source.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void RemoveAnnonce(String id)
        {
            String requete = "DELETE FROM Annonce WHERE id=@id";
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand(requete, myConnection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void UpdateAnnonce(int id_annonce_clef, String intitule_modif, String texte_modif)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("UPDATE Annonce set titre=@titre, texte=@texte where id=@id", myConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id_annonce_clef;
            cmd.Parameters.Add("@titre", SqlDbType.VarChar).Value = intitule_modif;
            cmd.Parameters.Add("@texte", SqlDbType.VarChar).Value = texte_modif;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }


        public static String GetPJ(int id_annonce)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("select * from ImageAnnonce where id in (select id_image from BanqueImage where id_annonce=@id_annonce)", myConnection);
            cmd.Parameters.Add("@id_annonce", SqlDbType.Int).Value = id_annonce;
            string res = cmd.ExecuteScalar().ToString();
            myConnection.Close();
            return res;
        }


        public static void DeletePJAnnonce(int id_annonce, int id_image, String chemin)
        {
            String requete = "DELETE from BanqueImage WHERE id_annonce=@id_annonce and id_image=@id_image";
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand(requete, myConnection);
            command.Parameters.AddWithValue("@id_annonce", id_annonce);
            command.Parameters.AddWithValue("@id_image", id_image);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}