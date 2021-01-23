using AutoMapper;
using DAL.CQRS;
using DAL.CQRS.Brokers.ReportBroker;
using DAL.CQRS.Reports;
using DAL.Models;

using DAL.Repositories;
using DAL.Services;
using EmployeeTimeSheet.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeTimeSheet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<employeetimesheetdbContext>(options =>
            options.UseMySql(
                Configuration.GetConnectionString("DefaultConnection")));
            


            services.AddIdentity<Aspnetusers, IdentityRole>()
                .AddEntityFrameworkStores<employeetimesheetdbContext>();


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IProjectSQLRepository, ProjectSQLRepository>();
            services.AddScoped<IProjectInfoSQLRepository, ProjectInfoSQLRepository>();
            services.AddScoped<IActivitySQLRepository, ActivitySQLRepository>();
            services.AddScoped<IProjectFacade, ProjectFacade>();

            services.AddScoped<IUserSQLRepository, UserSQLRepository>();

            services.AddScoped<IReportFacade, ReportFacade>();


            services.AddScoped<IBroker, Broker>();
            services.AddTransient<IQuery, GetTotalWorkingHoursByProjectAsync>();



            services.AddControllersWithViews();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                name: "MyAdminArea",
                areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapAreaControllerRoute(
                name: "MySupervisorArea",
                areaName: "Supervisor",
                pattern: "Supervisor/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
