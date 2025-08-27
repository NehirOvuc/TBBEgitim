using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace TBBEgitim.Helpers
{
    public class RecaptchaVerifyResponse
    {
        public bool success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> error_codes { get; set; }
    }

    public static class RecaptchaHelper
    {
        public static bool Validate(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var secret = ConfigurationManager.AppSettings["RecaptchaSecretKey"];
            if (string.IsNullOrEmpty(secret)) return false;

            using (var client = new WebClient())
            {
                // v2 için success=true/false döner
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}";
                var json = client.DownloadString(url);
                var result = JsonConvert.DeserializeObject<RecaptchaVerifyResponse>(json);
                return result != null && result.success;
            }
        }
    }
}
