using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Wisej.Web;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Data;

namespace Captain.Common.Utilities
{
    public class TwilloSendGridEmail
    {
        public string Emailstatus = "";
        public string EmailstatusDate = "";
        public async Task SendEmailAsync(string email, string _subject, string _HtmlbodyContent, string ServerName, string DBName,string Purpose, string SuccessMessage)
        {
            Application.ShowLoader = true;
            Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "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");
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("captain_AgencyManagement@capsystems.com", DBName);
            var to = new EmailAddress(email);

            var subject = _subject;
            var plainTextContent = "";
            var htmlContent = _HtmlbodyContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            
            var _id = await getMaxEmailIDAPI(); //getMaxEmailID(); // Guid.NewGuid().ToString();
            

            msg.AddCustomArg("EMAIL_ID", _id.ToString());
            msg.AddCustomArg("EMAIL_SERVER_NAME", ServerName);
            msg.AddCustomArg("EMAIL_DB_NAME", DBName);
            msg.AddCustomArg("EMAIL_PURPOSE", Purpose);
            msg.AddCustomArg("EMAIL", to.ToString());

            var response = await client.SendEmailAsync(msg);

            //var _id = "1";
            string recordStatus = await WaitForRecordAsync(_id.ToString());

            if (recordStatus != "")
            {
                List<EmailLog> dtLogs = ConvertJsonToDataTable(recordStatus);
                if (dtLogs.Count > 0)
                {
                    string _status = dtLogs[0].EMAIL_STATUS.ToString();
                    if (_status == "delivered")
                    {
                        Emailstatus = _status;
                        EmailstatusDate = dtLogs[0].EMAIL_DATE.ToString();
                        Application.ShowLoader = false;
                        //Application.Update();
                        AlertBox.Show(SuccessMessage);
                    }
                    else
                    {
                        Emailstatus = _status;
                        EmailstatusDate = dtLogs[0].EMAIL_DATE.ToString();
                        Application.ShowLoader = false;
                        //Application.Update(this);
                        AlertBox.Show("email bounced",MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    Emailstatus = "bounced";
                    EmailstatusDate = DateTime.Now.ToString();

                    Application.ShowLoader = false;
                    //Application.Update(this);
                    AlertBox.Show("email bounced", MessageBoxIcon.Warning);
                }
            }
            else
            {
                Emailstatus = "Failed";
                EmailstatusDate = DateTime.Now.ToString();

                Application.ShowLoader = false;
                //Application.Update(this);
                AlertBox.Show("email failed to send", MessageBoxIcon.Warning);
            }
            //Console.WriteLine($"Email sent. Status code: {response.StatusCode}");
        }

        public async Task<int> getMaxEmailIDAPI(int maxWaitTimeSeconds = 20)
        {
            int elapsedTime = 0;
            int pollingInterval = 1000; // Poll every 1 second
            int _result = 0;
            while (elapsedTime < maxWaitTimeSeconds * 1000)
            {
                string recordExists = await GetApiDataAsync($"https://capsysdev.capsystems.com/SendgridAPI/api/capwebhook/getall");  //CheckIfRecordExistsInDatabaseAsync(emailId);
                //string recordExists = await GetApiDataAsync($"https://localhost:44307/api/capwebhook/getall"); 
                if (recordExists != "" && recordExists != "\"\"")
                {
                    List<EmailLog> dtLogs = ConvertJsonToDataTable(recordExists);
                    if (dtLogs.Count > 0)
                    {
                        // Get the maximum ID value using LINQ
                        int maxId = dtLogs.Max(item => Convert.ToInt32(item.EMAIL_ID));
                        _result = maxId + 1;
                        return _result;

                    }
                    else
                    {
                        _result = 1;
                        return _result;
                    }
                }
                //else
                //{
                //    _result = 1;
                //    return _result;
                //}
                await Task.Delay(pollingInterval);
                elapsedTime += pollingInterval;
            }

            return _result;
        }

        private static readonly HttpClient client = new HttpClient();
        public async Task<string> GetApiDataAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throws if not 200-299

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<string> WaitForRecordAsync(string emailId, int maxWaitTimeSeconds = 20)
        {
            int elapsedTime = 0;
            int pollingInterval = 1000; // Poll every 1 second
            string _result = "";
            while (elapsedTime < maxWaitTimeSeconds * 1000)
            {
                string recordExists = await GetApiDataAsync($"https://capsysdev.capsystems.com/SendgridAPI/api/capwebhook/Getbyid?emailid=" + emailId + ""); //CheckIfRecordExistsInDatabaseAsync(emailId);
                //string recordExists = await GetApiDataAsync($"https://localhost:44307/api/capwebhook/Getbyid?emailid=" + emailId + "");
                if (recordExists != "" && recordExists != "\"\"")
                {
                    _result = recordExists;
                    return _result; // true;
                }

                await Task.Delay(pollingInterval);
                elapsedTime += pollingInterval;
            }

            return _result; //false; // Record was not found within the timeout period
        }
        public static List<EmailLog> ConvertJsonToDataTable(string jsonString)
        {
            try
            {
                string jsonFormatted = jsonString.Replace("\\r\\n", "\n").Replace("\\\"", "\"");
                jsonFormatted = jsonFormatted.Trim('"');
                // Step 2: Replace escaped sequences with actual characters
                jsonFormatted = jsonFormatted.Replace("\\\"", "\"").Replace("\\r\\n", "\n");

                // Step 3: Deserialize the cleaned JSON string to a DataTable
                DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonFormatted);

                JavaScriptSerializer json = new JavaScriptSerializer();
                List<EmailLog> objArray = json.Deserialize<List<EmailLog>>(jsonFormatted);
                return objArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


    public class EmailLog
    {
        public string EMAIL_ID { get; set; }
        public string EMAIL_SERVER_NAME { get; set; }
        public string EMAIL_DB_NAME { get; set; }
        public string EMAIL_PURPOSE { get; set; }
        public string EMAIL { get; set; }
        public string EMAIL_STATUS { get; set; }
        public string EMAIL_DATE { get; set; }
    }
}
