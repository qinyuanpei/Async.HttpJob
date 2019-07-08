using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncHttpJob.Extends;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.MySql.Core;
using Serilog;
using Serilog.Events;
using Serilog.Configuration;

namespace AsyncHttpJob
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire")));
            services.AddHangfire(x => 
                x.UseStorage(new MySqlStorage(Configuration.GetConnectionString("Hangfire")))
                    .UseFilter(new HttpJobFilter())
                    .UseSerilogLogProvider()
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMvcWithDefaultRoute();
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                Queues = new string[] { "default" },
                WorkerCount = 5,
                ServerName = "default",
            });
            app.UseHangfireDashboard();
            app.ApplicationServices.GetService<ILoggerFactory>().AddSerilog();
        }
    }
}
