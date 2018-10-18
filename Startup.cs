using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using webApp.Extensions;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Newtonsoft.Json;

namespace webApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot();
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TelegramBotClient client)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapPost("api/update", async context =>{
                StreamReader sr = new StreamReader(context.Request.Body);
                string body = await sr.ReadToEndAsync();
                Telegram.Bot.Types.Update update = JsonConvert.DeserializeObject<Telegram.Bot.Types.Update>(body);
                
                switch(update.Type){
                    case UpdateType.Message:
                        await client.SendTextMessageAsync(update.Message.Chat.Id,
                            "Просто ответ на сообщение", replyToMessageId: update.Message.MessageId);
                        break;
                    
                    case UpdateType.EditedMessage:
                        await client.SendTextMessageAsync(update.EditedMessage.Chat.Id,
                            "Ответ на отредактированное сообщение", 
                            replyToMessageId: update.EditedMessage.MessageId);
                    break;
                }
                
            });

            app.UseRouter(routeBuilder.Build());

            app.Run(async context =>{
                var me = await client.GetMeAsync();
                
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"<h1>My name is {me.FirstName}</h1>");
            });            
        }
    }


}
