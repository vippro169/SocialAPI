using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Authentication;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using SocialNetwork.Persistence.MySql.Authentication;
using SocialNetwork.Service.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace SocialNetwork
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
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>(n => new ApplicationDbContext(Configuration["MySql:ConnectionString"]));
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();

            services.AddTransient<IJwtTokenOptions, JwtTokenOptions>(o => new JwtTokenOptions(Configuration["Jwt:Issuer"], Configuration["Jwt:Audience"], Configuration["Jwt:SecurityKey"], Int32.Parse(Configuration["Jwt:ExpireHours"])));
            services.AddTransient<IAuthenticationHandler, AuthenticationHandler>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationFilter));
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]))
                    };
                });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Privacy Social Network API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Privacy Social Network API V1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
