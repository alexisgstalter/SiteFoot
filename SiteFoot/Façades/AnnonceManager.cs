using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
            SqlDataAdapter source = new SqlDataAdapter("select titre, texte from Annonce where id=@id", myConnection);
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
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset and @offset+4  order by date desc", myConnection);
            source.SelectCommand.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        } 
        public static DataTable GetAnnoncesScrollByTerms(int offset, String term)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset and @offset+4 and (titre like '%' + @terme + '%' or texte like '%' + @terme + '%' or date like '%' + @terme + '%' or prenom + ' ' + nom like '%' + @terme + '%') order by date desc", myConnection);
            source.SelectCommand.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
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
    }
}