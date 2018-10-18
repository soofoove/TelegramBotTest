using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace webApp.Extensions{
    public static class ServiceExtensions{
        public static IServiceCollection AddTelegramBot(this IServiceCollection services, string token){
            return services.AddSingleton<TelegramBotClient>(provider => {
                return new TelegramBotClient(token);
            });
        }
    }
}