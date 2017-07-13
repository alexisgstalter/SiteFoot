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
        public static DataTable GetAnnoncesScroll(int offset)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join banqueimage c on b.id=c.id_annonce left join imageannonce d on c.id_image = d.id left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset+1 and @offset+5  order by date desc", myConnection);
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
            SqlDataAdapter source = new SqlDataAdapter("select a.* from (select b.titre, b.texte, b.date,b.id,row_number() over (order by b.id) as 'num', e.prenom, e.nom from annonce b left join banqueimage c on b.id=c.id_annonce left join imageannonce d on c.id_image = d.id left join utilisateurs e on b.id_auteur=e.id) a where a.num between @offset+1 and @offset+5 and (titre like '%' + @terme + '%' or texte like '%' + @terme + '%' or date like '%' + @terme + '%' or prenom + ' ' + nom like '%' + @terme + '%') order by date desc", myConnection);
            source.SelectCommand.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static int SaveAnnonce(String titre, String texte, int id_auteur, DateTime date)
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
    }
}