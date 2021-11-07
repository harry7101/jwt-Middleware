using IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace jwt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext().MinimumLevel.Information()
             .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:30200/"))
             {
                 AutoRegisterTemplate = true,
                 AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                 IndexFormat = "abp-order-log-{0:yyyy.MM}",
                 FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                 EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                   EmitEventFailureHandling.WriteToFailureSink |
                                   EmitEventFailureHandling.RaiseCallback,
                 FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null)
             }).WriteTo.Console()
             .CreateLogger();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "Issuer",

                        ValidateAudience = true,
                        ValidAudience = "Audience",

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{069BD1DF-72D8-474B-8950-2C3EB03B2D03}")),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(30),
                    };
                });

          //  services.AddTransient<IServcieTestA, ServiceTestA>();
          //  services.AddSingleton<IServcieTestB, ServiceTestB>();
            // services.AddScoped<IServcieTestA, ServiceTestA>();
      


        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ServiceTestA>().As<IServcieTestA>();
           
            containerBuilder.RegisterType<ServiceTestB>().As<IServcieTestB>();
            containerBuilder.RegisterType<ServiceTestC>().As<IServcieTestC>().PropertiesAutowired();

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IServcieTestA>()));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            app.UseIP();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
