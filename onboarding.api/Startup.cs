using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using onboarding.bll;
using onboarding.bll.Cache;
using onboarding.bll.Kafka;
using onboarding.dal;
using onboarding.dal.Repositories;
using onboarding.Scheduler;
using onboarding.Scheduler.Job;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace onboarding.api
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
            services.AddControllers();

            services.AddDbContext<DwikiDbContext>(options =>
              options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tutorial Net Core", Version = "v1" });
            });

            services.AddTransient<MovieService>();

            services.AddTransient<NationService>();

            services.AddAutoMapper(typeof(AutoMapperProfileConfiguration));

            services.AddSingleton<IkafkaSender, KafkaSender>();
            services.AddSingleton<IJobFactory, QuartzJobFactory>();

            services.AddHostedService<SchedulerService>();

            services.AddTransient<LogTimeJob>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tutorial Net Core v1");
            });

        }
    }
}
