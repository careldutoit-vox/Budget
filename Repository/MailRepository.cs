using AE.Net.Mail;
using EntityModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Plus.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class MailRepository
    {
        UserCredential _credentials;
        static string ApplicationName = "BudgetCLientMail";
        public MailRepository()
        {
            GetCreadentials();
        }
        
        private async void GetCreadentials()
        {
            var scopes = new[] { GmailService.Scope.GmailReadonly };
            //////var uri = new Uri("ms-appx://client_id.json");
            ////ClientSecrets secret = new ClientSecrets(){
            ////    ClientId = "157077024985-hdi3jkdmppbl40l3qr6ma8h22ggve0hp.apps.googleusercontent.com",
            ////    ClientSecret = "QXWDdsOQDl7gb7McGUqWCuya"
            ////};
            ////_credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(secret, scopes, "user", CancellationToken.None);
            using (var stream =
                new FileStream(@"C:\Development\Stuff\Budget\Budget\client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = @"C:\Development\Stuff\Budget\Budget\gmail-dotnet-quickstart.json";

                _credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "dutoit.carel7",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        public async void ReadAllMails(string label)
        {
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("fnb");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }
    }
    public class CustomMailService
    {
        public PlusService service;
        public UserCredential credential;

        public List<Expenses> GetMails(string _client_id, string _client_secret)
        {
            CustomMailService result = new CustomMailService();
            List<Expenses> emailModels = new List<Expenses>();
            //If you want to test new gmail account, 
            //Go to the browser, log off, log in with the new account, 
            //and then change this 'tmpUser'
            var tmpUser = "dutoit.carel7@gmail.com";

            try
            {
                result.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = _client_id, ClientSecret = _client_secret },
                                                                         new[] { "https://mail.google.com/ email" },
                                                                         tmpUser,
                                                                         CancellationToken.None,
                                                                         new FileDataStore("Analytics.Auth.Store")).Result;
            }
            catch (Exception ex)
            {

            }

            if (result.credential != null)
            {

                result.service = new PlusService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = result.credential,
                    ApplicationName = "Google mail",
                });


                Google.Apis.Plus.v1.Data.Person me = result.service.People.Get("me").Execute();

                Google.Apis.Plus.v1.Data.Person.EmailsData myAccountEmail = me.Emails.Where(a => a.Type == "account").FirstOrDefault();

                // Connect to the IMAP server. The 'true' parameter specifies to use SSL
                // which is important (for Gmail at least)
                ImapClient imapClient = new ImapClient("imap.gmail.com", myAccountEmail.Value, result.credential.Token.AccessToken,
                                ImapClient.AuthMethods.SaslOAuth, 993, true);

                var mailBox = imapClient.SelectMailbox("FNB");

                Console.WriteLine("ListMailboxes");

                Console.WriteLine(mailBox.Name);
                var original = Console.ForegroundColor;

                var examine = imapClient.Examine(mailBox.Name);
                if (examine != null)
                {
                    Console.WriteLine(" - Count: " + imapClient.GetMessageCount());

                    //Get One Email per folder

                    MailMessage[] mm = imapClient.GetMessages(0, imapClient.GetMessageCount());
                    Console.ForegroundColor = ConsoleColor.Blue;
                    var counter = 0;
                    foreach (MailMessage m in mm)
                    {
                        if (m.Date > DateTime.Now.AddDays(-30))
                        {
                            if (m.Subject.Contains("reserved for purchase") || m.Subject.Contains("paid from cheq"))
                            {
                                
                                var substring = m.Subject.Substring(7, 10);
                                var number = Regex.Split (substring, @"[^0-9\.]+")[1];
                                var amount = Convert.ToDouble(number, CultureInfo.InvariantCulture);
                                var email = new Expenses() { PaymentType = PaymentType.Bill, Name = m.Subject, Amount = amount, Id = m.MessageID, PaymentDate = m.Date };
                                if(m.Subject.Contains("paid from cheq"))
                                {
                                    email.PaymentType = PaymentType.Buy;
                                }
                                emailModels.Add(email);
                                counter++;
                            }
                        }

                    }
                   
                }
                else
                {
                 
                }

                imapClient.Dispose();
            }
            return emailModels;

        }
    }
}
