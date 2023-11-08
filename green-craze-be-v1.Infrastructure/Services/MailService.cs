using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Common.Options;
using green_craze_be_v1.Application.Intefaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string name, string email, string content, string title)
        {
            try
            {
                var options = _configuration.GetOptions<MailJetOptions>("MailJet");
                MailjetClient client = new(options.PublicAPIKey, options.PrivateAPIKey);
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                   .Property(Send.FromEmail, options.SendFromEmail)
                   .Property(Send.FromName, options.SendFromName)
                   .Property(Send.Subject, title)
                   .Property(Send.HtmlPart, content)
                   .Property(Send.Recipients, new JArray {
                            new JObject {
                                 {"Email", email},
                                 {"Name", name}
                            }
                       });

                _ = Task.Run(() => client.PostAsync(request));
            }
            catch
            {
                throw;
            }
        }
    }
}