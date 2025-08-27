using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace TBBEgitim.Jobs
{
    public class BackupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // HttpContext yoksa bile çalışabilen yollar 
            string Map(string v) => HostingEnvironment.MapPath(v);

            string logOk = Map("~/App_Data/backup-log.txt");
            string logErr = Map("~/App_Data/backup-error.txt");

            try
            {
                string sourcePath = Map("~/App_Data/sifreler.txt");
                if (!File.Exists(sourcePath))
                {
                    File.AppendAllText(logErr, $"[HATA] Kaynak dosya yok: {sourcePath} - {DateTime.Now}\n");
                    return;
                }

                string backupDir = Map("~/App_Data/Backup");
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string backupFile = Path.Combine(
                    backupDir,
                    $"sifreler-backup-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
                );

                File.Copy(sourcePath, backupFile, true);

                File.AppendAllText(logOk, $"[OK] Backup alındı: {backupFile} - {DateTime.Now}\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText(logErr, $"[HATA] {ex} - {DateTime.Now}\n");
            }

            await Task.CompletedTask;
        }
    }
}
