using Hahn.ApplicatonProcess.December2020.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface;
using Microsoft.EntityFrameworkCore;
using Hahn.ApplicatonProcess.December2020.Data;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

namespace Hahn.ApplicatonProcess.December2020.Web
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
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                c.AddPolicy("AllowHeader", options => options.AllowAnyHeader());
                c.AddPolicy("AllowMethod", options => options.AllowAnyMethod());
            });
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("ApplicantDB"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //var provider = services.BuildServiceProvider().GetRequiredService<IApiDescriptionProvider>();
                //foreach (var desc in provider.Order)
                //{

                //}
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hahn.ApplicatonProcess.December2020.Web", Version = "v1" });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hahn Applicant API",
                    Description = "A Web API which exposes data and also stores,updates and deletes Applicant information",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Shayne Boyer",
                    //    Email = string.Empty,
                    //    Url = new Uri("https://twitter.com/spboyer"),
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    Url = new Uri("https://example.com/license"),
                    //}
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //var xmlPat2= Path.Combine("D:\Projects\Hahn\Hahn.ApplicatonProcess.Application\Hahn.ApplicatonProcess.December2020.Domain\Hahn.ApplicatonProcess.December2020.Domain.xml")
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlPath.Replace("Web","Domain"));
            });
            services.TryAddTransient<IApplicantService, ApplicantService>();
            services.TryAddTransient<IApplicantRepository, InMemoryApplicantRepository>();
            services.TryAddTransient<AbstractValidator<Applicant>, ApplicantValidator>();
            services.TryAddTransient<IValidator<Applicant>, ApplicantValidator>();
            services.AddScoped<ApplicantValidator>();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicantValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IApiDescriptionProvider prov, IServiceProvider serviceProvider)
        {
            //if (env.IsDevelopment())
            //{
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //app.addRequestIdHeader()
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });
                //app.UseSwagger();
                app.UseSwaggerUI(c =>   
                {
                    //c.DocExpansion(DocExpansion.None);
                    //var swaggerFilePath = Path.Combine(env.WebRootPath ?? "", "swagger.json");
                    //foreach(var desc in prov.ApiVersionDescriptions)
                    //{

                    //}
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn Applicant Api v1");
                    //c.RoutePrefix = string.Empty;
                }) ;
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var context = serviceProvider.GetService<ApiContext>();
            AddTestData(context);
            //app.UseMvc();
        }
        private static void AddTestData(ApiContext context)
        {
            var testUser1 = new Applicant
            {
                ID=1,
	            Name="George George",
	            FamilyName="Skywalker",
	            Address="103,Outer Nebula",
	            CountryOfOrigin="Germany",
	            EMailAddress="George@ger.com",
	            Age=30,
	            Hired=true,
                LastUpdated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt")
            };

            context.Applicants.Add(testUser1);

            var testUser2 = new Applicant
            {
                ID = 2,
                Name = "Tobias Luke",
                FamilyName = "LukeWarm",
                Address = "10,Outer Neptune",
                CountryOfOrigin = "India",
                EMailAddress = "Luke@ger.com",
                Age = 26,
                Hired = false,
                LastUpdated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt")
            };

            context.Applicants.Add(testUser2);

            context.SaveChanges();
        }
    }
}
