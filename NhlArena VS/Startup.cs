using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameLogic;
using Clients;

namespace NhlArena_VS
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(360);
                options.Cookie.HttpOnly = true;
            });

            //services.AddMvc(options => options.MaxModelValidationErrors = 50);

            services.AddMvc(options => options.MaxModelValidationErrors = 50).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();

            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".mtl"] = "text/plain";
            provider.Mappings[".obj"] = "text/plain";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.ToString().Contains("/connect_client"))
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        string[] pathContents = context.Request.Path.ToString().Split('/');

                        //checks if path contains more than just connect_client, and if there is a session with an logged in user.
                        if (pathContents[2] != null && !string.IsNullOrEmpty(context.Session.GetString("id")) && !string.IsNullOrEmpty(context.Session.GetString("username")))
                        {
                            if (pathContents[2] == "newGame")
                            {
                                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                                string username = context.Session.GetString("username");

                                Client cs = new Client(webSocket, username);
                                GameManager.ManageClient(cs);
                                await cs.StartReceiving();
                            }
                            else
                            {
                                try
                                {
                                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                                    string username = context.Session.GetString("username");
                                    Guid gameId = new Guid(pathContents[2]);
                                    
                                    Client cs = new Client(webSocket, username, gameId);
                                    GameManager.ManageClient(cs);
                                    await cs.StartReceiving();
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e);
                                }
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }


            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions());
        }
    }
}

