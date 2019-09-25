using ELOTEC.Infrastructure.Helpers;
using ELOTEC.Infrastructure.Helpers;
using ELOTEC.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace ELOTEC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DataBaseConnectionProvider._strconnection = getConnectionString();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ConfigHelper.AppSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ELOTEC API",
                    Description = "Test.",
                    Contact = new Contact
                    {
                        Name = "Prista Tech Solutions",
                        Email = string.Empty
                    }
                });

                c.CustomSchemaIds(y => y.FullName);
                c.DocInclusionPredicate((version, apiDescription) => true);
                c.EnableAnnotations();
            });

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Helper.ContentRootPath = env.ContentRootPath;

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ELOTEC API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public string getConnectionString()
        {
            return this.Configuration.GetSection("ConnectionString").GetSection("ELOTECDB").Value;
        }
    }
}
