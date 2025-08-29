using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TBBEgitim.Helpers;
using TBBEgitim.Models;

namespace TBBEgitim.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            //Session var mı
            if (Session["User"] != null)
            {
                return RedirectToAction("Anasayfa", "Giriş");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == "" || password == "")
            {
                ViewBag.Error = "Kullanıcı adı veya şifre boş olamaz!";
                return View();
            }
            //Dosyadaki satırları oku
            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/sifreler.txt"));

            string hashedPassword = BitConverter.ToString(SHA256HashHelper.ComputeSHA256Hash(password)).Replace("-", "").ToLowerInvariant();

            //Kullanıcı adı ve şifreyi kontrol
            var validUser = (lines
                .Select(line => line.Split(' '))
                .Any(parts => parts[0] == username && parts[1] == hashedPassword) 
                || (username == ConfigurationManager.AppSettings["superAdmin"] && hashedPassword == ConfigurationManager.AppSettings["superPassword"]));

            if (validUser)
            {
                Session["User"] = username;
            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }
            var recaptchaToken = Request["g-recaptcha-response"];
            // Captcha kontrolü
            if (!RecaptchaHelper.Validate(recaptchaToken))
            {
                ViewBag.Error = "Lütfen reCAPTCHA doğrulamasını tamamlayın.";
                return View();
            }
            return RedirectToAction("Anasayfa", "Giriş"); // başarılı giriş

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult AdminPanel()
        {
            if (Session["User"].ToString() != ConfigurationManager.AppSettings["superAdmin"])
            {
                return RedirectToAction("AuthorizationError");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AdminPanel(string username, string password)
        {
            //Kullanıcı adı veya şifre boş mu
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Kullanıcı adı veya şifre boş olamaz.";
                return View();
            }

            //Kullanıcı adında boşluk var mı
            if (username.Contains(" "))
            {
                ViewBag.Error = "Kullanıcı adında boşluk olamaz.";
                return View();
            }

            if(username == ConfigurationManager.AppSettings["superAdmin"])
            {
                ViewBag.Error = "Yönetici kullanıcı adında kullanıcı eklenemez!";
                return View();
            }
            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/sifreler.txt"));

            //Kullanıcı adı ve şifre doğru mu
            var nonValidUser = (lines.Select(line => line.Split(' ')).Any(parts => parts[0] == username));

            if (nonValidUser)
            {
                ViewBag.Error = "Bu kullanıcı adı zaten var!";
                return View();
            }

            // Şifreyi hashle
            var hashedPassword = BitConverter
                .ToString(SHA256HashHelper.ComputeSHA256Hash(password))
                .Replace("-", "")
                .ToLowerInvariant();

            var newLine = $"{username} {hashedPassword}";

            // Dosyaya ekle
            var filePath = Server.MapPath("~/App_Data/sifreler.txt");
            System.IO.File.AppendAllLines(filePath, new[] { newLine });
            ViewBag.Success = "Kullanıcı başarıyla eklendi!";
            return View();
        }
        public ActionResult AuthorizationError()
        {
            return View();
        }


        public ActionResult LogData()
        {
            //bu sayfaya sadece superadmin erişebilir
            if (Session["User"].ToString() != ConfigurationManager.AppSettings["superAdmin"])
            {
                return RedirectToAction("AuthorizationError");
            }

            var logPath = Server.MapPath("~/App_Data/backup-log.txt");

            var logs = new List<BackupLog>();

            //dosyayı gerektiği gibi ayır
            if (System.IO.File.Exists(logPath)){
                var lines = System.IO.File.ReadAllLines(logPath);
                var regex = new Regex(@"Backup\\(.*?) - (\d{2}\.\d{2}\.\d{4}) (\d{2}:\d{2}):\d{2}");

                foreach (var line in lines)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        logs.Add(new BackupLog
                        {
                            DosyaAdi = match.Groups[1].Value,
                            KayitTarihi = match.Groups[2].Value,
                            KayitSaati = match.Groups[3].Value,
                            Basarili = "Başarılı"
                        });
                    }
                }
            }
            var errorLogPath = Server.MapPath("~/App_Data/backup-error.txt");
            if (System.IO.File.Exists(errorLogPath)){

                var lines = System.IO.File.ReadAllLines(logPath);
                var regex = new Regex(@"Backup\\(.*?) - (\d{2}\.\d{2}\.\d{4}) (\d{2}:\d{2}):\d{2}");

                foreach (var line in lines)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        logs.Add(new BackupLog
                        {
                            DosyaAdi = match.Groups[1].Value,
                            KayitTarihi = match.Groups[2].Value,
                            KayitSaati = match.Groups[3].Value,
                            Basarili = "Başarısız"
                        });
                    }
                }
            }

            return View(logs);
        }
    }
}
