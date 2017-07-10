using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace SiteFoot.Façades
{
    public class Hash
    {
        public static String GetHashSHA256(String textToHash) //Permet de hasher une chaîne de caractère
        {
            SHA256 hash = SHA256Managed.Create(); //On créé un nouveau hash
            byte[] hashValue = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(textToHash)); //Le tabelau contiendra le texte hashé
            String hashString = "";
            foreach (byte x in hashValue) //On converti le tableau de byte en chaîne de caractère
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString; //On retourne la chaîne hashée
        }

        public static String GetNewSaltKey() //Permet de générer une nouvelle clé de salage
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[20];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }
    }
}