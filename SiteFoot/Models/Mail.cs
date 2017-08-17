using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace SiteFoot.Models
{
    class Mail
    {
        private String From; //Adresse source
        private String To;  //Destinataire
        private String Subject; //Sujet
        private String Body;    //Corps du message
        private String AttachedFilePath; //Le chemin de la pièce jointe

        private MailMessage message;

        public MailMessage Message
        {
            get { return message; }
            set { message = value; }
        }
        private SmtpClient smtp;

        public SmtpClient Smtp
        {
            get { return smtp; }
            set { smtp = value; }
        }

        public void SetBody(String body)
        {
            this.Body = body;
        }

        public void SetSubject(String subject)
        {
            this.Subject = subject;
        }

        public Mail(String from, String to, String subject, String body, String attachedfilepath)
        {
            smtp = new SmtpClient();
            message = new MailMessage();
            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Body = body;
            this.AttachedFilePath = attachedfilepath;
        }

        public Mail(String from, String subject, String body)
        {
            smtp = new SmtpClient();
            message = new MailMessage();
            this.From = from;
            this.Subject = subject;
            this.Body = body;
        }
        public Mail(String from, String to, String subject, String body)
        {
            smtp = new SmtpClient();
            message = new MailMessage();
            this.From = from;
            this.Subject = subject;
            this.Body = body;
            this.To = to;
        }

        public void AddTo(String addto)
        {
            message.To.Add(new MailAddress(addto));
        }

        public void AddBCC(String bcc)
        {
            message.Bcc.Add(bcc);
        }

        public void AddCC(String cc)
        {
            message.CC.Add(cc);
        }
        public bool Send()
        {
            try
            {
                bool ok = true;
                message.From = new MailAddress(this.From);
                if (this.To != null && this.To != "")
                {
                    message.To.Add(new MailAddress(this.To));
                }
                message.Subject = this.Subject;
                message.IsBodyHtml = true;
                message.Body = this.Body;

                if (this.AttachedFilePath != "" && this.AttachedFilePath != null)
                {
                    message.Attachments.Add(new Attachment(this.AttachedFilePath));
                }

                smtp.Host = "10.0.0.4";
                smtp.Port = 25;
                //smtp.Credentials = new System.Net.NetworkCredential(this.From,"ljung+1234"); //Avec cette ligne le nom de l'emmeteur est automatiquement défini, sinon il faut utiliser le Display Name plsu haut :)
                //Permet d'éviter de bloquer de thread

                ok = true;
                Thread t = new Thread(() => trysend());
                t.Start();
                Debug.WriteLine(ok);
                return ok;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace + " " + ex.Message + " " + ex.InnerException);
                return false;
            }

        }

        public bool SendLitiges()
        {
            try
            {
                bool ok = true;
                message.From = new MailAddress(this.From);
                message.To.Add(new MailAddress(this.To));
                message.Subject = this.Subject;
                message.IsBodyHtml = true;
                message.Body = this.Body;

                if (this.AttachedFilePath != "")
                {
                    message.Attachments.Add(new Attachment(this.AttachedFilePath));
                }

                smtp.Host = "10.0.0.4";
                smtp.Port = 25;
                //smtp.Credentials = new System.Net.NetworkCredential(this.From,"ljung+1234"); //Avec cette ligne le nom de l'emmeteur est automatiquement défini, sinon il faut utiliser le Display Name plsu haut :)
                //Permet d'éviter de bloquer de thread

                ok = true;
                ok = trysend();
                Debug.WriteLine(ok);
                return ok;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace + " " + ex.Message + " " + ex.InnerException);
                return false;
            }

        }

        public bool trysend()
        {

            try
            {
                smtp.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}