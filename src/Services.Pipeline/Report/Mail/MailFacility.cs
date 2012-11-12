namespace Services.Pipeline.Report.Mail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;

    public class MailFacility : IMailFacility
    {
        public MailFacility()
        {
            this.Message = new MailMessage();
            this.Smtp = new SmtpClient();
        }

        public MailFacility(string host, int port)
        {
            this.Message = new MailMessage();
            this.Smtp = new SmtpClient(host, port);
        }
        
        public SmtpClient Smtp { get; private set; }
        public MailMessage Message { get; private set; }

        public void ReadTemplate(string path)
        {
            using (var reader = new StreamReader(path))
            {
                this.Message.Body = reader.ReadToEnd();
            }
        }

        public void ReplaceBodyWithKeys(IDictionary<string, string> keys)
        {
            this.Message.Body = this.ReplaceWithKeys(keys, this.Message.Body);
        }

        public void ReplaceSubjectWithKeys(IDictionary<string, string> keys)
        {
            this.Message.Subject = this.ReplaceWithKeys(keys, this.Message.Subject);
        }

        public void Send()
        {
            this.Smtp.Send(this.Message);
        }

        private string ReplaceWithKeys(IEnumerable<KeyValuePair<string, string>> keys, string message)
        {
            if (keys == null)
            {
                throw new ArgumentNullException();
            }
            return keys.Aggregate(message, (current, key) => current.Replace(key.Key, key.Value));
        }
    }
}
