using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using vita.EInvoicing.Dto;
using vita.TenantConfigurations.Dtos;
using System.Linq;

namespace vita.Utils
{
    public static class EmailSender
    {
        public static void SendEmail(string attachmentpath, InvoiceRequest invoiceData, EmailDto emailSetting)
        {

            // SMTP server details
            string smtpHost = emailSetting.smtpHost;
            int smtpPort = emailSetting.smtpPort;
            string smtpUsername = emailSetting.smtpUserName;
            string smtpPassword = emailSetting.smtpPassword;

            string fromEmail = emailSetting.fromAddress;
            string toEmail = emailSetting.isenableemail ? invoiceData?.Buyer[0]?.ContactPerson?.Email ?? "ankit@abylle.com" : "ankit@abylle.com";

            var subject = emailSetting.subject;

            var body = emailSetting.body;
            MailMessage message = new MailMessage(fromEmail, toEmail);

            if (invoiceData.AdditionalData2.Count > 0)
            {
                var additionaldata = Newtonsoft.Json.Linq.JObject.Parse(invoiceData?.AdditionalData2[0]?.ToString());
                if (invoiceData.InvoiceNumber != "-1")
                    subject = subject.Replace("<inv>", invoiceData.InvoiceNumber);
                else
                    subject = subject.Replace("<inv>", invoiceData.BillingReferenceId);
                subject = subject.Replace("<so>", additionaldata["original_order_number"].ToString());
                subject = subject.Replace("<po>", additionaldata["purchase_order_no"].ToString());
                subject = subject.Replace("<date>", additionaldata["invoice_reference_date"].ToString());

                if (invoiceData.InvoiceNumber != "-1")
                    body = body.Replace("<inv>", invoiceData.InvoiceNumber);
                else
                    body = body.Replace("<inv>", invoiceData.BillingReferenceId);
                body = body.Replace("<so>", additionaldata["original_order_number"].ToString());
                body = body.Replace("<po>", additionaldata["purchase_order_no"].ToString());
                if (File.Exists(attachmentpath))
                {
                    Attachment attachment1 = new Attachment(attachmentpath);
                    attachment1.Name = invoiceData.BillingReferenceId?.ToString() + "_" + additionaldata["invoice_reference_date"].ToString() + "_" + additionaldata["original_order_number"].ToString() + ".pdf";
                    message.Attachments.Add(attachment1);
                }
            }
            else
            {
                if (File.Exists(attachmentpath))
                {
                    Attachment attachment1 = new Attachment(attachmentpath);
                    //attachment1.Name = invoiceData.BillingReferenceId.ToString() + "_" + additionaldata["invoiceRefDate"].ToString() + "_" + additionaldata["originalOrderNo"].ToString() + ".pdf";
                    message.Attachments.Add(attachment1);
                }
            }

            // Sender and recipient email addresses

            // Create a new MailMessage

            message.Subject = subject;
            message.Body = body;

            var ccEmail = emailSetting.ccEmails.Split(';').ToList();

            foreach (var cc in ccEmail)
            {
                message.CC.Add(cc);

            }
            //message.CC.Add(ccEmail);

            // Create a SmtpClient and configure it
            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Send the email
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
