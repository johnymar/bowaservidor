using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.Engine.Helpers
{
    public class Email
    {
        public bool IsValid { get; private set; }

        public string Mail { get; set; }
        public string Password { get; set; }
        public string SMPTServer { get; set; }
        public int SMPTPort { get; set; }
        public bool UseSSL { get; set; }

        public Email(string email)
        {
            IsValid = IsValidEmail(email);
            if(IsValid)
                Mail = email;
        }

        public bool LoadParameters()
        {
            Mail = "";
            Password = "";
            SMPTServer = "";
            SMPTPort = 0;
            UseSSL = true;

            return false;
        }

        public bool Send(string to, string subject, string content)
        {            
            MailMessage correo = new MailMessage(Mail, to, subject, content);
            correo.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(SMPTServer);
            smtp.EnableSsl = UseSSL;
            smtp.UseDefaultCredentials = false;
            smtp.Host = SMPTServer;
            smtp.Port = SMPTPort;
            smtp.Credentials = new System.Net.NetworkCredential(Mail, Password);
            
            try
            {
                smtp.Send(correo);
                return true;
            }
            catch { return false; }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
