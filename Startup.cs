using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using webApp.Extensions;

using Telegram.Bot;

namespace webApp
{
    public class Startup
    {
        public IConfiguration AppConfig { get; set; }
        public Startup(IConfiguration config)
        {
            AppConfig = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot(AppConfig["botToken"]);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TelegramBotClient client)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>{
                var me = await client.GetMeAsync();
                
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"<h1>My name is {me.FirstName}</h1>");
            });            
        }
    }


}
