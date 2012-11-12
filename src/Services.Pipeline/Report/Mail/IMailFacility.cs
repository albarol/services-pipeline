namespace Services.Pipeline.Report.Mail
{
    using System.Collections.Generic;
    using System.Net.Mail;

    public interface IMailFacility
    {
        SmtpClient Smtp { get; }
        MailMessage Message { get; }
        void ReadTemplate(string path);
        void ReplaceBodyWithKeys(IDictionary<string, string> keys);
        void ReplaceSubjectWithKeys(IDictionary<string, string> keys);
        void Send();
    }
}
