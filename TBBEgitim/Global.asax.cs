using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TBBEgitim.Jobs;

namespace TBBEgitim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IScheduler _scheduler;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Quartz scheduler ayarı
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            _scheduler.Start();

            // Job tanımı
            IJobDetail job = JobBuilder.Create<TBBEgitim.Jobs.BackupJob>()
                .WithIdentity("backup-sifreler")
                .Build();

            // Trigger tanımı (her gün 15:00 Türkiye saatiyle)
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("backup-sifreler-trigger")
                .WithCronSchedule("0 37 09 * * ?", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")))
                .Build();

            _scheduler.ScheduleJob(job, trigger);

            // Test için
            //_scheduler.TriggerJob(new JobKey("backup-sifreler")).Wait();
        }

        protected void Application_End()
        {
            _scheduler?.Shutdown(waitForJobsToComplete: true);
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            if (httpException == null)
            {
                routeData.Values["action"] = "General";
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "NotFound";
                        break;
                    default:
                        routeData.Values["action"] = "General";
                        break;
                }
            }

            Server.ClearError();
            IController errorController = new Controllers.ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }


    }
}
