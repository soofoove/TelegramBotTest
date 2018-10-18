using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace webApp.Extensions{
    public static class ServiceExtensions{
        public static IServiceCollection AddTelegramBot(this IServiceCollection services){
            return services.AddSingleton<TelegramBotClient>(provider => {
                var config = provider.GetService<IConfiguration>();
                
                var client = new TelegramBotClient(config["botToken"]);
                client.DeleteWebhookAsync();
                client.SetWebhookAsync(config["webhook_url"]);
                return client;
            });
        }
    }
}