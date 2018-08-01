using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Data;
using ChatServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatServer
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
            //services.AddDbContext<MessagesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
            services.AddDbContext<MessageContext>(options => options.UseInMemoryDatabase("ChatDB"));
            //services.AddSingleton(IMessageContext, MessageContext);
            services.AddScoped<IMessageService, MessageService>();

            services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("UsersDB"));

            services.AddIdentity<IdentityUser, IdentityRole>();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "api1";
                });

            //    services.AddAuthentication(options =>
            //    {
            //        // Identity made Cookie authentication the default.
            //        // However, we want JWT Bearer Auth to be the default.
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //.AddJwtBearer(options =>
            //{
            //    // Configure JWT Bearer Auth to expect our security key
            //    options.TokenValidationParameters =
            //        new TokenValidationParameters
            //        {
            //            LifetimeValidator = (before, expires, token, param) =>
            //            {
            //                return expires > DateTime.UtcNow;
            //            },
            //            ValidateAudience = false,
            //            ValidateIssuer = false,
            //            ValidateActor = false,
            //            ValidateLifetime = true,
            //            //IssuerSigningKey = SecurityKey
            //        };

            //    // We have to hook the OnMessageReceived event in order to
            //    // allow the JWT authentication handler to read the access
            //    // token from the query string when a WebSocket or 
            //    // Server-Sent Events request comes in.
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            var accessToken = context.Request.Query["access_token"];

            //            // If the request is for our hub...
            //            var path = context.HttpContext.Request.Path;
            //            if (!string.IsNullOrEmpty(accessToken) &&
            //                (path.StartsWithSegments("/ChatHub")))
            //            {
            //                // Read the token out of the query string
            //                context.Token = accessToken;
            //            }
            //            return Task.CompletedTask;
            //        }
            //};
            //});

            services.AddMvc();

            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //builder =>
            //{
            //    builder.AllowAnyMethod().AllowAnyHeader()
            //           .WithOrigins("http://localhost:55830")
            //           .AllowCredentials();
            //}));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc();
        }
    }
}
