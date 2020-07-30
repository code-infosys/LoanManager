using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System; 
using System.IO; 
using System.Security.Cryptography;
using System.Text; 
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LoanManager.Web
{
    public static class AppContext
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static HttpContext Current => _httpContextAccessor.HttpContext;
    }
    public static class Env
    { 
        public static string GetUserInfo(string value, IEnumerable<Claim> Claim)
        { 
            string ReturnVal = string.Empty;
            
            foreach (var item in Claim)
            {
                if (value == "Id" && item.Type == ClaimTypes.NameIdentifier)
                    ReturnVal = item.Value;
                if (value == "UserName" && item.Type == ClaimTypes.Name)
                    ReturnVal = item.Value;
                if (value == "Role" && item.Type == ClaimTypes.Role)
                    ReturnVal = item.Value;
                if (value== "RoleId" && item.Type == ClaimTypes.Rsa)
                    ReturnVal = item.Value;

                //if (value == "DateOfBirth" && item.ValueType == ClaimValueTypes.DateTime)
                //    ReturnVal = item.Value;

            }


            return ReturnVal;

        }
         

        public static HtmlString AppInfo(string value, IEnumerable<Claim> Claim)
        {
            string ReturnVal = string.Empty;

            foreach (var item in Claim)
            {
                if (item.Type == ClaimTypes.Webpage)
                {
                    try
                    {
                        string[] claimValue = item.Value.Split('#').Where(i => i.Split('=')[0] == value).ToArray();
                        ReturnVal = claimValue[0].Split('=')[1];
                    }
                    catch (Exception)
                    {
                        ReturnVal = "";
                    }

                }
            }

            return new HtmlString(ReturnVal);
        }
         public static string Encrypt(string text, string keyString="")
        {
            keyString = "E546C8DF278CD5931069B522E6953332";
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string keyString = "")
        {
            keyString = "E546C8DF278CD5931069B522E6953332";
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length]; //changes here

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            // Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length); // changes here
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }









	//turn on: https://myaccount.google.com/u/0/lesssecureapps
        public static void SendEmail(string ToUser, string subject, string body
            ,string username,string password,string port,string smtpServer)
        {
            var toAddress = new System.Net.Mail.MailAddress(ToUser, ToUser);

            try
            {
                System.Net.Mail.MailMessage emessage = new System.Net.Mail.MailMessage();
                  
                emessage.To.Add(toAddress);
                emessage.Subject = subject;
                emessage.From = new System.Net.Mail.MailAddress(username);
                emessage.Body = body;
                emessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient();
                var netCredential = new System.Net.NetworkCredential(username, password);
                sc.Host = smtpServer;
                sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = netCredential;
                sc.EnableSsl = true;
                sc.Port = Convert.ToInt32(port);
                sc.Send(emessage);

            }
            catch (Exception ex)
            {

            }
        }
  
        public static Int32 ToInt32(this string val)
        {
            return Convert.ToInt32(val);
        }

        public static string ToTimeAgo(this DateTime date)
        {
            var dateIndia = DateTime.Now;
            TimeSpan timeSince = dateIndia.Subtract(Convert.ToDateTime(date));
            //TimeSpan timeSince = DateTime.Now.Subtract(Convert.ToDateTime(date));

            if (timeSince.TotalMilliseconds < 1)
                return "not yet";
            if (timeSince.TotalMinutes < 1)
                return "just now";
            if (timeSince.TotalMinutes < 2)
                return "1 minute ago";
            if (timeSince.TotalMinutes > 5 && timeSince.TotalMinutes < 60)
                return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes > 60 && timeSince.TotalMinutes < 80)
                return "1 hour ago";
            if (timeSince.TotalHours < 24)
                return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays == 1)
                return "yesterday";
            if (timeSince.TotalDays < 7)
                return string.Format("{0} days ago", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "last week";
            if (timeSince.TotalDays < 21)
                return "2 weeks ago";
            if (timeSince.TotalDays < 28)
                return "3 weeks ago";
            if (timeSince.TotalDays < 60)
                return "last month";
            if (timeSince.TotalDays < 100)
                return "2 months ago";
            if (timeSince.TotalDays < 365)
                return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "last year";

            //last but not least...
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));

        }
        public static string ToSpaceBeforeUpperChar(this string val)
        {
            string ch = string.Concat(val.Select(i => Char.IsUpper(i) ? " " + i : i.ToString())).TrimStart(' ');
            return ch;
        }

        public static DateTime ToTimeZoneDate(this DateTime datetime)
        {
            return GetTimeZoneInfo(datetime);
        }

        public static DateTime GetTimeZoneInfo(DateTime datetime)
        {
            DateTime date = DateTime.Now;
            try
            {
                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(Startup.StaticConfig["ApplicationSettings:TimeZone"].ToString());
                date = TimeZoneInfo.ConvertTime(datetime, zone);
            }
            catch (Exception)
            {
                date = DateTime.Now;
            }

            return date;
        }

        public static Task<string> GetUploadedFilePath(IFormFile mediaUrl, bool IsUseRoot = false)
        {
            string ymdPath, getExt, fileName, dirPath, filePath;
            getExt = Path.GetExtension(mediaUrl.FileName);
            var date = Env.GetTimeZoneInfo(DateTime.Now);
            ymdPath = "/" + date.Year + "/" + date.ToString("MMM") + "/" + date.Day + "/";
            //ymdPath = Path.Combine(date.Year.ToString(), date.ToString("MMM"), date.Day.ToString());
            fileName = date.ToString("ddMMMyyyyhhmmss") + getExt;
            dirPath = "wwwroot/uploads" + ymdPath;
            //dirPath = Path.Combine("wwwroot", "uploads", ymdPath);
            var serverMap = AppDomain.CurrentDomain.BaseDirectory;
            if (Startup.StaticConfig["ApplicationSettings:IsDevelopment"].ToString() == "Yes")
                serverMap = Directory.GetCurrentDirectory();

            string dirCreatePath = Path.Combine(serverMap, dirPath);
            if (!Directory.Exists(dirCreatePath) && IsUseRoot == false)
            {
                Directory.CreateDirectory(dirCreatePath);
            } 

            filePath = "wwwroot/uploads" + ymdPath + fileName;
            //filePath = Path.Combine("wwwroot", "uploads", ymdPath, fileName);
             
            var path = Path.Combine(serverMap, filePath);
            if (IsUseRoot)
                path = Path.Combine(serverMap, "wwwroot/uploads", mediaUrl.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                mediaUrl.CopyTo(stream);
            }
            if (IsUseRoot)
                return Task.FromResult(mediaUrl.FileName);
            else
                return Task.FromResult((ymdPath+fileName));
            //return Task.FromResult(Path.Combine(ymdPath,fileName));
        }
    }
}

