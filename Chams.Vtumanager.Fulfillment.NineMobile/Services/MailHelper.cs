using Chams.Vtumanager.Fulfillment.NineMobile.Models;
using Chams.Vtumanager.Provisioning.Entities.EtopUp.Mtn;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _config;
        private readonly SmtpSettings _settings;
        private readonly ILogger<MailHelper> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public MailHelper(IConfiguration configuration, ILogger<MailHelper> logger)
        {
            _config = configuration;
            _settings = new SmtpSettings();
            _settings = _config.GetSection("SmtpSettings").Get<SmtpSettings>();
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        /// <param name="recipientAddress"></param>
        /// <param name="ccAddress"></param>
        public bool SendMail(string subject, string mailBody, string recipientAddress)
        {
            bool mailstatus = false;
            //_clientId = _config["SmtpSettings:V1:Username"];
            int PORT = int.Parse(_settings.Port);
            string HOST = _settings.HostUrl;

            String SUBJECT = subject;

            // The body of the email
            String BODY = mailBody;

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(_settings.SenderMail, _settings.MailFromFriendlyname);  //nairaplus@pelpay.africa
            message.To.Add(new MailAddress(recipientAddress));

            var ccAddresses = _settings.CSLMail.Split(";");
            foreach (var to in ccAddresses)
            {
                message.CC.Add(new MailAddress(to));
            }
            //message.CC.Add(new MailAddress(ccAddress));
            message.Subject = SUBJECT;
            message.Body = BODY;
             message.IsBodyHtml = true;

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials

                client.Credentials =
                new NetworkCredential(_settings.SenderMail, _settings.Password);
                
                //client.UseDefaultCredentials = false;

                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    Console.WriteLine("Email sent...");
                    return mailstatus;

                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed ot send mail to {recipientAddress} exception: {ex.Message} {JsonConvert.SerializeObject(ex.InnerException)}" );
                    return mailstatus;
                    //Console.WriteLine("The email was not sent.");
                    //Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }
    }
}
